namespace Day05;

public class Set
{
    public long DestinationStart { get; }
    public long SourceStart { get; }
    public long RangeLength { get; }
    public long From => SourceStart;
    public long To => SourceStart + RangeLength - 1;
    public long Adjustment => DestinationStart - SourceStart;

    public Set(string data)
    {
        var numbers = data.Split(" ").Select(long.Parse).ToList();
        if (numbers.Count != 3)
            throw new ArgumentException("The data is dirty, needs 3 numbers separated by a space.");

        DestinationStart = numbers[0];
        SourceStart = numbers[1];
        RangeLength = numbers[2];
    }

    public bool IsInRange(long number) => (number >= SourceStart) && (number < SourceStart + RangeLength);
    public long Transform(long source) => source + (DestinationStart - SourceStart);
}