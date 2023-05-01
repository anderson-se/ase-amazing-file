using System.ComponentModel;

namespace AmazingFile.Domain.Models;

public class LotrItem : IFileLine
{
    private LotrItem(string character, IList<string> nicknames, string actor, DateTime dateOfBirth)
    {
        Character = character;
        Nicknames = nicknames;
        Actor = actor;
        DateOfBirth = dateOfBirth;
    }

    public static LotrItem Create(string character, IList<string> nicknames, string actor, DateTime dateOfBirth)
        => new(character, nicknames, actor, dateOfBirth);

    [Description("character")]
    public string Character { get; }

    [Description("nicknames")]
    public IList<string> Nicknames { get; }

    [Description("actor")]
    public string Actor { get; }

    [Description("dateOfBirth")]
    public DateTime DateOfBirth { get; }

    public override string ToString()
    {
        return $"Character: {Character}, Nicknames: {string.Join(",", Nicknames)}, Actor: {Actor}, Date of Birth: {DateOfBirth}";
    }        
}
