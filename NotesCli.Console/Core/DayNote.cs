using System.Text;

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

    public Dictionary<string, int> CategoryMinutesBreakdown() =>
        TimeSpanEntries
            .Where(e => !e.IsSleepEntry())
            .GroupBy(e => e.Category)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Duration));

    public Dictionary<string, int> CategoryMinutesBreakdownWithSleep() =>
        TimeSpanEntries
            .GroupBy(e => e.Category)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Duration));

    public static Dictionary<string, int> CombineMinutesBreakdowns(
        List<Dictionary<string, int>> breakdowns
    )
    {
        Dictionary<string, int> result = [];
        foreach (var breakdown in breakdowns)
        {
            foreach (var (category, minutes) in breakdown)
            {
                if (result.ContainsKey(category))
                {
                    result[category] += minutes;
                }
                else
                {
                    result[category] = minutes;
                }
            }
        }
        return result;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(Date.ToString("yyyy/MM/dd"));
        sb.AppendLine(StartOfDayText);
        foreach (var timeSpan in TimeSpanEntries)
        {
            sb.AppendLine(timeSpan.ToString());
        }
        sb.AppendLine(Summary);
        return sb.ToString();
    }
}
