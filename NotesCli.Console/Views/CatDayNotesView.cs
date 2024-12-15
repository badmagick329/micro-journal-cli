using NotesCli.Console.Core;
using Spectre.Console;

namespace NotesCli.Console.Views;

class CatDayNotesView
{
    IEnumerable<DayNote> DayNotes { get; init; }

    public CatDayNotesView(IEnumerable<DayNote> dayNotes) => DayNotes = dayNotes;

    public void ShowBreakdown()
    {
        var breakdowns = DayNotes.Select(dn => dn.CategoryMinutesBreakdown()).ToList();
        var combinedBreakdown = DayNote
            .CombineMinutesBreakdowns(breakdowns)
            .OrderByDescending(kvp => kvp.Value);

        var breakdownChart = new BreakdownChart().Width(80);
        var enumerator = NextBreakdownColor().GetEnumerator();
        foreach (var (category, minutes) in combinedBreakdown)
        {
            enumerator.MoveNext();
            breakdownChart.AddItem(category, minutes, enumerator.Current);
        }
        AnsiConsole.Write(breakdownChart);
    }

    private static IEnumerable<Color> NextBreakdownColor()
    {
        List<Color> colors =
        [
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
            Color.White,
            Color.DarkRed,
            Color.DarkGreen,
            Color.DarkBlue,
            Color.DarkMagenta,
            Color.DarkCyan,
        ];

        int i = 0;
        while (true)
        {
            yield return colors[i];
            i = (i + 1) % colors.Count;
        }
    }
}
