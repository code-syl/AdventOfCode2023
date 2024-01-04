using System.Drawing;

namespace Day10;

public record Node(char Label, int X, int Y)
{
    public List<Direction> Exits => Label switch
    {
        '|' => new List<Direction> {Direction.North, Direction.South},
        '-' => new List<Direction> {Direction.West, Direction.East},
        'L' => new List<Direction> {Direction.North, Direction.East},
        'J' => new List<Direction> {Direction.North, Direction.West},
        '7' => new List<Direction> {Direction.West, Direction.South},
        'F' => new List<Direction> {Direction.East, Direction.South},
        _   => new List<Direction>() // others are either invalid or have no exits
    };
}

public enum Direction
{
    North, East, South, West
}

public static class DirectionExtensions
{
    public static Direction Opposite(this Direction input)
    {
        return input switch
        {
            Direction.East => Direction.West,
            Direction.North => Direction.South,
            Direction.West => Direction.East,
            Direction.South => Direction.North,
            _ => throw new ArgumentException("Direction does not exist.", nameof(input))
        };
    }
}

public static class NodeExtensions
{
    // mainly to see which pipe type 'S' is, but should also be able to identify other pipes
    public static char ConfirmIdentityClockwise(Node previous, Node next)
    {
        var nextExits = next.Exits;
        var previousExits = previous.Exits;

        if (previousExits.Any(e => e is Direction.West) &&
            nextExits.Any(e => e is Direction.South))
        {
            return 'L';
        }
        
        if (previousExits.Any(e => e is Direction.North) &&
            nextExits.Any(e => e is Direction.West))
        {
            return 'F';
        }
        
        if (previousExits.Any(e => e is Direction.North) &&
            nextExits.Any(e => e is Direction.South))
        {
            return '|';
        }
        
        if (previousExits.Any(e => e is Direction.East) &&
            nextExits.Any(e => e is Direction.West))
        {
            return '-';
        }
        
        if (previousExits.Any(e => e is Direction.East) &&
            nextExits.Any(e => e is Direction.North))
        {
            return '7';
        }
        
        if (previousExits.Any(e => e is Direction.South) &&
            nextExits.Any(e => e is Direction.East))
        {
            return 'J';
        }

        throw new ArgumentException("No connections.");
    }
}