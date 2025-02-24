using System.Diagnostics;
using NotesCli.Console.Common;

namespace NotesCli.Console.Core;

static class TimeSpanEntryFactory
{
    public static TimeSpanEntry CreateTimeSpan(string text, List<Category> categories)
    {
        var match = TextLineInterpreter.TryTimeSpanMatch(text);
        Debug.Assert(match.Success, "Successful match expected. Invalid text passed to factory");

        var (startHours, startMinutes, endHours, endMinutes, entryText, difficulty) =
            TextLineInterpreter.ReadInputForTimeSpanEntry(match);
        var category = "Other";
        foreach (var c in categories)
        {
            if (c.IsAlias(entryText))
            {
                category = c.Name;
                break;
            }
        }

        return new TimeSpanEntry(
            startHours,
            startMinutes,
            endHours,
            endMinutes,
            entryText,
            category,
            difficulty
        );
    }
}
