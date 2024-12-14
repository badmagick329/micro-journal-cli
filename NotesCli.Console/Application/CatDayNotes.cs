using NotesCli.Console.Core;

namespace NotesCli.Console.Application;

class CatDayNotes
{
    public List<DayNote> DayNotes { get; init; }
    public int Count => DayNotes.Count;

    public CatDayNotes(IDayNotesReader notesReader)
    {
        DayNotes = DayNoteFactory.CreateDayNotes(notesReader);
    }
}
