using Day07;

const string input = "input.txt";
using var streamReader = new StreamReader(input);

var hands = new List<Hand>();
while (streamReader.Peek() > 0)
{
    var line = await streamReader.ReadLineAsync();
    if (!string.IsNullOrWhiteSpace(line))
        hands.Add(new Hand(line));
}

Console.WriteLine($"Part 1: {Part1(hands)}");
return;

static long Part1(IReadOnlyList<Hand> hands)
{
    var tempHands = new List<Hand>(hands);
    tempHands.Sort();

    return tempHands.Select((hand, index) => (long)hand.Bet * (index + 1)).Sum();
}