namespace NotesCli.Console.Core;

class DayNote
{
    public List<string> DayLines { get; init; }

    public DayNote(List<string> lines)
    {
        DayLines = lines;
    }
}
