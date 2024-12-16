namespace NotesCli.Console.SpectreCommands;

using System.ComponentModel;
using System.Diagnostics;
using NotesCli.Console.Application;
using NotesCli.Console.Infrastructure;
using NotesCli.Console.Infrastructure.Config;
using NotesCli.Console.Views;
using Spectre.Console;
using Spectre.Console.Cli;

class CatCommand : Command<CatCommand.Settings>
{
    public AppConfig Config { get; init; }

    public sealed class Settings : CommandSettings
    {
        [Description("Date to cat (yymmdd)")]
        [CommandArgument(0, "<date>")]
        public string? Date { get; init; }
    }

    public CatCommand()
    {
        Config = new AppConfigReader().CreateAppConfig();
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var inputDate = settings.Date;
        if (inputDate is null)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] No date specified");
            return 1;
        }

        if (inputDate.Contains('-'))
        {
            return CatMultipleDates(inputDate);
        }

        return CatSingleDate(inputDate);
    }

    private int CatSingleDate(string inputDate)
    {
        var date = DateStringToDate(inputDate);
        var fileName = DateToFileName(date);
        var filePath = Path.Join(Config.NotesRoot, fileName);
        Debug.Assert(filePath is not null);
        if (!File.Exists(filePath))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] File not found: {filePath}");
            return 1;
        }

        var manager = new DayNotesManager(new NotesFileReader(filePath), Config.Categories);
        var dateNote = manager.FindDayNote(date);
        if (dateNote is null)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] No note found for date: {date}");
            return 0;
        }

        AnsiConsole.MarkupLineInterpolated($"[yellow]Showing note for {date}[/]");
        var view = new CatDayNoteView(dateNote);
        view.Show();
        AnsiConsole.MarkupLineInterpolated(
            $"[yellow]Showing breakdown for {date}[/] [[minutes/day]]"
        );
        view.ShowBreakdown();

        return 0;
    }

    private int CatMultipleDates(string dateRange)
    {
        var rangeParts = dateRange.Split('-').Select(dr => dr.Trim()).ToList();
        (var rangeStart, var rangeEnd) = rangeParts switch
        {
            { Count: 2 } => (rangeParts.First(), rangeParts.Last()),
            _ => throw new ArgumentException("Invalid date range format. Expected: start-end"),
        };
        var startDate = DateStringToDate(rangeStart);
        var endDate = DateStringToDate(rangeEnd);
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before end date");
        }
        List<DateOnly> dates = [];
        for (int i = 0; ; i++)
        {
            var nextDate = startDate.AddDays(i);
            if (nextDate > endDate)
            {
                break;
            }
            dates.Add(nextDate);
        }

        List<string> filePaths = [];
        foreach (var date in dates)
        {
            var fileName = DateToFileName(date);
            var filePath = Path.Join(Config.NotesRoot, fileName);
            Debug.Assert(filePath is not null);
            if (!File.Exists(filePath))
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] File not found: {filePath}");
                return 1;
            }
            if (!filePaths.Contains(filePath))
            {
                filePaths.Add(filePath);
            }
        }
        var manager = new DayNotesManager(new NotesFileReader(filePaths), Config.Categories);
        var dayNotes = manager.FindDayNotes(startDate, endDate);
        var view = new CatDayNotesView(dayNotes);
        AnsiConsole.MarkupLine(
            $"[yellow]Showing breakdown for {startDate} to {endDate}[/] [[average minutes/day]]"
        );
        view.ShowBreakdown();

        return 0;
    }

    private static DateOnly DateStringToDate(string inputDateString)
    {
        try
        {
            return new DateOnly(
                2000 + int.Parse(inputDateString[..2]),
                int.Parse(inputDateString[2..4]),
                int.Parse(inputDateString[4..])
            );
        }
        catch (Exception e)
        {
            throw new ArgumentException(
                $"Invalid date argument: {inputDateString}. Expected format: yymmdd",
                e
            );
        }
    }

    private static string DateToFileName(DateOnly date) =>
        $"{date.Year.ToString()[2..]}{date.Month:D2}.md";
}
