using Amazon.SQS;
using AmazingFile.Consumer;
using AmazingFile.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json");

var configuration = configurationBuilder.Build();
var timeoutInSeconds = configuration.GetValue<int>("Default:Timeout");

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(services =>
{
    services.AddFileConverterServices();
    services.Configure<QueueSettings>(configuration.GetSection(QueueSettings.Key));
    services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
    services.AddHostedService<QueueConsumerService>();
});

var host = builder.Build();
host.Run();
