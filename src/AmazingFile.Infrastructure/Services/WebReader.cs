using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace AmazingFile.Infrastructure.Services;

public class WebReader : IFileReader
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IFileReader> _logger;

    public WebReader(IHttpClientFactory httpClientFactory, ILogger<IFileReader> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async IAsyncEnumerable<string> ReadLines(string url, int bufferSize = 81920, 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            throw new ArgumentException($"Invalid source path: {url}");
        }

        var httpClient = _httpClientFactory.CreateClient("file-reader");

        using var stream = await httpClient.GetStreamAsync(uri, cancellationToken);
        using var bufferedStream = new BufferedStream(stream, bufferSize);
        using var streamReader = new StreamReader(bufferedStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize, leaveOpen: true);

        while (await streamReader.ReadLineAsync() is string line)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("File read was interrupted.");
                break;
            }

            yield return line;
        }
    }
}
