using Advent2023.Days;

if (args.Length != 1)
{
    Console.WriteLine("Please provide the day to solve");
    return;
}

var type = Type.GetType($"Advent2023.Days.Day{args[0]}");
if (type == null)
{
    Console.WriteLine($"Unable to solve Day {args[0]}");
    return;
}

var day = Activator.CreateInstance(type) as IDay;
if (day == null)
{
    Console.WriteLine($"Unable to solve Day {args[0]}");
    return;
}

day.Solve($"./inputs/day{args[0]}.txt");
