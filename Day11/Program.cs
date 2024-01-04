using System.Drawing;

// parse input
const string input = "input.txt";
using var streamReader = new StreamReader(input);
var data = (await streamReader.ReadToEndAsync()).Split("\r\n");

var universe = data.Select(line => line.ToCharArray()).ToArray();
var rows = Enumerable.Range(0, universe.Length).ToHashSet();
var columns = Enumerable.Range(0, universe[0].Length).ToHashSet();
var galaxies = new List<Point>();
for (var y = 0; y < universe.Length; y++)
    for (var x = 0; x < universe[y].Length; x++)
    {
        if (universe[y][x] == '#')
        {
            galaxies.Add(new Point(x, y));
            // remove the rows and columns attached to the galaxy, as they do not expand
            rows.Remove(y);
            columns.Remove(x);
        }
    }

Console.WriteLine($"Part 1: {TotalTravelTime(galaxies, 2L, (rows, columns))}");
Console.WriteLine($"Part 2: {TotalTravelTime(galaxies, 1_000_000L, (rows, columns))}");
return;

static long TotalTravelTime(List<Point> galaxies, long expansionFactor, (HashSet<int> Rows, HashSet<int> Columns) emptyGalaxy)
{
    var sum = 0L;
    
    for (var i = 0; i < galaxies.Count - 1; i++)
        for (var j = i + 1; j < galaxies.Count; j++)
        {
            var minX = Math.Min(galaxies[i].X, galaxies[j].X);
            var maxX = Math.Max(galaxies[i].X, galaxies[j].X);
            var minY = Math.Min(galaxies[i].Y, galaxies[j].Y);
            var maxY = Math.Max(galaxies[i].Y, galaxies[j].Y);

            var expansionX = (expansionFactor - 1) * emptyGalaxy.Columns.Count(x => x > minX && x < maxX);
            var expansionY = (expansionFactor - 1) * emptyGalaxy.Rows.Count(y => y > minY && y < maxY);

            // use taxicab distance https://en.wikipedia.org/wiki/Taxicab_geometry
            // where it is easier to calculate the distance by relative x and relative y
            // take the expansion into account
            var taxicabNormal = Math.Abs(galaxies[i].X - galaxies[j].X) + Math.Abs(galaxies[i].Y - galaxies[j].Y);
            var taxicabWithExpansion = taxicabNormal + expansionX + expansionY;

            sum += taxicabWithExpansion;
        }

    return sum;
}