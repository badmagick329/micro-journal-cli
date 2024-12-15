using System.Text.RegularExpressions;
using NotesCli.Console.Core;

namespace NotesCli.Console.Infrastructure;

class NotesReader : IDayNotesReader
{
    public string FilePath { get; init; }

    public NotesReader(string filePath) => FilePath = filePath;

    public string[] Read() => File.ReadAllLines(FilePath);

    public int ReadYear()
    {
        var sep = Path.DirectorySeparatorChar == '\\' ? "\\\\" : "\\/";
        Match match = Regex.Match(FilePath, @$".+{sep}(\d{4})(:?\.md)?");
        if (!match.Success)
        {
            throw new ArgumentException("Invalid file path.");
        }

        return int.Parse(match.Groups[1].Value);
    }
}
