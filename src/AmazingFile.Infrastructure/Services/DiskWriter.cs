using AmazingFile.Domain.Models;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;
using System.Text;

namespace AmazingFile.Infrastructure.Services;

public class DiskWriter : IFileWriter
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<IFileWriter> _logger;

    public DiskWriter(IFileSystem fileSystem, ILogger<IFileWriter> logger)
    {
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task WriteLines(string path, IAsyncEnumerable<IFileLine> lines, CancellationToken cancellationToken = default)
    {
        var fileInfo = _fileSystem.FileInfo.New(path);
        fileInfo.Directory?.Create();

        using var streamWriter = new StreamWriter(path, append: true, Encoding.UTF8, bufferSize: 1024 * 8);

        await foreach (var line in lines)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("File read was interrupted");
                break;
            }

            if (line is null)
                continue;

            await streamWriter.WriteLineAsync(line.ToString());
        }
    }
}
