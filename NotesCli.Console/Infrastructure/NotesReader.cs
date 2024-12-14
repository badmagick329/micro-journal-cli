using NotesCli.Console.Core;

namespace NotesCli.Console.Infrastructure;

class NotesReader : IDayNotesReader
{
    public string FilePath { get; init; }

    public NotesReader(string filePath)
    {
        FilePath = filePath;
    }

    public string[] Read() => File.ReadAllLines(FilePath);
}
