using System.Drawing;
using Day10;

// parse input
const string input = "input.txt";
using var streamReader = new StreamReader(input);
var data = (await streamReader.ReadToEndAsync()).Split("\r\n");
var map = data.Select(line => line.ToCharArray()).ToArray(); // y,x
var startingNode = GetStart(map);

Console.WriteLine($"Part 1: {Part1(startingNode, map, out var path)}");
Console.WriteLine($"Part 2: {Part2(path, map)}");
return;

static int Part1(Node startingNode, char[][] map, out List<Node> path)
{
    // follow the 'snake' until back at the starting location. Then do / 2.
    var currentNode = startingNode;
    path = new List<Node>();
    var direction = StartingDirection(startingNode, map);
    
    do
    {
        var nextNode = Next(currentNode, direction, map, path, out var nextDirection);
        path.Add(nextNode);
        direction = nextDirection;
        currentNode = nextNode;
    } while (currentNode.Label != 'S');
    
    return path.Count / 2;
}

static int Part2(List<Node> bounds, char[][] map)
{
    // replace the S tile with its pipe version (is in last spot of the bounds list)
    bounds[^1] = bounds[^1] with {Label = NodeExtensions.ConfirmIdentityClockwise(bounds[^2], bounds[0])};
    map[bounds[^1].Y][bounds[^1].X] = bounds[^1].Label;
    
    var enclosedTiles = 0;
    var inside = false;
    var corner = '0';
    
    // For all rows, go from left to right
    // If a pipe is encountered, it means the next ground tile after is inside the loop
    // If another pipe is encountered, the ground tiles after that are outside of the loop and will not be counted.
    for (var y = 0; y < map.Length; y++)
    for (var x = 0; x < map[y].Length; x++)
    {
        var currentTile = map[y][x];
        var here = new Point(x, y);
        // if we are not on top of the bounds...
        if (!bounds.Any(node => node.X == here.X && node.Y == here.Y))
        {
            // if inside, increment
            if (!inside) continue;
            enclosedTiles++;
        }
        else
        {
            switch (currentTile)
            {
                case '|':
                    inside = !inside;
                    break;
                case 'F' or 'L':
                    corner = currentTile;
                    break;
                default:
                    switch (currentTile)
                    {
                        case 'J':
                        {
                            if (corner == 'F') inside = !inside;
                            corner = '0';
                            break;
                        }
                        case '7':
                        {
                            if (corner == 'L') inside = !inside;
                            corner = '0';
                            break;
                        }
                    }
                    break;
            }
        }
    }
    
    
    return enclosedTiles;
}

static Node Next(Node currentNode, Direction direction, char[][] map, List<Node> path, out Direction nextDirection)
{
    var nextNode = direction switch
    {
        Direction.North => new Node(map[currentNode.Y - 1][currentNode.X], currentNode.X, currentNode.Y - 1),
        Direction.East => new Node(map[currentNode.Y][currentNode.X + 1], currentNode.X + 1, currentNode.Y),
        Direction.South => new Node(map[currentNode.Y + 1][currentNode.X], currentNode.X, currentNode.Y + 1),
        Direction.West => new Node(map[currentNode.Y][currentNode.X - 1], currentNode.X - 1, currentNode.Y),
        _ => throw new ArgumentException("Direction does not exist.", nameof(direction))
    };

    nextDirection = nextNode.Exits.FirstOrDefault(e => !e.Equals(direction.Opposite()));

    return nextNode;
}

static Direction StartingDirection(Node startingNode, char[][] map)
{
    var width = map[0].Length;
    var height = map.Length;
    
    if (startingNode.Y - 1 >= 0) // North (y - 1)
    {
        var thatNode = new Node(map[startingNode.Y - 1][startingNode.X], startingNode.X, startingNode.Y - 1);
        if (thatNode.Exits.Contains(Direction.North.Opposite()))
            return Direction.North;
    }
    if (startingNode.X + 1 < width) // East (x + 1)
    {
        var thatNode = new Node(map[startingNode.Y][startingNode.X + 1], startingNode.X + 1, startingNode.Y);
        if (thatNode.Exits.Contains(Direction.East.Opposite()))
            return Direction.East;
    }
    if (startingNode.Y + 1 < height) // South (y + 1)
    {
        var thatNode = new Node(map[startingNode.Y + 1][startingNode.X], startingNode.X, startingNode.Y + 1);
        if (thatNode.Exits.Contains(Direction.South.Opposite()))
            return Direction.South;
    }
    if (startingNode.X - 1 >= 0) // West (x - 1)
    {
        var thatNode = new Node(map[startingNode.Y][startingNode.X - 1], startingNode.X - 1, startingNode.Y);
        if (thatNode.Exits.Contains(Direction.West.Opposite()))
            return Direction.West;
    }

    throw new ArgumentException("No direction possible from this node.", nameof(startingNode));
}

static Node GetStart(char[][] map)
{
    for (var y = 0; y < map.Length; y++)
    for (var x = 0; x < map[y].Length; x++)
    {
        if (map[y][x] != 'S')
            continue;
        
        return new Node('S', x, y);
    }
    

    throw new ArgumentException("Map does not contain a start.", nameof(map));
}