namespace NotesCli.Console.Core;

class TimeSpanEntry
{
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public string EntryText { get; init; }

    public TimeSpanEntry(
        int startHours,
        int startMinutes,
        int endHours,
        int endMinutes,
        string entryText
    )
    {
        StartTime = new TimeOnly(startHours, startMinutes);
        EndTime = new TimeOnly(endHours, endMinutes);
        EntryText = entryText;
    }
}
