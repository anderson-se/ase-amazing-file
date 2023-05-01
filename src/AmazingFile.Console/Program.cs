using AmazingFile.Application.Services;
using AmazingFile.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;

var builder = new ConfigurationBuilder();
builder.AddJsonFile("appsettings.json");

var configuration = builder.Build();
var timeoutInSeconds = configuration.GetValue<int>("Default:Timeout");

var services = new ServiceCollection();
services.AddFileConverterServices();

var serviceProvider = services.BuildServiceProvider();

if (args.Length != 2)
{
    Console.WriteLine($"Please, inform: inputPath and outputPath");
    return;
}

try
{
    var fileService = serviceProvider.GetService<IFileService>();
    var cts = new CancellationTokenSource(delay: TimeSpan.FromMinutes(timeoutInSeconds));
    await fileService.ConvertFile(args[0], args[1], cts.Token);

    Console.WriteLine($"File was successfully converted to the specified format. Destination: {args[1]}");
}
catch (IOException e)
{
    Console.WriteLine($"An error occurred while reading/writing a file. Details: {e.Message}");
}
catch (HttpRequestException e)
{
    Console.WriteLine($"An error occurred while downloading/uploading a file. Details: {e.Message}");
}
catch (Exception e)
{
    Console.WriteLine($"Application exited with unexpected error. Details: {e.Message}");
}
