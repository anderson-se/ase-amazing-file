using AmazingFile.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace AmazingFile.Application.Services;

public class FileService : IFileService
{
    private readonly IFileReader _fileReader;
    private readonly IFileWriter _fileWriter;
    private readonly IFileConverter _fileConverter;
    private readonly ILogger<FileService> _logger;

    public FileService(IFileReader fileReader, IFileWriter fileWriter, IFileConverter fileConverter, ILogger<FileService> logger)
    {
        _fileReader = fileReader;
        _fileWriter = fileWriter;
        _fileConverter = fileConverter;
        _logger = logger;
    }

    public async Task ConvertFile(string inputPath, string outputPath, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"[BEGIN] {nameof(ConvertFile)}");

        var fileLines = _fileReader.ReadLines(inputPath, cancellationToken: cancellationToken);
        var convertedLines = _fileConverter.ConvertLines(fileLines, cancellationToken);
        await _fileWriter.WriteLines(outputPath, convertedLines, cancellationToken);

        _logger.LogInformation($"[END] {nameof(ConvertFile)}");
    }
}
