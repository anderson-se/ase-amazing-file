using AmazingFile.Domain.Models;

namespace AmazingFile.Application.Services;

public interface IFileConverter
{
    IAsyncEnumerable<IFileLine> ConvertLines(IAsyncEnumerable<string> lines, CancellationToken cancellationToken);
}