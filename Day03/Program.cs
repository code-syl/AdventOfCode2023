using System.Drawing;
using System.Text;

// parse input
var lines = await File.ReadAllLinesAsync("input.txt");
var grid = lines.Select(line => line.ToCharArray()).ToArray();

Part1(grid);
return;

static void Part1(char[][] grid)
{
    var numberBuilder = new StringBuilder();
    var tempCoordinateSet = new List<Point>();
    
    var numbers = new List<string>();
    var coordinateSets = new List<List<Point>>();

    for (var y = 0; y < grid.Length; y++)
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

    Console.WriteLine(numbersWithSurroundingSymbol.Sum(Convert.ToInt32));

}

static bool HasSurroundingSymbol(int centerX, int centerY, char[][] grid)
{
    const char invalidSymbol = '.';
    
    var minX = Math.Max(0, centerX - 1);
    var maxX = Math.Min(grid[centerY].Length - 1, centerX + 1);
    var minY = Math.Max(0, centerY - 1);
    var maxY = Math.Min(grid.Length - 1, centerY + 1);
    
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