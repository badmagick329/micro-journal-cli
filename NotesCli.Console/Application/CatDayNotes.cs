using NotesCli.Console.Core;

namespace NotesCli.Console.Application;

class CatDayNotes
{
    public List<DayNote> DayNotes { get; init; }
    public int Count => DayNotes.Count;

    public CatDayNotes(IDayNotesReader notesReader, List<Category> categories)
    {
        DayNotes = DayNoteFactory.CreateDayNotes(notesReader, categories);
    }

    public DayNote? FindDayNote(DateOnly date) => DayNotes.Find(dn => dn.Date == date);
}
