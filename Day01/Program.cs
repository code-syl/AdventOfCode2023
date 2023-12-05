var lines = await File.ReadAllLinesAsync("input.txt");

// Part 1: Get the first and last digit per line of input.txt. Combine into whole number and sum.
await Part1(lines);
// Part 2: Same as part 1, but now digits can also be spelled out.
await Part2(lines);
return;

static void Part1(string[] lines)
{
    var sum =
        lines.Select(line => line.First(char.IsDigit) + "" + line.Last(char.IsDigit))
            .Select(firstAndLastDigit => Convert.ToInt32(firstAndLastDigit)).Sum();

    Console.WriteLine("Part 1: " + sum);
}


static void Part2(string[] lines)
{
    var stringToNumbers = new Dictionary<string, int>
    {
        { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 },
        { "eight", 8 }, { "nine", 9 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 }, { "6", 6 },
        { "7", 7 }, { "8", 8 }, { "9", 9 }
    };

    var sum = lines.Select(line =>
    {
        var first = GetFirstAppearance(stringToNumbers, line);
        var last = GetLastAppearance(stringToNumbers, line);
        return Convert.ToInt32(first + last);
    }).Sum();

    Console.WriteLine("Part 2: " + sum);
}

static string GetFirstAppearance(Dictionary<string, int> dictionary, string input)
{
    return dictionary.Select(kvp => (Index: input.IndexOf(kvp.Key, StringComparison.Ordinal), Value: kvp.Value))
        .Where(ivp => ivp.Index >= 0)
        .MinBy(ivp => ivp.Index)
        .Value
        .ToString();
}

static string GetLastAppearance(Dictionary<string, int> dictionary, string input)
{
    return dictionary.Select(kvp => (Index: input.LastIndexOf(kvp.Key, StringComparison.Ordinal), Value: kvp.Value))
        .Where(ivp => ivp.Index >= 0)
        .MaxBy(ivp => ivp.Index)
        .Value
        .ToString();
}