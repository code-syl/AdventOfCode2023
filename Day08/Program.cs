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

partial class Program
{
    [GeneratedRegex(@"\((.*?)\)")]
    private static partial Regex LeftRightRegex();
}