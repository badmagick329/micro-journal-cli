using NotesCli.Console.Core;

namespace NotesCli.Console.Infrastructure;

class NotesFileReader : IDayNotesReader
{
    public IEnumerable<string> FilePaths { get; init; }

    public NotesFileReader(string filePath) => FilePaths = [filePath];

    public NotesFileReader(IEnumerable<string> filePaths) => FilePaths = filePaths;

    public IEnumerable<string> Read() =>
        FilePaths.SelectMany(fp => File.ReadAllLines(fp)).ToArray();
}
