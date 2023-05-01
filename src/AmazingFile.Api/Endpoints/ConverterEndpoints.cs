using AmazingFile.Api.Requests;
using AmazingFile.Application.Services;

namespace AmazingFile.Api.Endpoints;

public static class ConverterEndpoints
{
    public static void MapConverterEndpoints(this WebApplication app)
    {
        app!.MapPost("file/convert",
            async (IFileService fileService, FileRequest request, CancellationToken cancellationToken) =>
            await ConvertFile(fileService, request, cancellationToken));
    }

    public static async Task ConvertFile(IFileService fileService, FileRequest request, CancellationToken cancellationToken)
    {
        await fileService.ConvertFile(request.InputPath, request.OutputPath, cancellationToken);
    }
}
