using NotesCli.Console.Core;

namespace NotesCli.Console.Infrastructure;

class NotesFileReader : IDayNotesReader
{
    public string FilePath { get; init; }

    public NotesFileReader(string filePath) => FilePath = filePath;

    public string[] Read() => File.ReadAllLines(FilePath);
}
