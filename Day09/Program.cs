// Parse the input
const string input = "input.txt";
using var streamReader = new StreamReader(input);
var data = (await streamReader.ReadToEndAsync()).Split("\r\n");

var sequences = data.Select(line => line.Split(' ').Select(long.Parse).ToList()).ToList();
var solution = NextValues(sequences);
Console.WriteLine($"Part 1: {solution.Part1}");
Console.WriteLine($"Part 2: {solution.Part2}");
return;

static (long Part1, long Part2) NextValues(List<List<long>> sequences)
{
    // Extrapolate the next value in the sequence, based on the previous values
    // For example, in the sequence 3 6 9 12, the next value is 15
    // In the sequence 2 4 8 16, the next value is 32
    // And backwards...
    var partOne = 0L;
    var partTwo = 0L;
    
    foreach (var sequence in sequences)
    {
        var tempNumbers = new List<long>(sequence);
        var differences = new List<List<long>> {tempNumbers};

        while (tempNumbers.Any(number => number != 0))
        {
            tempNumbers = new List<long>();

            for (var i = 0; i < differences.Last().Count - 1; i++)
            {
                var difference = differences.Last()[i + 1] - differences.Last()[i];
                tempNumbers.Add(difference);
            }
            
            differences.Add(tempNumbers);
        }

        for (var i = differences.Count - 1; i > 0; i--)
        {
            differences[i - 1].Add(differences[i - 1].Last() + differences[i].Last());
            differences[i - 1].Insert(0, differences[i - 1].First() - differences[i].First());
        }

        partOne += differences[0].Last();
        partTwo += differences[0].First();
    }

    return (Part1: partOne, Part2: partTwo);
}