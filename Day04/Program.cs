using System.Text.RegularExpressions;
using Day04;

var lines = await File.ReadAllLinesAsync("input.txt");
var scratchCards = lines.Select(line =>
{
    var cardRegexResultGroups = CardRegex().Match(line).Groups; // https://stackoverflow.com/a/17252641
    var id = Convert.ToInt32(cardRegexResultGroups[1].Value);
    var selectedNumbers = AnyWhitespaceRegex().Split(cardRegexResultGroups[2].Value).ToScratchCardNumbers();
    var winningNumbers = AnyWhitespaceRegex().Split(cardRegexResultGroups[3].Value).ToScratchCardNumbers();
    return new ScratchCard {Id = id, SelectedNumbers = selectedNumbers, WinningNumbers = winningNumbers};
}).ToList();

// Part 1: Given the scratch card input, per card, get the number of selected winning numbers.
//         0 or 1 winning picks equals 0 or 1 points. 1+ winning picks = 2^{winning picks - 1} points
//         Get the sum of total points.
Part1(scratchCards);
// Part 2: Given the scratch card input, per card, get the number of selected winning numbers.
//         Per card, for each winning number, a copy of the next x cards will be generated. 
//         For example: if Card 1 has 5 winning picks, card 2-3-4-5-6 will be copied one time.
//         If Card 2 has 2 winning picks, card 3-4 will be copied TWICE, because there are two Card 2s.
//         You cannot go beyond the last card (Card 219, in this case).
//         How many total scratch cards do you end up with?
Part2(scratchCards);
return;

static void Part1(IEnumerable<ScratchCard> cards)
{
    var sum = cards.Select(card => card.SelectedNumbers.Intersect(card.WinningNumbers).Count())
        .Select(winningPicks => winningPicks switch
        {
            <= 1 => winningPicks,
            _ => (int) Math.Pow(2, winningPicks - 1)
        })
        .Sum();
    
    Console.WriteLine(sum);
}

static void Part2(IReadOnlyList<ScratchCard> cards)
{
    var i = 0;
    while (i < cards.Count)
    {
        for (var j = 0; j < cards[i].Copies; j++)
        {
            var winningPicksX = 0;
            foreach (var pick in cards[i].SelectedNumbers) // do not convert to LINQ
            {
                if (cards[i].WinningNumbers.Contains(pick)) winningPicksX++;
            }
            
            for (var k = 0; k < winningPicksX && i + k + 1 < cards.Count; k++)
            {
                cards[i + k + 1].Copies++;
            }
        }
        i++;
    }

    var sumOfCards = cards.Select(c => c.Copies).Sum();

    Console.WriteLine(sumOfCards);
}

partial class Program
{
    [GeneratedRegex(@"^Card\s+(.+?)\:\s+(.+?)\s\|\s+(.+?)$")]
    private static partial Regex CardRegex();
    [GeneratedRegex(@"\s+")]
    private static partial Regex AnyWhitespaceRegex();
}

internal static class Extensions
{
    internal static List<int> ToScratchCardNumbers(this IEnumerable<string> input)
    {
        return input
            .Where(s => s != string.Empty)
            .Select(s => Convert.ToInt32(s))
            .OrderBy(n => n)
            .ToList();
    }
}