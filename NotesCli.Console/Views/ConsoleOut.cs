using NotesCli.Console.Presentation;
using Spectre.Console;

namespace NotesCli.Console.Views;

class ConsoleOut : IConsoleOut
{
    public void WriteLineError(string message) =>
        AnsiConsole.MarkupLineInterpolated($"[red]{message}[/]");

    public void WriteError(string message) => AnsiConsole.MarkupInterpolated($"[red]{message}[/]");

    public void WriteLineWarn(string message) =>
        AnsiConsole.MarkupLineInterpolated($"[yellow]{message}[/]");

    public void WriteWarn(string message) =>
        AnsiConsole.MarkupInterpolated($"[yellow]{message}[/]");
}
