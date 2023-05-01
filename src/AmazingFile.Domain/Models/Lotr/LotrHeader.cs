namespace AmazingFile.Domain.Models;

public class LotrHeader : IFileLine
{
    private LotrHeader(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public static LotrHeader Create(string key, string value) => new(key, value);

    public string Key { get; private set; }
    public string Value { get; private set; }

    public override string ToString() => $"#{Key}: {Value}";
}
