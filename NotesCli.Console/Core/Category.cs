using NotesCli.Console.Common;

namespace NotesCli.Console.Core;

class Category
{
    private string _name { get; init; }
    public string Name => _name.Replace("_", " ").TitleCase();
    public IEnumerable<string> Aliases { get; init; }

    public Category(string name, IEnumerable<string> aliases)
    {
        _name = name;
        Aliases = aliases;
    }

    public bool IsAlias(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }

        var firstWord = text.Split(' ').First();
        firstWord = firstWord.EndsWith('.') ? firstWord[..^1] : firstWord;
        return Aliases.Contains(firstWord);
    }
}
