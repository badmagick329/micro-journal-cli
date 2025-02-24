namespace NotesCli.Console.Core;

class TimeSpanEntry
{
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public int Duration => (int)(EndTime - StartTime).TotalMinutes;
    public string EntryText { get; init; }
    public string Category { get; init; }
    public int? Difficulty { get; init; }

    // TODO: Refactor score calculator out?
    public float? Score => Difficulty is null ? null : ((Difficulty / (float)20) + 1) * Duration;

    public TimeSpanEntry(
        int startHours,
        int startMinutes,
        int endHours,
        int endMinutes,
        string entryText,
        string category,
        int? difficulty
    )
    {
        StartTime = new TimeOnly(startHours, startMinutes);
        EndTime = new TimeOnly(endHours, endMinutes);
        EntryText = entryText;
        Category = category;
        Difficulty = difficulty;
    }

    public bool IsSleepEntry() =>
        Category.Equals("sleep", StringComparison.OrdinalIgnoreCase)
        || Category.Equals("sleep disruption", StringComparison.OrdinalIgnoreCase);

    public override string ToString()
    {
        return $"{StartTime}-{EndTime} - [{Category}] - {EntryText}";
    }
}
