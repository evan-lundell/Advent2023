namespace Advent2023.Days;

public class Day4 : IDay
{
    public void Solve(string[] input)
    {
        Part1(input);
        Part2(input);
    }

    private void Part1(string[] input)
    {
        var totalScore = input.Select(Card.ParseCard).Select(c => c.GetScore()).Sum();
        Console.WriteLine($"Part1: {totalScore}");
    }

    private void Part2(string[] input)
    {
        var cards = input.Select(Card.ParseCard).ToArray();
        var cardCounts = Enumerable.Repeat(1, input.Length).ToArray();
        for (int cardIndex = 0; cardIndex < cards.Length; cardIndex++)
        {
            var score = cards[cardIndex].GetMatches();
            for (int scoreIndex = 1 + cardIndex; scoreIndex <= cardIndex + score; scoreIndex++)
            {
                cardCounts[scoreIndex] += cardCounts[cardIndex];
            }
        }

        Console.WriteLine($"Part2: {cardCounts.Sum()}");
    }
}

class Card (HashSet<int> winningNumbers, List<int> cardNumbers)
{
    public int GetScore() => (int)Math.Pow(2, GetMatches() - 1);

    public int GetMatches() => cardNumbers.Count(winningNumbers.Contains);
    
    public static Card ParseCard(string line)
    {
        var gameNumbers = line
            .Split(":")[1];
        var gameNumbersSplit = gameNumbers.Split("|", StringSplitOptions.TrimEntries);
        var winningNumbers = gameNumbersSplit[0]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToHashSet();
        var cardNumbers = gameNumbersSplit[1]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
        return new Card(winningNumbers, cardNumbers);
    }
}
