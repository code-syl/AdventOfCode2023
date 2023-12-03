using System.Text;

// parse input
var lines = await File.ReadAllLinesAsync("input.txt");
var grid = lines.Select(line => line.ToCharArray()).ToArray();

//Part1(grid);
var x = HasSurroundingSymbol(8, 0, grid);
Console.WriteLine(x);

// get 2d array of characters
// loop through each character and check if it's a number
// if it is a number, check its surroundings for symbols not in the invalidCharacters array
    // if a symbol is found flag it
    // continue through the rest of the number and add it to a list

static void Part1(char[][] grid)
{
    var numberBuilder = new StringBuilder();
    var numbersWithSymbolsAround = new List<int>();
    
    for (var y = 0; y < grid.Length; y++)
    {
        for (var x = 0; x < grid[y].Length; x++)
        {
            if (!char.IsDigit(grid[y][x])) continue;
            var hasSurroundingSymbol = HasSurroundingSymbol(x, y, grid);

        }
    }
}

static bool HasSurroundingSymbol(int centerX, int centerY, char[][] grid)
{
    const char invalidSymbol = '.';
    // select all surrounding coordinates from the x and y of grid
    
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