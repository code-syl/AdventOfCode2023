namespace Day07;

public record Hand : IComparable<Hand>
{
    public List<Card> Cards { get; init; } = new ();
    public int Bet { get; init; }
    public HandType Type { get; init; }
    
    private readonly char[] _cardLabels = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
    
    public Hand(string line)
    {
        var split = line.Split(" ");
        Bet = int.Parse(split[1]);
        
        foreach (var c in split[0])
        {
            Cards.Add(new Card(c, Array.IndexOf(_cardLabels,c)));
        }

        Type = HandExtensions.GetHandType(split[0]);
    }

    public int CompareTo(Hand? other)
    {
        if (other is null)
            return 1;
        
        var value = Type.CompareTo(other.Type);
        if (value == 0)
        {
            return other!.Cards
                .Select((_, card) => Cards[card].CompareTo(other.Cards[card]))
                .FirstOrDefault(result => result != 0);
        }
        
        return value;
    }
}

public enum HandType
{
    HighCard = 1,
    OnePair = 2,
    TwoPair = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7
}

public static class HandExtensions
{
    public static HandType GetHandType(string handString)
    {
        return handString switch
        {
            _ when handString.HasDefinedUniques(5, 1) => HandType.FiveOfAKind,
            _ when handString.HasDefinedUniques(4, 1) && handString.HasDefinedUniques(1, 1) => 
                HandType.FourOfAKind,
            _ when handString.HasDefinedUniques(3, 1) && handString.HasDefinedUniques(2, 1) =>
                HandType.FullHouse,
            _ when handString.HasDefinedUniques(3, 1) && handString.HasDefinedUniques(1, 2) =>
                HandType.ThreeOfAKind,
            _ when handString.HasDefinedUniques(2, 2) && handString.HasDefinedUniques(1, 1) => 
                HandType.TwoPair,
            _ when handString.HasDefinedUniques(2, 1) && handString.HasDefinedUniques(1, 3) => 
                HandType.OnePair,
            _ => HandType.HighCard
        };
    }
    private static bool HasDefinedUniques(this string input, int count1, int countCount, int count2 = 0)
    {
        count2 = count2 == 0 ? count1 : count2;

        return input
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count())
            .Values
            .Count(v => v == count1 || v == count2) == countCount;
    }
}