using ConsoleAppFramework;

namespace NotesCli.Console.Presentation;

class App
{
    public static void Run(string[] args)
    {
        var consoleApp = ConsoleApp.Create();
        consoleApp.Add("cat", Commands.Cat);
        consoleApp.Run(args);
    }
}
