using AmazingFile.Application.Services;
using AmazingFile.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace AmazingFile.IoC;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFileConverterServices(this IServiceCollection services)
    {
        services.AddHttpClient("file-reader");
        services.AddHttpClient("file-writer");

        services.AddScoped<IFileSystem, FileSystem>();
        services.AddScoped<IFileReader, WebReaderBadMemoryUsage>();
        services.AddScoped<IFileWriter, DiskWriter>();
        services.AddScoped<IFileConverter, LotrConverter>();
        services.AddScoped<IFileService, FileService>();

        return services;
    }
}
