namespace Advent2023.Days;

public class Day5 : IDay
{
    public void Solve(string[] input)
    {
        var seeds = input[0]
            .Split(":", StringSplitOptions.TrimEntries)[1]
            .Split(" ")
            .Select(long.Parse)
            .ToArray();
        var seedToSoilMapping = Mapping.CreateMapping(input[3..18]);
        var soilToFertilizer = Mapping.CreateMapping(input[20..32]);
        var fertilizerToWater = Mapping.CreateMapping(input[34..72]);
        var waterToLight = Mapping.CreateMapping(input[74..98]);
        var lightToTemperature = Mapping.CreateMapping(input[100..129]);
        var temperatureToHumidity = Mapping.CreateMapping(input[131..162]);
        var humidityToLocation = Mapping.CreateMapping(input[164..]);
        Part1(seeds, seedToSoilMapping, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation);
        Part2(seeds, seedToSoilMapping, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation);
    }

    private void Part1(
        long[] seeds,
        Mapping seedToSoilMapping,
        Mapping soilToFertilizer,
        Mapping fertilizerToWater,
        Mapping waterToLight,
        Mapping lightToTemperature,
        Mapping temperatureToHumidity,
        Mapping humidityToLocation)
    {
        var min = seeds.Select(s => humidityToLocation.GetMappedValue(temperatureToHumidity.GetMappedValue(
                lightToTemperature.GetMappedValue(
                    waterToLight.GetMappedValue(
                        fertilizerToWater.GetMappedValue(soilToFertilizer.GetMappedValue(seedToSoilMapping.GetMappedValue(s))))))))
            .Min();
        Console.WriteLine($"Part1: {min}");
    }

    private void Part2(
        long[] seeds,
        Mapping seedToSoilMapping,
        Mapping soilToFertilizer,
        Mapping fertilizerToWater,
        Mapping waterToLight,
        Mapping lightToTemperature,
        Mapping temperatureToHumidity,
        Mapping humidityToLocation)
    {
        long min = long.MaxValue;
        for (int i = 0; i < seeds.Length / 2; i++)
        {
            var start = seeds[i * 2];
            var end = start + seeds[i * 2 + 1];
            var seedRange = new LongRange(start, end);
            var soilRange = seedToSoilMapping.GetMappedRanges(new[] { seedRange });
            var fertilizerRange = soilToFertilizer.GetMappedRanges(soilRange);
            var waterRange = fertilizerToWater.GetMappedRanges(fertilizerRange);
            var lightRange = waterToLight.GetMappedRanges(waterRange);
            var temperatureRange = lightToTemperature.GetMappedRanges(lightRange);
            var humidityRange = temperatureToHumidity.GetMappedRanges(temperatureRange);
            var locationRange = humidityToLocation.GetMappedRanges(humidityRange);
            min = Math.Min(locationRange.Select(l => l.Start).Min(), min);
        }
        
        Console.WriteLine($"Part2: {min}");
    }
}

public class Mapping (MappingRange[] ranges)
{
    public MappingRange[] Ranges => ranges;
    public long GetMappedValue(long key)
    {
        var mappingRange = ranges.FirstOrDefault(r => key >= r.Source && key < r.SourceEnd);
        return mappingRange == null ? key : mappingRange.Destination + (key - mappingRange.Source);
    }

    public IEnumerable<LongRange> GetMappedRanges(IEnumerable<LongRange> ranges)
    {
        var mappedRanges = new List<LongRange>();
        foreach (var range in ranges)
        {
            var matchedRanges = Ranges.Where(r => !(range.End <= r.Source || range.Start >= r.SourceEnd))
                .OrderBy(r => r.Source)
                .ToArray();
            if (matchedRanges.Length == 0)
            {
                mappedRanges.Add(range);
                continue;
            }

            if (range.Start < matchedRanges[0].Source)
            {
                mappedRanges.Add(new LongRange(range.Start, matchedRanges[0].Source));
            }

            if (range.End > matchedRanges[^1].SourceEnd)
            {
                mappedRanges.Add(new LongRange(matchedRanges[^1].SourceEnd, range.End));
            }

            foreach (var matchedRanged in matchedRanges)
            {
                var difference = matchedRanged.Destination - matchedRanged.Source;
                mappedRanges.Add(new LongRange(Math.Max(matchedRanged.Source, range.Start) + difference, Math.Min(matchedRanged.SourceEnd, range.End) + difference));
            }
        }

        return mappedRanges;
    }

    public static Mapping CreateMapping(string[] lines)
        => new (lines.Select(line => line.Split(" ").Select(long.Parse).ToArray())
            .Select(nums => new MappingRange(nums[0], nums[1], nums[2])).ToArray());
}

public record MappingRange(long Destination, long Source, long Length)
{
    public long SourceEnd => Source + Length;
    public long DestinationEnd => Destination + Length;
}

public struct LongRange(long start, long end)
{
    public long Start => start;
    public long End => end;
}
