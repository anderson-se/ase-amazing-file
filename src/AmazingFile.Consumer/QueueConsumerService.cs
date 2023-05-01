using Amazon.SQS;
using Amazon.SQS.Model;
using AmazingFile.Application.Services;
using AmazingFile.Consumer.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AmazingFile.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IFileService _fileService;
    private readonly IOptions<QueueSettings> _queueSettings;

    public QueueConsumerService(IAmazonSQS sqs, IFileService fileService, IOptions<QueueSettings> queueSettings)
    {
        _sqs = sqs;
        _fileService = fileService;
        _queueSettings = queueSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, stoppingToken);
        var messageRequest = new ReceiveMessageRequest(queueUrlResponse.QueueUrl);

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqs.ReceiveMessageAsync(messageRequest, stoppingToken);

            foreach (var message in response.Messages)
            {
                var typedMessage = JsonSerializer.Deserialize<FileMessage>(message.Body)!;
                await _fileService.ConvertFile(typedMessage.InputPath, typedMessage.OutputPath, stoppingToken);
                await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
