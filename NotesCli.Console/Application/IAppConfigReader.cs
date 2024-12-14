namespace NotesCli.Console.Application;

using NotesCli.Console.Infrastructure.Config;

interface IAppConfigReader
{
    public AppConfig CreateAppConfig();
}
