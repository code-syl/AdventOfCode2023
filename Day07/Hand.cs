namespace Day07;

public record Hand : IComparable<Hand>
{
    public int Bet { get; }
    private readonly HandType _type;
    private readonly List<Card> _cards = new ();
    
    public Hand(string line, bool jokerMode = false)
    {
        var split = line.Split(" ");
        Bet = int.Parse(split[1]);
        
        foreach (var c in split[0])
        {
            _cards.Add(new Card(c,
                Array.IndexOf(jokerMode ? HandExtensions.CharLabelsJoker : HandExtensions.CharLabels, c)));
        }
        
        _type = jokerMode ? HandExtensions.GetHandTypeJokerMode(split[0]) : HandExtensions.GetHandType(split[0]);
    }

    public int CompareTo(Hand? other)
    {
        if (other is null)
            return 1;
        
        var value = _type.CompareTo(other._type);
        if (value == 0)
        {
            return other!._cards
                .Select((_, card) => _cards[card].CompareTo(other._cards[card]))
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
    public static readonly char[] CharLabels = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' }; 
    public static readonly char[] CharLabelsJoker = { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
    
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

    public static HandType GetHandTypeJokerMode(string handString)
    {
        // Joker = J and can act as another card, making it possible to have a higher type.
        // For example, JJJJ2 is a FiveOfAKind, but JJJJ3 is a FiveOfAKind with a higher value.
        var possibleHands = GeneratePossibleHandsWithJoker(handString);
        return possibleHands.Max(GetHandType);
    }
    
    private static bool HasDefinedUniques(this string input, int count, int countCount)
    {
        return input
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count())
            .Values
            .Count(v => v == count) == countCount;
    }

    private static IEnumerable<string> GeneratePossibleHandsWithJoker(string handString)
    {
        var jokerCount = handString.Count(c => c == 'J');
        var cardLabels = new[] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

        foreach (var cardLabel in cardLabels)
        {
            var possibleHand = handString;
            for (var i = 0; i < jokerCount; i++)
            {
                possibleHand = possibleHand.Replace('J', cardLabel);
            }
            
            yield return possibleHand;
        }
    }
}