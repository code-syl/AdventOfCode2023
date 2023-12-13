namespace Day05;

public class Set
{
    public readonly long _destinationStart;
    private readonly long _sourceStart;
    private readonly long _rangeLength;

    public Set(string data)
    {
        var numbers = data.Split(" ").Select(long.Parse).ToList();
        if (numbers.Count != 3)
            throw new ArgumentException("The data is dirty, needs 3 numbers separated by a space.");

        _destinationStart = numbers[0];
        _sourceStart = numbers[1];
        _rangeLength = numbers[2];
    }

    public bool IsInRange(long number) => (number >= _sourceStart) && (number < _sourceStart + _rangeLength);
    public long Transform(long source) => source + (_destinationStart - _sourceStart);
}