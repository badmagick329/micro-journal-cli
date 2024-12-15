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
        [Description("File to cat")]
        [CommandArgument(0, "<fileName>")]
        public string? FileName { get; init; }
    }

    public CatCommand()
    {
        Config = new AppConfigReader().CreateAppConfig();
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var fileName = settings.FileName;
        if (fileName is null)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] No file specified");
            return 1;
        }

        var filePath = Path.Join(Config.NotesRoot, fileName);
        System.Console.WriteLine($"Constructed file path: {filePath}");
        Debug.Assert(filePath is not null);
        if (!File.Exists(filePath))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] File not found: {filePath}");
            return 1;
        }
        AnsiConsole.MarkupLineInterpolated($"[green]Checking file[/]: [blue]{filePath}[/]");

        var catDayNotes = new CatDayNotes(new NotesFileReader(filePath), Config.Categories);
        AnsiConsole.MarkupLineInterpolated($"Number of notes read: [green]{catDayNotes.Count}[/]");
        new CatDayNoteView(catDayNotes.DayNotes.Last()).Show();

        return 0;
    }
}
