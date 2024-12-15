namespace NotesCli.Console.Core;

class TimeSpanEntry
{
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public int Duration => (int)(EndTime - StartTime).TotalMinutes;
    public string EntryText { get; init; }
    public string Category { get; init; }

    public TimeSpanEntry(
        int startHours,
        int startMinutes,
        int endHours,
        int endMinutes,
        string entryText,
        string category
    )
    {
        StartTime = new TimeOnly(startHours, startMinutes);
        EndTime = new TimeOnly(endHours, endMinutes);
        EntryText = entryText;
        Category = category;
    }

    public override string ToString()
    {
        return $"{StartTime}-{EndTime} - [{Category}] - {EntryText}";
    }
}
