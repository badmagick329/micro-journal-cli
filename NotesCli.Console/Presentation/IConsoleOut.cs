namespace NotesCli.Console.Presentation;

interface IConsoleOut
{
    public void WriteLineWarn(string message);
    public void WriteWarn(string message);
    public void WriteLineError(string message);
    public void WriteError(string message);
}
