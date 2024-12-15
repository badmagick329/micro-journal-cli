namespace NotesCli.Console.Infrastructure.Config;

using System.Diagnostics;
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

        var aliases = Config.GetSection("Notes:Aliases").Get<Dictionary<string, string[]>>();
        Debug.Assert(aliases is not null);

        var categoryReader = new CategoryReader(aliases);

        return new AppConfig(NotesRoot: notesRoot, Categories: categoryReader.Read());
    }
}
