namespace AmazingFile.Application.Services;

public interface IFileService
{
    Task ConvertFile(string inputPath, string outputPath, CancellationToken cancellationToken = default);
}