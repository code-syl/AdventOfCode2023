using Day05;

const string input = "input.txt";

using var streamReader = new StreamReader(input);
var data = (await streamReader.ReadToEndAsync()).Split("\r\n\r\n");
var seeds = data[0]
    .Split(": ")[1]
    .Split(" ")
    .Select(long.Parse)
    .ToList();
var maps = data[1..]
    .Select(chunk => new Map(chunk))
    .ToList();

Part1(seeds, maps);
Part2(seeds, maps);

return;

static long Part1(List<long> seeds, List<Map> maps)
{
    var minLocation = seeds
        .Select(seed => maps.Aggregate(seed, (current, map) => map.Transform(current)))
        .Min();

    return minLocation;
}

static void Part2(List<long> seeds, List<Map> maps)
{
    if (seeds.Count % 2 != 0)
        throw new ArgumentException("Seed list must be divisible by 2!");

    var newSeeds = new List<IEnumerable<long>>();
    for (var i = 0; i % 2 == 0 && i < seeds.Count; i += 2)
        newSeeds.Add(seeds[i].Range(seeds[i + 1]));

    var minima = new List<long>();
    for (var i = 0; i < newSeeds.Count; i++)
    {
        var min = Part1(newSeeds[i].ToList(), maps);
        minima.Add(min);
    }
    
    
    Console.WriteLine(minima.Min());
}

public static class ExtentionLong
{
    public static IEnumerable<long> Range(this long source, long length)
    {
        for (var i = source; i < length; i++)
        {
            yield return i;
        }
    }
}