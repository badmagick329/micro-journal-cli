using NotesCli.Console.Core;

namespace NotesCli.Console.Application;

class DayNotesManager
{
    public List<DayNote> DayNotes { get; init; }
    public int Count => DayNotes.Count;

    public DayNotesManager(IDayNotesReader notesReader, List<Category> categories)
    {
        DayNotes = DayNoteFactory.CreateDayNotes(notesReader, categories);
    }

    public DayNote? FindDayNote(DateOnly date) => DayNotes.Find(dn => dn.Date == date);

    public List<DayNote> FindDayNotes(DateOnly startDate, DateOnly endDate) =>
        DayNotes.FindAll(dn => dn.Date >= startDate && dn.Date <= endDate);
}
