await Part1();
await Part2();


static async Task Part1()
{
    var lines = (await File.ReadAllLinesAsync("input.txt"));
    
    var sum =
        lines.Select(line => line.First(char.IsDigit) + "" + line.Last(char.IsDigit))
            .Select(firstAndLastDigit => Convert.ToInt32(firstAndLastDigit)).Sum();

    Console.WriteLine("Part 1: " + sum);
}


static async Task Part2()
{
    var stringToNumbers = new Dictionary<string, int>
    {
        { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 },
        { "eight", 8 }, { "nine", 9 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 }, { "6", 6 },
        { "7", 7 }, { "8", 8 }, { "9", 9 }
    };
    var lines = await File.ReadAllLinesAsync("input.txt");

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