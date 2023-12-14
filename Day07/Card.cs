namespace Day07;

public record Card(char Label, int Value) : IComparable<Card>
{
    public int CompareTo(Card? other)
    {
        return other is null ? 1 : Value.CompareTo(other.Value);
    }
}