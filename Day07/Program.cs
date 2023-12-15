using Day07;

const string input = "input.txt";
using var streamReader = new StreamReader(input);

var hands = new List<Hand>();
var jokerHands = new List<Hand>();
while (streamReader.Peek() > 0)
{
    var line = await streamReader.ReadLineAsync();
    if (!string.IsNullOrWhiteSpace(line))
    {
        hands.Add(new Hand(line));
        jokerHands.Add(new Hand(line, true));
    }
}

Console.WriteLine($"Part 1: {Part1(hands)}");
Console.WriteLine($"Part 2: {Part2(jokerHands)}");

return;

static long Part1(IReadOnlyList<Hand> hands)
{
    var tempHands = new List<Hand>(hands);
    tempHands.Sort();

    return tempHands.Select((hand, index) => (long)hand.Bet * (index + 1)).Sum();
}

static long Part2(IReadOnlyList<Hand> hands)
{
    var tempHands = new List<Hand>(hands);
    tempHands.Sort();

    return tempHands.Select((hand, index) => (long)hand.Bet * (index + 1)).Sum();
}