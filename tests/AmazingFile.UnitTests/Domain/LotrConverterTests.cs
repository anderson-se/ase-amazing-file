using AmazingFile.Application.Services;
using AmazingFile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AmazingFile.UnitTests.Domain;

public class LotrConverterTests
{
    private readonly LotrConverter _lotrConverter;

    public LotrConverterTests()
    {
        _lotrConverter = new LotrConverter();
    }

    [Fact]
    public async Task Should_ConvertItems()
    {
        // Arrange
        var fileLines = GetFileLines();

        // Act
        var lines = await _lotrConverter.ConvertLines(fileLines).ToListAsync();
        var line = (LotrItem)lines.Single(o => o is LotrItem);

        // Assert
    }

    [Fact]
    public async Task Should_ConvertHeader()
    {
        // Arrange
        var fileLines = GetFileLines();
        var provider = Guid.NewGuid().ToString();

        // Act
        var lines = await _lotrConverter.ConvertLines(fileLines).ToListAsync();
        var headerLines = lines.Where(o => o is LotrHeader).Cast<LotrHeader>();

        // Assert
        Assert.Equal(3, headerLines.Count());
    }

    private static async IAsyncEnumerable<string> GetFileLines()
    {
        yield return await Task.FromResult("");
    }
}
