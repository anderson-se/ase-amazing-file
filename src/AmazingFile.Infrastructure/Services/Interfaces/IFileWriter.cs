using AmazingFile.Domain.Models;

namespace AmazingFile.Infrastructure.Services;

public interface IFileWriter
{
    Task WriteLines(string path, IAsyncEnumerable<IFileLine> lines, CancellationToken cancellationToken = default);
}