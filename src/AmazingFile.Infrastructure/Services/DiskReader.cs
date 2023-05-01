using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace AmazingFile.Infrastructure.Services;

public class DiskReader : IFileReader
{
    private readonly ILogger<DiskReader> _logger;

    public DiskReader(ILogger<DiskReader> logger)
    {
        _logger = logger;
    }

    public async IAsyncEnumerable<string> ReadLines(string path, int bufferSize = 81920, 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var bufferedStream = new BufferedStream(stream, bufferSize);
        using var streamReader = new StreamReader(bufferedStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize, leaveOpen: true);

        while (await streamReader.ReadLineAsync() is string line)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("File read was interrupted");
                break;
            }

            yield return line;
        }
    }
}
