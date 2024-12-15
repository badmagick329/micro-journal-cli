namespace NotesCli.Console.Core;

interface IDayNotesReader
{
    public string[] Read();
    public int ReadYear();
}
