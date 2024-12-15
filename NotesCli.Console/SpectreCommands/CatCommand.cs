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
        var inputDateString = settings.Date;
        if (inputDateString is null)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] No date specified");
            return 1;
        }

        var date = DateStringToDate(inputDateString);
        var fileName = DateToFileName(date);
        var filePath = Path.Join(Config.NotesRoot, fileName);
        Debug.Assert(filePath is not null);
        if (!File.Exists(filePath))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] File not found: {filePath}");
            return 1;
        }

        var catDayNotes = new CatDayNotes(new NotesFileReader(filePath), Config.Categories);

        var dateNote = catDayNotes.FindDayNote(date);
        if (dateNote is null)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] No note found for date: {date}");
            return 0;
        }
        AnsiConsole.MarkupLineInterpolated($"[yellow]Showing note for {date}[/]");
        var view = new CatDayNoteView(dateNote);
        view.Show();
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
