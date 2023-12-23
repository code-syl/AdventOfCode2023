// Parse the input
const string input = "input.txt";
using var streamReader = new StreamReader(input);
var data = (await streamReader.ReadToEndAsync()).Split("\r\n");

var sequences = data.Select(line => line.Split(' ').Select(long.Parse).ToList()).ToList();
foreach (var sequence in sequences)
{
    Console.WriteLine(string.Join(' ', sequence));
    var differences = new List<long>();
    for (var i = 0; i < sequence.Count - 1; i++)
    {
        differences.Add(sequence[i + 1] - sequence[i]);
    }
    Console.WriteLine(string.Join(' ', differences));
    Console.WriteLine();
    
    // not done
}