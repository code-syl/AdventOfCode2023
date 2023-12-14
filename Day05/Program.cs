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

Console.WriteLine($"Part 1: {Part1(seeds, maps)}");
Console.WriteLine($"Part 2: {Part2(seeds, maps)}");

return;

static long Part1(IEnumerable<long> seeds, IReadOnlyCollection<Map> maps)
{
    var minLocation = seeds
        .Select(seed => maps.Aggregate(seed, (current, map) => map.Transform(current)))
        .Min();

    return minLocation;
}

static long Part2(IReadOnlyList<long> seeds, List<Map> maps)
{
    if (seeds.Count % 2 != 0)
        throw new ArgumentException("Seed list must be divisible by 2!");

    var seedRanges = new List<(long From, long To)>();
    for (var i = 0; i < seeds.Count; i += 2) 
        seedRanges.Add((From: seeds[i], To: seeds[i] + seeds[i + 1] - 1));
    
    foreach (var map in maps)
    {
        var orderedSets = map.Sets.OrderBy(s => s.From).ToList();
        var newRanges = new List<(long From, long To)>();
        foreach (var seedRange in seedRanges)
        {
            var range = seedRange;
            foreach (var orderedSet in orderedSets)
            {
                if (range.From < orderedSet.From)
                {
                    newRanges.Add((range.From, Math.Min(range.To, orderedSet.From - 1)));
                    range.From = orderedSet.From;
                    if (range.From > range.To)
                        break;
                }

                if (range.From <= orderedSet.To)
                {
                    newRanges.Add((range.From + orderedSet.Adjustment, Math.Min(range.To, orderedSet.To) + orderedSet.Adjustment));
                    range.From = orderedSet.To + 1;
                    if (range.From > range.To)
                        break;
                }
            }
            
            if (range.From <= range.To)
                newRanges.Add(range);
        }

        seedRanges = newRanges;
    }

    return seedRanges.Min(r => r.From);
}