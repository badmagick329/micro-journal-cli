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
            Color.Red3,
            Color.Green3,
            Color.Yellow3,
        ];
        int colorIndex = 0;
        int maxItems = colors.Count;
        foreach (var (category, minutes) in combinedBreakdown)
        {
            breakdownChart.AddItem(category, minutes, colors[colorIndex++]);
            if (colorIndex >= maxItems)
            {
                break;
            }
        }
        AnsiConsole.Write(breakdownChart);
    }
}
