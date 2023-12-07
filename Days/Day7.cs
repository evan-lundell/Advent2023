namespace Advent2023.Days;

public class Day7 : IDay
{
    public void Solve(string[] input)
    {
        var start = DateTime.Now;
        Part1(input);
        Part2(input);
        var end = DateTime.Now;
        Console.WriteLine($"Total runtime: {(end - start).TotalMilliseconds}");
    }

    private void Part1(string[] input)
    {
        var start = DateTime.Now;
        var result = input
            .Select(s => s.Split(" "))
            .Select(s => (Hand: new NormalHand(s[0]), Bid: long.Parse(s[1])))
            .OrderBy(h => h.Hand)
            .Select((h, i) => h.Bid * (i + 1))
            .Sum();

        var end = DateTime.Now;
        Console.WriteLine($"Part1: {result}");
        Console.WriteLine($"Part1 time: {(end - start).TotalMilliseconds}");
    }

    private void Part2(string[] input)
    {
        var start = DateTime.Now;
        var result = input
            .Select(s => s.Split(" "))
            .Select(s => (Hand: new WildHand(s[0]), Bid: long.Parse(s[1])))
            .OrderBy(h => h.Hand)
            .Select((h, i) => h.Bid * (i + 1))
            .Sum();

        var end = DateTime.Now;
        Console.WriteLine($"Part2: {result}");
        Console.WriteLine($"Part2 time: {(end - start).TotalMilliseconds}");
    }
}

public abstract class Hand(string handString) : IComparable<Hand>
{
    protected readonly char[] Cards = handString
        .ToArray();

    protected int[]? _cardCounts;
    private int Rank =>
        (CardCounts[0], CardCounts.Length > 1 ? CardCounts[1] : 0) switch
        {
            (5, _) => 7,
            (4, _) => 6,
            (3, 2) => 5,
            (3, _) => 4,
            (2, 2) => 3,
            (2, _) => 2,
            (_, _) => 1
        };

    protected abstract int[] CardCounts { get; }

    public int CompareTo(Hand? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (Rank != other.Rank)
        {
            return Rank - other.Rank;
        }

        for (var i = 0; i < Cards.Length; i++)
        {
            if (Cards[i] != other.Cards[i])
            {
                return GetCardRank(Cards[i]) - GetCardRank(other.Cards[i]);
            }
        }

        return 0;
    }

    protected abstract int GetCardRank(char c);
}

public class NormalHand(string handString) : Hand(handString)
{
    protected override int[] CardCounts
    {
        get
        {
            if (_cardCounts != null)
            {
                return _cardCounts;
            }

            _cardCounts = Cards.GroupBy(c => c).Select(c => c.Count()).OrderDescending().ToArray();
            return _cardCounts;
        }
    }

    protected override int GetCardRank(char c)
        => c switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => c - '0'
        };
}

public class WildHand(string handString) : Hand(handString)
{
    protected override int[] CardCounts
    {
        get
        {
            if (_cardCounts != null)
            {
                return _cardCounts;
            }

            var cardCounts = Cards.GroupBy(c => c).Select(c => c.Count()).OrderDescending().ToArray();
            var jokers = Cards.Count(c => c == 'J');
            if (jokers == Cards.Length)
            {
                _cardCounts = new int[] { jokers };
                return _cardCounts;
            }
            var jokerCountIndex = Array.IndexOf(cardCounts, jokers);
            if (jokerCountIndex != -1)
            {
                cardCounts[jokerCountIndex] = 0;
                if (jokerCountIndex == 0)
                {
                    cardCounts[1] = Math.Min(Cards.Length, cardCounts[1] + jokers);
                }
                else
                {
                    cardCounts[0] = Math.Min(Cards.Length, cardCounts[0] + jokers);
                }
                cardCounts = cardCounts.OrderDescending().ToArray();
            }

            _cardCounts = cardCounts;
            return _cardCounts;
        }
    }

    protected override int GetCardRank(char c)
        => c switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 0,
            'T' => 10,
            _ => c - '0'
        };
}