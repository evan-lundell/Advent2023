namespace Advent2023.Days;

public class Day6 : IDay
{
    public void Solve(string[] input)
    {
        var part1 = GetResult(Race.CreateRaces(input));
        Console.WriteLine($"Part1: {part1}");
        var part2 = GetResult(Race.CreateRaces(input, true));
        Console.WriteLine($"Part2: {part2} ");
    }

    private long GetResult(Race[] races)
    {
        long result = 1;
        foreach (var race in races)
        {
            HashSet<long> checkedValues = new();
            var previous = race.Time;
            var current = race.Time % 2 == 0 ? race.Time / 2 : race.Time / 2 + 1;
            long lastSuccess = 0;
            while (!checkedValues.Contains(current) && current > 0)
            {
                checkedValues.Add(current);
                var interval = Math.Abs(previous - current);
                previous = current;
                if (current * (race.Time - current) > race.Distance)
                {
                    lastSuccess = current;
                    current += interval % 2 == 0 ? interval / 2 : interval / 2 + 1;
                }
                else
                {
                    current -= interval % 2 == 0 ? interval / 2 : interval / 2 + 1;
                }
            }

            var numOfWins = (lastSuccess - (race.Time / 2)) * 2;
            if (race.Time % 2 == 0)
            {
                numOfWins++;
            }

            result *= numOfWins;
        }

        return result;
    }
}

readonly struct Race(long time, long distance)
{
    public long Time => time;
    public long Distance => distance;
    public static Race[] CreateRaces(string[] input, bool fixSpaces = false)
    {
        long[] times;
        long[] distances;
        if (!fixSpaces)
        {
            times = input[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1..].Select(long.Parse).ToArray();
            distances = input[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1..].Select(long.Parse).ToArray();
        }
        else
        {
            times = new[]
                { long.Parse(string.Join("", input[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1..])) };
            distances = new[]
                { long.Parse(string.Join("", input[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1..])) };
        }

        return times.Select((t, i) => new Race(t, distances[i])).ToArray();
    }
}