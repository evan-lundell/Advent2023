namespace Advent2023.Days;

public class Day3 : IDay
{
    public void Solve(string[] input)
    {
        // Part1(input);
        Part2(input);
    }

    private void Part2(string[] input)
    {
        var numberSections = input.SelectMany(GetNumbers).ToArray();
        var sum = 0;
        for (int rowIndex = 0; rowIndex < input.Length; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < input[rowIndex].Length; columnIndex++)
            {
                if (input[rowIndex][columnIndex] == '*')
                {
                    var adjacentNumbers = numberSections.Where(ns => ns.IsAdjacent(rowIndex, columnIndex)).ToArray();
                    if (adjacentNumbers.Length == 2)
                    {
                        sum += adjacentNumbers[0].Value * adjacentNumbers[1].Value;
                    }
                }
            }
        }

        Console.WriteLine($"Part2: {sum}");
    }
    
    private void Part1(string[] input)
    {
        var numberSections = input.SelectMany(GetNumbers);
        var result = numberSections.Where(numberSection =>
            {
                var startIndex = Math.Max(0, numberSection.StartIndex - 1);
                var endIndex = Math.Min(input[numberSection.Row].Length - 1, numberSection.EndIndex + 1);

                for (var index = startIndex; index <= endIndex; index++)
                {
                    if (numberSection.Row != 0 && input[numberSection.Row - 1][index] != '.' && !char.IsDigit(input[numberSection.Row - 1][index]))
                    {
                        return true;
                    }
                    if (numberSection.Row != input.Length - 1 && input[numberSection.Row + 1][index] != '.' && !char.IsDigit(input[numberSection.Row + 1][index]))
                    {
                        return true;
                    }
                }
                if (numberSection.StartIndex != 0 && input[numberSection.Row][numberSection.StartIndex - 1] != '.')
                {
                    return true;
                }
                
                if (numberSection.EndIndex != input[numberSection.Row].Length - 1 && input[numberSection.Row][numberSection.EndIndex + 1] != '.')
                {
                    return true;
                }
                return false;
            })
        .Select(ns => ns.Value)
        .Sum();
        Console.WriteLine($"Part1: {result}");
    }

    private IEnumerable<NumberSection> GetNumbers(string line, int rowIndex)
    {
        var numbers = new List<NumberSection>();
        var findingNumber = false;
        var startIndexOfNumber = 0;
        for (var i = 0; i < line.Length; i++)
        {
            var currentChar = line[i];
            if (!char.IsDigit(currentChar))
            {
                if (findingNumber)
                {
                    numbers.Add(new NumberSection(rowIndex, startIndexOfNumber, i - 1, int.Parse(line.Substring(startIndexOfNumber, i - startIndexOfNumber))));
                    findingNumber = false;
                }
                
                continue;
            }

            if (!findingNumber)
            {
                findingNumber = true;
                startIndexOfNumber = i;
            }
        }

        if (findingNumber)
        {
            numbers.Add(new NumberSection(rowIndex, startIndexOfNumber, line.Length - 1, int.Parse(line.Substring(startIndexOfNumber))));
        }

        return numbers;
    }
}

class NumberSection(int row, int startIndex, int endIndex, int value)
{
    public int Row => row;
    public int StartIndex => startIndex;
    public int EndIndex => endIndex;
    public int Value => value;

    public bool IsAdjacent(int rowIndex, int columnIndex)
    {
        if (Row == rowIndex - 1 || Row == rowIndex + 1)
        {
            if ((StartIndex >= columnIndex - 1 && StartIndex <= columnIndex + 1) ||
                EndIndex >= columnIndex - 1 && EndIndex <= columnIndex + 1)
            {
                return true;
            }
        }

        return Row == rowIndex && (EndIndex == columnIndex - 1 || StartIndex == columnIndex + 1);
    }
}