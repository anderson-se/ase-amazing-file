using AmazingFile.Application.Services;
using AmazingFile.Domain.Models;
using AmazingFile.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AmazingFile.UnitTests.Services;

public class FileServiceTests
{
    private readonly FileService _fileService;
    private readonly Mock<IFileReader> _fileReaderMock;
    private readonly Mock<IFileWriter> _fileWriterMock;
    private readonly Mock<IFileConverter> _fileConverterMock;

    public FileServiceTests()
    {
        _fileReaderMock = new Mock<IFileReader>();
        _fileWriterMock = new Mock<IFileWriter>();
        _fileConverterMock = new Mock<IFileConverter>();
        _fileService = new FileService(_fileReaderMock.Object, _fileWriterMock.Object, _fileConverterMock.Object, Mock.Of<ILogger<FileService>>());
    }

    [Fact]
    public async Task Should_ConvertFile()
    {
        // Arrange
        string inputPath = Guid.NewGuid().ToString();
        string outputPath = Guid.NewGuid().ToString();

        var fileLines = GetFileLines();
        _fileReaderMock
            .Setup(o => o.ReadLines(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(fileLines);

        var formattedLines = GetFormattedLines();
        _fileConverterMock
            .Setup(o => o.ConvertLines(It.IsAny<IAsyncEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .Returns(formattedLines);

        _fileWriterMock
            .Setup(o => o.WriteLines(It.IsAny<string>(), It.IsAny<IAsyncEnumerable<IFileLine>>(), It.IsAny<CancellationToken>()));

        // Act
        await _fileService.ConvertFile(inputPath, outputPath);

        // Assert
        _fileReaderMock.Verify(o => o.ReadLines(inputPath, It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _fileConverterMock.Verify(o => o.ConvertLines(fileLines, It.IsAny<CancellationToken>()), Times.Once);
        _fileWriterMock.Verify(o => o.WriteLines(outputPath, formattedLines, It.IsAny<CancellationToken>()), Times.Once);
    }

    private static async IAsyncEnumerable<string> GetFileLines()
    {
        yield return await Task.FromResult(It.IsAny<string>());
    }

    private static async IAsyncEnumerable<IFileLine> GetFormattedLines()
    {
        yield return await Task.FromResult(It.IsAny<IFileLine>());
    }
}
