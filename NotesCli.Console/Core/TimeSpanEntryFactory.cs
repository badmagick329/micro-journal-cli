using System.Diagnostics;
using NotesCli.Console.Common;

namespace NotesCli.Console.Core;

static class TimeSpanEntryFactory
{
    public static TimeSpanEntry CreateTimeSpan(string text)
    {
        var match = TextLineInterpreter.TryTimeSpanMatch(text);
        Debug.Assert(match.Success, "Successful match expected. Invalid text passed to factory");

        var (startHours, startMinutes, endHours, endMinutes, entryText) =
            TextLineInterpreter.ReadHoursMinutesAndTextFromMatch(match);
        return new TimeSpanEntry(startHours, startMinutes, endHours, endMinutes, entryText);
    }
}
