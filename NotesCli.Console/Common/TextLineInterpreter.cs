using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NotesCli.Console.Common;

static class TextLineInterpreter
{
    public static bool LineIsDate(string line) => line.StartsWith('#');

    public static List<string> DoReadWhile(
        List<string> lines,
        Func<string, bool> predicate,
        int startIndex = 0
    )
    {
        if (lines.Count == 0)
        {
            return [];
        }
        Debug.Assert(startIndex < lines.Count);

        List<string> result = [];
        int i = startIndex;
        do
        {
            result.Add(lines[i]);
            i++;
        } while (predicate(lines[i]));

        return result;
    }

    public static Match TryTimeSpanMatch(string text) =>
        Regex.Match(text, @"-\s(\d{2}):(\d{2})-(\d{2}):(\d{2})\s-(.*)");

    public static (
        int startHours,
        int startMinutes,
        int endHours,
        int endMinutes,
        string text
    ) ReadHoursMinutesAndTextFromMatch(Match match)
    {
        var startHours = int.Parse(match.Groups[1].Value);
        var startMinutes = int.Parse(match.Groups[2].Value);
        var endHours = int.Parse(match.Groups[3].Value);
        var endMinutes = int.Parse(match.Groups[4].Value);
        var text = match.Groups[5].Value.Trim();

        return (startHours, startMinutes, endHours, endMinutes, text);
    }

    // public static (TimeOnly startTime, TimeOnly endTime)?
}
