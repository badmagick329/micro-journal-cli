namespace NotesCli.Console.Infrastructure.Config;

using Microsoft.Extensions.Configuration;
using NotesCli.Console.Application;

class AppConfigReader : IAppConfigReader
{
    public IConfigurationRoot Config { get; init; }

    public AppConfigReader()
    {
        Config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public AppConfig CreateAppConfig()
    {
        var notesRoot = Config["Notes:Root"];
        ArgumentException.ThrowIfNullOrWhiteSpace(notesRoot);
        return new AppConfig(NotesRoot: notesRoot);
    }
}
