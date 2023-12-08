using System.Text.RegularExpressions;

namespace Advent2023.Days;

public class Day8 : IDay
{
    public void Solve(string[] input)
    {
        var start = DateTime.Now;
        var sequence = input[0];
        var dict = input[2..]
            .Select(s => Regex.Matches(s, "[0-9A-Z]{3}"))
            .ToDictionary(m => m[0].Value, m => (Left: m[1].Value, Right: m[2].Value));
        Part1(sequence, dict);
        Part2(sequence, dict);
        var end = DateTime.Now;
        Console.WriteLine($"Total time: {(end - start).TotalMilliseconds}");
    }

    private void Part1(string sequence, Dictionary<string, (string Left, string Right)> dict)
    {
        var start = DateTime.Now;
        int count = 0;
        string current = "AAA";
        while (current != "ZZZ")
        {
            var direction = sequence[count % sequence.Length];
            current = direction == 'L' ? dict[current].Left : dict[current].Right;
            count++;
        }

        var end = DateTime.Now;
        Console.WriteLine($"Part1: {count}");
        Console.WriteLine($"Part1 time: {(end - start).TotalMilliseconds}");
    }

    private void Part2(string sequence, Dictionary<string, (string Left, string Right)> dict)
    {
        var startTime = DateTime.Now;
        var starts = dict.Keys.Where(k => k[^1] == 'A').ToArray();
        long lcm = 1;
        foreach (var start in starts)
        {
            int count = 0;
            string current = start;
            while (current[^1] != 'Z')
            {
                var direction = sequence[count % sequence.Length];
                current = direction == 'L' ? dict[current].Left : dict[current].Right;
                count++;
            }

            lcm = lcm * count / GreatestCommonDivisor(lcm, count);
        }
        
        var end = DateTime.Now;
        Console.WriteLine($"Part1: {lcm}");
        Console.WriteLine($"Part1 time: {(end - startTime).TotalMilliseconds}");
    }

    private long GreatestCommonDivisor(long a, long b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
            {
                a %= b;
            }
            else
            {
                b %= a;
            }
        }

        return a | b;
    }
}
