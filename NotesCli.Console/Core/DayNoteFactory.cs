using System.Diagnostics;
using NotesCli.Console.Common;
using Spectre.Console;

namespace NotesCli.Console.Core;

static class DayNoteFactory
{
    public static List<DayNote> CreateDayNotes(IDayNotesReader dayNotesReader)
    {
        var lines = dayNotesReader.Read();
        var dayNotes = new List<DayNote>();
        var currentDayLines = new List<string>();
        int year = dayNotesReader.ReadYear();

        foreach (var line in lines)
        {
            if (!TextLineInterpreter.LineIsDate(line))
            {
                currentDayLines.Add(line);
                continue;
            }

            if (currentDayLines.Count > 0)
            {
                Debug.Assert(TextLineInterpreter.LineIsDate(currentDayLines[0]));
                dayNotes.Add(CreateDayNote(currentDayLines, year));
                currentDayLines = [];
            }
            currentDayLines.Add(line);
        }

        if (currentDayLines.Count > 0)
        {
            dayNotes.Add(CreateDayNote(currentDayLines, year));
        }

        return dayNotes;
    }

    public static DayNote CreateDayNote(IEnumerable<string> lines, int year)
    {
        Debug.Assert(lines.Any());

        var monthString = lines.First().Split()[1][..2];
        var dayString = lines.First().Split()[1][2..];
        var date = new DateOnly(year, int.Parse(monthString), int.Parse(dayString));

        string startText = ReadStartText(lines);
        string endText = ReadEndText(lines);
        var timeSpans = ReadTimeSpanLines(lines)
            .Select(text => TimeSpanEntryFactory.CreateTimeSpan(text))
            .ToList();
        return new DayNote(date, startText, endText, timeSpans);
    }

    private static string ReadStartText(IEnumerable<string> lines)
    {
        List<string> startLines = [];

        foreach (var line in lines)
        {
            if (TextLineInterpreter.LineIsDate(line) || string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (TextLineInterpreter.TryTimeSpanMatch(line).Success)
            {
                break;
            }
            startLines.Add(line);
        }

        return string.Join(" ", startLines);
    }

    private static string ReadEndText(IEnumerable<string> lines)
    {
        List<string> endLines = [];
        bool timeSpanDetected = false;

        foreach (var line in lines)
        {
            if (TextLineInterpreter.TryTimeSpanMatch(line).Success)
            {
                timeSpanDetected = true;
            }
            else if (timeSpanDetected && !string.IsNullOrWhiteSpace(line))
            {
                endLines.Add(line);
            }
        }

        return string.Join(" ", endLines);
    }

    private static List<string> ReadTimeSpanLines(IEnumerable<string> lines)
    {
        List<string> timeSpanLines = [];
        List<string> currentTimeSpanLines = [];

        bool TimeSpansHaveNotStartedBeingRead;
        bool TimeSpansHaveBeenRead;
        bool TimeSpanIsBeingRead;
        bool NewTimeSpanDetected;
        foreach (var line in lines)
        {
            var match = TextLineInterpreter.TryTimeSpanMatch(line);
            TimeSpansHaveNotStartedBeingRead = !match.Success && timeSpanLines.Count == 0;
            if (TimeSpansHaveNotStartedBeingRead)
            {
                continue;
            }

            TimeSpansHaveBeenRead =
                !match.Success && timeSpanLines.Count > 0 && string.IsNullOrWhiteSpace(line);
            if (TimeSpansHaveBeenRead)
            {
                Debug.Assert(
                    currentTimeSpanLines.Count > 0,
                    "Should've had leftover lines from the last timespan entry"
                );
                timeSpanLines.Add(string.Join(" ", currentTimeSpanLines));
                currentTimeSpanLines.Clear();
                break;
            }

            NewTimeSpanDetected = match.Success;
            if (NewTimeSpanDetected)
            {
                if (currentTimeSpanLines.Count > 0)
                {
                    timeSpanLines.Add(string.Join(" ", currentTimeSpanLines));
                    currentTimeSpanLines.Clear();
                }
                currentTimeSpanLines.Add(line);
            }

            TimeSpanIsBeingRead =
                !match.Success
                && currentTimeSpanLines.Count > 0
                && !string.IsNullOrWhiteSpace(line);
            if (TimeSpanIsBeingRead)
            {
                currentTimeSpanLines.Add(line);
            }
        }
        if (currentTimeSpanLines.Count > 0)
        {
            timeSpanLines.Add(string.Join(" ", currentTimeSpanLines));
            currentTimeSpanLines.Clear();
        }

        return timeSpanLines;
    }
}
