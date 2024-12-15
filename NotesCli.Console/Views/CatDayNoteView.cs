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
}
