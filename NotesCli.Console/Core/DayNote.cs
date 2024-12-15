namespace NotesCli.Console.Core;

class DayNote
{
    public DateOnly Date { get; private set; }
    public List<TimeSpanEntry> TimeSpanEntries { get; private set; } = [];
    public string StartOfDayText { get; init; }
    public string Summary { get; init; }

    public DayNote(DateOnly date, string startText, string endText, List<TimeSpanEntry> timeSpans)
    {
        Date = date;
        if (string.IsNullOrEmpty(endText))
        {
            Summary = startText ?? string.Empty;
        }
        else
        {
            StartOfDayText = startText ?? string.Empty;
            Summary = endText;
        }
        TimeSpanEntries = timeSpans;
    }
}
