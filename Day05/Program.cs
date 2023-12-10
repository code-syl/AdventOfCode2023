// Parse Input:

using System.Text.RegularExpressions;

var seeds = Array.Empty<string>();
try
{
    var streamReader = new StreamReader("input.txt");
    seeds = Regex.Match(
        await streamReader.ReadLineAsync() ?? string.Empty, 
        @"^seeds\:\s(.+)$"
        ).Groups[1].Value.Split(" ");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Environment.Exit(-1);
}

Console.WriteLine(seeds);