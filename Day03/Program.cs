using System.Drawing;
using System.Text;

var lines = await File.ReadAllLinesAsync("input.txt");
var grid = lines.Select(line => line.ToCharArray()).ToArray();

// Part 1: Given the engine schematic (input.txt), get all numbers that have a symbol around them (excluding dot (.)).
//         Get the sum of those part numbers.
Part1(grid, out var numbers, out var coordinates);
// Part 2: Given the engine schematic (input.txt), get all gear ratios. A gear is an asterisk (*) surrounded by exactly
//         2 numbers.
//         The gear ratio is a multiplication of the two numbers surrounding the gear.
//         Get the sum of all gear ratios.
Part2(grid, numbers, coordinates);
return;

static void Part1(IReadOnlyList<char[]> grid, out List<string> allNumbers, out List<List<Point>> allNumberCoordinates)
{
    var numberBuilder = new StringBuilder();
    var tempCoordinateSet = new List<Point>();
    
    var numbers = new List<string>();
    var coordinateSets = new List<List<Point>>();

    for (var y = 0; y < grid.Count; y++)
    {
        numberBuilder.Clear();
        tempCoordinateSet.Clear();

        for (var x = 0; x < grid[y].Length; x++)
        {
            if (!char.IsDigit(grid[y][x]))
            {
                if (numberBuilder.Length > 0)
                {
                    numbers.Add(numberBuilder.ToString());
                    coordinateSets.Add(new List<Point>(tempCoordinateSet));
                }
                numberBuilder.Clear();
                tempCoordinateSet.Clear();
                continue;
            }

            numberBuilder.Append(grid[y][x]);
            tempCoordinateSet.Add(new Point(x, y));
        }
        
        if (numberBuilder.Length > 0)
        {
            numbers.Add(numberBuilder.ToString());
            coordinateSets.Add(new List<Point>(tempCoordinateSet));
        }
    }

    var numbersWithSurroundingSymbol = new List<string>();
    for (var i = 0; i < coordinateSets.Count; i++)
    {
        var hasSurroundingSymbol = coordinateSets[i].Any(point => HasSurroundingSymbol(point.X, point.Y, grid));
        if (hasSurroundingSymbol) 
            numbersWithSurroundingSymbol.Add(numbers[i]);
    }

    allNumbers = new List<string>(numbers);
    allNumberCoordinates = new List<List<Point>>(coordinateSets);
    Console.WriteLine(numbersWithSurroundingSymbol.Sum(Convert.ToInt32));
}

static bool HasSurroundingSymbol(int centerX, int centerY, IReadOnlyList<char[]> grid)
{
    const char invalidSymbol = '.';
    
    var minX = Math.Max(0, centerX - 1);
    var maxX = Math.Min(grid[centerY].Length - 1, centerX + 1);
    var minY = Math.Max(0, centerY - 1);
    var maxY = Math.Min(grid.Count - 1, centerY + 1);
    
    for (var x = minX; x <= maxX; x++)
    {
        for (var y = minY; y <= maxY; y++)
        {
            if (x == centerX && y == centerY) 
                continue;
            
            var value = grid[y][x];

            if (!char.IsDigit(value) && value != invalidSymbol)
                return true;
        }
    }

    return false;
}

static void Part2(IReadOnlyList<char[]> grid, IReadOnlyList<string> numbers, IReadOnlyList<List<Point>> coordinates)
{
    var sum = 0;

    for (var y = 0; y < grid.Count; y++)
    {
        for (var x = 0; x < grid[y].Length; x++)
        {
            var isGear = IsGear(new Point(x, y), grid, numbers, coordinates, out var ratios);
            if (isGear)
            {
                sum += ratios.Item1 * ratios.Item2;
            }
        }
    }

    Console.WriteLine(sum);
}

static bool IsGear(Point point, IReadOnlyList<char[]> grid, IReadOnlyList<string> numbers,
    IReadOnlyList<List<Point>> coordinates, out (int, int) ratios)
{
    ratios = (0, 0);
    if (grid[point.Y][point.X] != '*') 
        return false;

    var surroundingNumberCoordinates = GetSurroundingNumberCoordinates(point, grid);
    var associatedNumbers = GetAssociatedNumbers(surroundingNumberCoordinates, numbers, coordinates);

    if (associatedNumbers.Count != 2) 
        return false;
    
    ratios = (Convert.ToInt32(associatedNumbers[0]), Convert.ToInt32(associatedNumbers[1]));
    return true;

}

static List<string> GetAssociatedNumbers(List<Point> numberCoordinates, IReadOnlyList<string> numbers,
    IReadOnlyList<List<Point>> coordinates)
{
    var associatedNumbers = new List<string>();
    var tempCoordinateSets = new List<List<Point>>();
    foreach (var coordinate in numberCoordinates)
    {
        for (var i = 0; i < coordinates.Count; i++)
        {
            if (!coordinates[i].Any(point => point.X == coordinate.X && point.Y == coordinate.Y) ||
                tempCoordinateSets.Contains(coordinates[i])) 
                continue;
            
            tempCoordinateSets.Add(coordinates[i]);
            associatedNumbers.Add(numbers[i]);
        }
    }

    return associatedNumbers;
}

static List<Point> GetSurroundingNumberCoordinates(Point coordinate, IReadOnlyList<char[]> grid)
{
    var minX = Math.Max(0, coordinate.X - 1);
    var maxX = Math.Min(grid[coordinate.Y].Length - 1, coordinate.X + 1);
    var minY = Math.Max(0, coordinate.Y - 1);
    var maxY = Math.Min(grid.Count - 1, coordinate.Y + 1);
    var coordinates = new List<Point>();
    
    for (var x = minX; x <= maxX; x++)
    {
        for (var y = minY; y <= maxY; y++)
        {
            if (x == coordinate.X && y == coordinate.Y) 
                continue;
            
            var value = grid[y][x];
            if (char.IsDigit(value)) 
                coordinates.Add(new Point(x, y));
        }
    }

    return coordinates;
}