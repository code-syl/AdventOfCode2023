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

static void Part1(List<long> seeds, List<Map> maps)
{
    var minLocation = seeds
        .Select(seed => maps.Aggregate(seed, (current, map) => map.Transform(current)))
        .Min();

    Console.WriteLine(minLocation);
}

static void Part2(List<long> seeds, List<Map> maps)
{
    if (seeds.Count % 2 != 0)
        throw new ArgumentException("Seed list must be divisible by 2!");

}