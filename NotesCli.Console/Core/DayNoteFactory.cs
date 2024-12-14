using System.Diagnostics;

namespace NotesCli.Console.Core;

static class DayNoteFactory
{
    public static List<DayNote> CreateDayNotes(IDayNotesReader dayNotesReader)
    {
        var lines = dayNotesReader.Read();
        var dayNotes = new List<DayNote>();
        var currentDayLines = new List<string>();

        foreach (var line in lines)
        {
            if (!line.StartsWith('#'))
            {
                currentDayLines.Add(line);
                continue;
            }

            if (currentDayLines.Count > 0)
            {
                Debug.Assert(currentDayLines[0].StartsWith('#'));
                dayNotes.Add(new DayNote(currentDayLines));
                currentDayLines = [];
            }
            currentDayLines.Add(line);
        }

        if (currentDayLines.Count > 0)
        {
            dayNotes.Add(new DayNote(currentDayLines));
        }

        return dayNotes;
    }
}
