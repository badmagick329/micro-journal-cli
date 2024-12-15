namespace NotesCli.Console.Core;

interface IDayNotesReader
{
    public IEnumerable<string> Read();
}
