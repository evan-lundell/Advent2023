namespace Advent2023.Days;

public class Day2 : IDay
{
    public void Solve(string[] input)
    {
        Part1(input);
        Part2(input);
    }

    private void Part1(string[] gameStrings)
    {
        var limits = new Dictionary<string, int>
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };

        var sum = gameStrings
            .Where(gs => !gs.Split(":", StringSplitOptions.TrimEntries)[1]
                .Split(";")
                .Any(gameSplit => gameSplit.Split(",", StringSplitOptions.TrimEntries)
                    .Where(drawSplit =>
                    {
                        var ballSplit = drawSplit.Split(" ", StringSplitOptions.TrimEntries);
                        return int.Parse(ballSplit[0]) > limits[ballSplit[1]];
                    })
                    .Any()))
            .Select(gs => int.Parse(gs.Split(":", StringSplitOptions.TrimEntries)[0].Substring(5)))
            .Sum();

        Console.WriteLine($"Part1: {sum}");
    }

    private void Part2(string[] gameStrings)
    {
        int total = 0;
        foreach (var games in gameStrings.Select(gs =>
                     gs.Split(":", StringSplitOptions.TrimEntries)[1]))
        {
            Dictionary<string, int> maxColorCounts = new Dictionary<string, int>
            {
                { "red", 0 },
                { "blue", 0 },
                { "green", 0 }
            };

            foreach (var draw in games.Split(";", StringSplitOptions.TrimEntries))
            {
                foreach (var ball in draw.Split(",", StringSplitOptions.TrimEntries))
                {
                    var ballSplit = ball.Split(" ", StringSplitOptions.TrimEntries);
                    maxColorCounts[ballSplit[1]] = Math.Max(maxColorCounts[ballSplit[1]], int.Parse(ballSplit[0]));
                }
            }

            total += maxColorCounts["green"] * maxColorCounts["red"] * maxColorCounts["blue"];
        }

        Console.WriteLine($"Part2: {total}");
    }
}
