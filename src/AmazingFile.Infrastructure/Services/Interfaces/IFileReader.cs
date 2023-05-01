namespace AmazingFile.Infrastructure.Services;

public interface IFileReader
{
    IAsyncEnumerable<string> ReadLines(string path, int bufferSize = 81920, CancellationToken cancellationToken = default);
}