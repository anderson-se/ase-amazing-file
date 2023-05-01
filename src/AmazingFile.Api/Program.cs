using AmazingFile.Api.Endpoints;
using AmazingFile.Api.Middlewares;
using AmazingFile.IoC;
using FluentValidation;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = "AmazingFile.Api",
    Args = args,
});

builder.Services
    .AddFileConverterServices()
    .AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.MapConverterEndpoints();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.Run();
