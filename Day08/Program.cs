using System.Text.RegularExpressions;
using Day08;

// Parse the input
const string input = "input.txt";
using var streamReader = new StreamReader(input);
var data = (await streamReader.ReadToEndAsync()).Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

var instructions = data[0];
var mapPoints = data[1..].Select(line =>
{
    var location = line.Split('=', StringSplitOptions.TrimEntries)[0];
    var leftRight = LeftRightRegex().Match(line).Groups[1].Value.Split(',', StringSplitOptions.TrimEntries);
    return new MapPoint(location, leftRight[0], leftRight[1]);
}).OrderBy(mp => mp.Location).ToList();

Console.WriteLine($"Part 1: {Part1(instructions, mapPoints)}");
Console.WriteLine($"Part 2: {Part2(instructions, mapPoints)}");
return;


static int Part1(string instructions, IReadOnlyList<MapPoint> mapPoints)
{
    // Based on the instructions, go to the left or right element of the map point you are currently at. 
    // How many steps does it take to go to ZZZ, starting at AAA? 
    // If the instructions do not lead to ZZZ, repeat the instructions until you reach ZZZ.
    var steps = 0;
    var reached = false;
    var currentLocation = "AAA";

    while (!reached)
    {
        foreach (var step in instructions)
        {
            if (currentLocation == "ZZZ")
            {
                reached = true;
                break;
            }
            
            var tempLocation = currentLocation;
            var nextLocation = step switch
            {
                'L' => mapPoints.First(mp => mp.Location == tempLocation).Left,
                'R' => mapPoints.First(mp => mp.Location == tempLocation).Right,
                _ => string.Empty
            };

            currentLocation = nextLocation;
            steps++;
        }
    }
    
    return steps;
}

static long Part2(string instructions, IReadOnlyList<MapPoint> mapPoints)
{
    // This only works because of the structure of the input file. :(
    var startingLocations = mapPoints.Where(mp => mp.Location.EndsWith('A')).Select(mp => mp.Location);
    var stepsPerStartingLocation = new List<long>();

    foreach (var startingLocation in startingLocations)
    {
        long steps = 0;
        var reached = false;
        var currentLocation = startingLocation;

        while (!reached)
        {
            foreach (var step in instructions)
            {
                if (currentLocation.EndsWith('Z'))
                {
                    reached = true;
                    break;
                }
            
                var tempLocation = currentLocation;
                var nextLocation = step switch
                {
                    'L' => mapPoints.First(mp => mp.Location == tempLocation).Left,
                    'R' => mapPoints.First(mp => mp.Location == tempLocation).Right,
                    _ => string.Empty
                };

                currentLocation = nextLocation;
                steps++;
            }
        }
        
        stepsPerStartingLocation.Add(steps);
    }

    return LeastCommonMultiple(stepsPerStartingLocation);
}

static long LeastCommonMultiple(List<long> numbers)
{
    // https://stackoverflow.com/a/29717490
    // https://www.mathsisfun.com/least-common-multiple.html
    return numbers.Aggregate((a, b) => (Math.Abs(a * b) / GreatestCommonDivisor(a, b)));
}

static long GreatestCommonDivisor(long a, long b)
{
    while (true)
    {
        // https://stackoverflow.com/a/29717490
        // https://www.mathsisfun.com/greatest-common-factor.html
        // https://en.wikipedia.org/wiki/Euclidean_algorithm
        if (b == 0) return a;
        var a1 = a;
        a = b;
        b = a1 % b;
    }
}

partial class Program
{
    [GeneratedRegex(@"\((.*?)\)")]
    private static partial Regex LeftRightRegex();
}