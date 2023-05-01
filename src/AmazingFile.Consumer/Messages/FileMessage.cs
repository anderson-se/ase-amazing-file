namespace AmazingFile.Consumer.Messages;

public class FileMessage
{
    public string InputPath { get; init; } = default!;
    public string OutputPath { get; init; } = default!;
}
