namespace AmazingFile.Api.Requests;

public class FileRequest
{
    public string InputPath { get; init; } = default!;
    public string OutputPath { get; init; } = default!;
}
