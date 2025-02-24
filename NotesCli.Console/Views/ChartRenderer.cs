using System.Text;
using Spectre.Console;

static class ChartRenderer
{
    public static void MinutesChart(IOrderedEnumerable<KeyValuePair<string, int>> breakdown)
    {
        var breakdownChart = new BreakdownChart().Width(80);
        List<Color> colors =
        [
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
            Color.White,
            Color.DarkRed,
            Color.DarkGreen,
            Color.DarkBlue,
            Color.DarkMagenta,
            Color.DarkCyan,
            Color.Red3,
            Color.Green3,
            Color.Yellow3,
        ];
        int colorIndex = 0;
        int maxItems = colors.Count;
        foreach (var (category, minutes) in breakdown)
        {
            breakdownChart.AddItem(category, minutes, colors[colorIndex++]);
            if (colorIndex >= maxItems)
            {
                break;
            }
        }
        AnsiConsole.Write(breakdownChart);
    }

    public static void ScoreChart(
        int scoredMinutes,
        int totalMinutes,
        int totalScore,
        double averageDifficulty
    )
    {
        if (scoredMinutes == 0)
        {
            return;
        }

        var percentage = scoredMinutes / (float)totalMinutes * 100;
        var scoreAdjustedPercentage = totalScore / (float)totalMinutes * 100;

        var breakdownChart = new BreakdownChart().Width(80);
        breakdownChart.AddItem("Scored", scoredMinutes, ScoreColor(percentage));
        breakdownChart.AddItem("Unscored", totalMinutes - scoredMinutes, Color.Grey);
        AnsiConsole.MarkupLine("[underline]Scored Minutes[/]");
        AnsiConsole.Write(breakdownChart);

        AnsiConsole.MarkupLine($"[underline]Adjusted for difficulty ({averageDifficulty:F1})[/]");
        var difficultyAdjustedBreakdownChart = new BreakdownChart().Width(80);
        difficultyAdjustedBreakdownChart.AddItem(
            "Adjusted Scored",
            totalScore,
            ScoreColor(scoreAdjustedPercentage)
        );
        difficultyAdjustedBreakdownChart.AddItem(
            "Unscored",
            Math.Max(totalMinutes - totalScore, 0),
            Color.Grey
        );
        AnsiConsole.Write(difficultyAdjustedBreakdownChart);
    }

    private static Color ScoreColor(float percentage) =>
        percentage switch
        {
            < 10 => Color.Red,
            < 20 => Color.Fuchsia,
            < 30 => Color.Yellow,
            < 40 => Color.Lime,
            _ => Color.Teal,
        };

    public static void PrintScoreLegend()
    {
        var scoreString = new StringBuilder();
        scoreString.Append("[red]0-10%[/] ");
        scoreString.Append("[fuchsia]10-20%[/] ");
        scoreString.Append("[yellow]20-30%[/] ");
        scoreString.Append("[lime]30-40%[/] ");
        scoreString.Append("[teal]40%+[/]");
        AnsiConsole.MarkupLine(
            $"\n[yellow]Score color based on percentage of total time spent:[/]\n{scoreString}"
        );
    }
}
