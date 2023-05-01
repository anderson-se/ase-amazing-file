using AmazingFile.Domain.Models;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AmazingFile.Application.Services;

public class LotrConverter : IFileConverter
{
    public async IAsyncEnumerable<IFileLine> ConvertLines(IAsyncEnumerable<string> lines,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var headerLine in GetHeaderLines())
        {
            yield return headerLine;
        }

        await foreach (var line in lines)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            if (string.IsNullOrWhiteSpace(line))
                continue;

            yield return CreateLotrLine(line);
        }
    }

    private static IEnumerable<IFileLine> GetHeaderLines()
    {
        var version = "1.0";
        var date = DateTime.Now.ToString();
        var descriptions = typeof(LotrItem).GetProperties()
            .Select(o => o.GetCustomAttribute<DescriptionAttribute>()!.Description);
        var fields = string.Join(' ', descriptions);

        return new[]
        {
            LotrHeader.Create("## Version", version),
            LotrHeader.Create("## Date", date),
            LotrHeader.Create("## Info", fields),
        };
    }

    private static LotrItem CreateLotrLine(string line)
    {
        string[] values = line.Split('|');

        string character = values[FieldPosition.Character];
        string[] nicknames = values[FieldPosition.Nicknames].Split(',', StringSplitOptions.RemoveEmptyEntries);
        string actor = values[FieldPosition.Actor];

        if (!DateTime.TryParse(values[FieldPosition.DateOfBirth], out var dateOfBirth))
        {
            dateOfBirth = DateTime.MinValue;
        }

        return LotrItem.Create(character, nicknames, actor, dateOfBirth);
    }
}
