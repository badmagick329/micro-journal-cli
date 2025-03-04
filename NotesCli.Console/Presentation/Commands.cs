using System.Diagnostics;
using NotesCli.Console.Application;
using NotesCli.Console.Infrastructure;
using NotesCli.Console.Infrastructure.Config;
using NotesCli.Console.Views;

namespace NotesCli.Console.Presentation;

static class Commands
{
    /// <summary>
    /// Cat a date or a range of dates
    /// </summary>
    /// <param name="dates">-d, Date or range of dates separated by -</param>
    public static void Cat(string dates)
    {
        if (dates.Contains('-'))
        {
            CatMultipleDates(dates);
        }
        else
        {
            CatSingleDate(dates);
        }
    }

    private static int CatSingleDate(string inputDate)
    {
        // TODO: Refactor
        var config = new AppConfigReader().CreateAppConfig();
        var consoleOut = new ConsoleOut();

        var date = DateStringToDate(inputDate);
        var fileName = DateToFileName(date);
        var filePath = Path.Join(config.NotesRoot, fileName);
        Debug.Assert(filePath is not null);
        if (!File.Exists(filePath))
        {
            consoleOut.WriteLineError($"File not found: {filePath}");
            return 1;
        }

        var manager = new DayNotesManager(new NotesFileReader(filePath), config.Categories);
        var dateNote = manager.FindDayNote(date);
        if (dateNote is null)
        {
            consoleOut.WriteLineError($"No note found for date: {date}");
            return 0;
        }

        consoleOut.WriteLineWarn($"Note for {date}");
        var view = new CatDayNoteView(dateNote);
        view.Show();
        consoleOut.WriteLineWarn($"Breakdown for {date} [minutes/day]");
        view.ShowBreakdown();

        return 0;
    }

    private static int CatMultipleDates(string dateRange)
    {
        // TODO: Refactor
        var config = new AppConfigReader().CreateAppConfig();
        var consoleOut = new ConsoleOut();

        var rangeParts = dateRange.Split('-').Select(dr => dr.Trim()).ToList();
        (var rangeStart, var rangeEnd) = rangeParts switch
        {
            { Count: 2 } => (rangeParts.First(), rangeParts.Last()),
            _ => throw new ArgumentException("Invalid date range format. Expected: start-end"),
        };
        var startDate = DateStringToDate(rangeStart);
        var endDate = DateStringToDate(rangeEnd);
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before end date");
        }
        List<DateOnly> dates = [];
        for (int i = 0; ; i++)
        {
            var nextDate = startDate.AddDays(i);
            if (nextDate > endDate)
            {
                break;
            }
            dates.Add(nextDate);
        }

        List<string> filePaths = [];
        foreach (var date in dates)
        {
            var fileName = DateToFileName(date);
            var filePath = Path.Join(config.NotesRoot, fileName);
            Debug.Assert(filePath is not null);
            if (!File.Exists(filePath))
            {
                consoleOut.WriteLineError($"File not found: {filePath}");
                return 1;
            }
            if (!filePaths.Contains(filePath))
            {
                filePaths.Add(filePath);
            }
        }
        var manager = new DayNotesManager(new NotesFileReader(filePaths), config.Categories);
        var dayNotes = manager.FindDayNotes(startDate, endDate);
        var view = new CatDayNotesView(dayNotes);
        consoleOut.WriteLineWarn($"Notes for {startDate} to {endDate} [minutes/day]");
        view.ShowBreakdown();

        return 0;
    }

    private static DateOnly DateStringToDate(string inputDateString)
    {
        try
        {
            return new DateOnly(
                2000 + int.Parse(inputDateString[..2]),
                int.Parse(inputDateString[2..4]),
                int.Parse(inputDateString[4..])
            );
        }
        catch (Exception e)
        {
            throw new ArgumentException(
                $"Invalid date argument: {inputDateString}. Expected format: yymmdd",
                e
            );
        }
    }

    private static string DateToFileName(DateOnly date) =>
        $"{date.Year.ToString()[2..]}{date.Month:D2}.md";
}
