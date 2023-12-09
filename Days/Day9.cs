namespace Advent2023.Days;

public class Day9 : IDay
{
    public void Solve(string[] input)
    {
        // Part1(input);
        Part2(input);
    }
    
    private void Part1 (string[] input)
    {
        long sum = 0;
        foreach (var line in input)
        {
            var expandedLines = ExpandLine(line);
            for (int i = expandedLines.Count - 1; i > 0; i--)
            {
                expandedLines[i - 1].Add(expandedLines[i - 1][^1] + expandedLines[i][^1]);
            }
            sum += expandedLines[0][^1];

        }

        Console.WriteLine($"Part1: {sum}");
    }

    private void Part2(string[] input)
    {
        long sum = 0;
        foreach (var line in input)
        {
            var expandedLines = ExpandLine(line);
            for (int i = expandedLines.Count - 1; i > 0; i--)
            {
                expandedLines[i - 1].Insert(0, expandedLines[i - 1][0] - expandedLines[i][0]);
            }
            sum += expandedLines[0][0];
        }

        Console.WriteLine($"Part2: {sum}");
    }

    private List<List<long>> ExpandLine(string line)
    {
        var expandedLines = new List<List<long>>();
        var nums = line.Split(" ").Select(long.Parse).ToList();
        expandedLines.Add(nums);
        while (nums.Any(n => n != 0))
        {
            var nextLine = new List<long>();
            for (int i = 0; i < nums.Count - 1; i++)
            {
                nextLine.Add(nums[i + 1] - nums[i]);
            }

            expandedLines.Add(nextLine);
            nums = nextLine;
        }

        return expandedLines;
    }
}