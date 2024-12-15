using NotesCli.Console.Core;
using Spectre.Console;

namespace NotesCli.Console.Views;

class CatDayNoteView
{
    DayNote DayNote { get; init; }

    public CatDayNoteView(DayNote dayNote) => DayNote = dayNote;

    public void Show()
    {
        var table = new Table();
        table.AddColumns(
            new TableColumn("Timespan"),
            new TableColumn("Duration"),
            new TableColumn("Category")
        );
        foreach (var entry in DayNote.TimeSpanEntries)
        {
            table.AddRow(
                $"[yellow]{entry.StartTime}-{entry.EndTime}[/]",
                $"[blue]{entry.Duration}m[/]",
                $"[teal]{entry.Category}[/]"
            );
        }
        AnsiConsole.Write(table);
    }

    public void ShowAll()
    {
        var table = new Table();
        table.AddColumns(
            new TableColumn("Timespan"),
            new TableColumn("Duration"),
            new TableColumn("Category"),
            new TableColumn("Description")
        );
        foreach (var entry in DayNote.TimeSpanEntries)
        {
            table.AddRow(
                $"[yellow]{entry.StartTime}-{entry.EndTime}[/]",
                $"[blue]{entry.Duration}m[/]",
                $"[teal]{entry.Category}[/]",
                $"[lime]{entry.EntryText}[/]"
            );
        }
        AnsiConsole.Write(table);
    }

    public void ShowBreakdown()
    {
        var breakdown = DayNote.CategoryMinutesBreakdown().OrderByDescending(kvp => kvp.Value);
        var breakdownChart = new BreakdownChart().Width(80);
        var enumerator = NextBreakdownColor().GetEnumerator();
        foreach (var (category, minutes) in breakdown)
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
