namespace NotesCli.Console;

using NotesCli.Console.Infrastructure.Config;
using NotesCli.Console.SpectreCommands;
using Spectre.Console.Cli;

class Program
{
    public static void Main(string[] args)
    {
        var appConfig = new AppConfigReader().CreateAppConfig();
        System.Console.WriteLine($"Notes root: {appConfig.NotesRoot}");

        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddCommand<FileSizeCommand>("size");
            config.AddCommand<CatCommand>("cat");
        });
        app.Run(args);
    }
}
