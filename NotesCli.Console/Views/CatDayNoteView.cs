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
        foreach (var (category, minutes) in breakdown)
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
