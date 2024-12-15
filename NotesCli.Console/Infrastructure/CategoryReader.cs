using NotesCli.Console.Core;

namespace NotesCli.Console.Infrastructure;

class CategoryReader : ICategoryReader
{
    public Dictionary<string, string[]> Aliases { get; init; }

    public CategoryReader(Dictionary<string, string[]> aliases)
    {
        Aliases = aliases;
    }

    public List<Category> Read() => Aliases.Select(x => new Category(x.Key, x.Value)).ToList();
}
