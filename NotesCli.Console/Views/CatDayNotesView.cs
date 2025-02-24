using NotesCli.Console.Core;
using Spectre.Console;

namespace NotesCli.Console.Views;

class CatDayNotesView
{
    IEnumerable<DayNote> DayNotes { get; init; }

    public CatDayNotesView(IEnumerable<DayNote> dayNotes) => DayNotes = dayNotes;

    public void ShowBreakdown()
    {
        var breakdowns = DayNotes.Select(dn => dn.CategoryMinutesBreakdown()).ToList();
        foreach (var breakdown in breakdowns)
        {
            foreach (var (category, minutes) in breakdown)
            {
                breakdown[category] /= breakdowns.Count;
            }
        }
        var combinedBreakdown = DayNote
            .CombineMinutesBreakdowns(breakdowns)
            .OrderByDescending(kvp => kvp.Value);
        ChartRenderer.MinutesChart(combinedBreakdown);

        var scoredMinutes = DayNotes.Sum(dn => dn.ScoredMinutes);
        var totalMinutes = DayNotes.Sum(dn => dn.TotalMinutes);
        var totalScore = DayNotes.Sum(dn => dn.TotalScore);
        var averageDifficulty =
            DayNotes
                .SelectMany(dn => dn.TimeSpanEntries)
                .Where(t => t.Difficulty is not null)
                .Average(t => t.Difficulty) ?? 0;
        ChartRenderer.PrintScoreLegend();
        ChartRenderer.ScoreChart(scoredMinutes, totalMinutes, totalScore, averageDifficulty);
    }
}
