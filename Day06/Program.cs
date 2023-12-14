const string input = "input.txt";
using var streamReader = new StreamReader(input);

var data = (await streamReader.ReadToEndAsync()).Split("\r\n");
const StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
var raceTimes = data[0].Split(':')[1].Split(' ', splitOptions).Select(long.Parse).ToList();
var recordDistances = data[1].Split(':')[1].Split(' ', splitOptions).Select(long.Parse).ToList();
var bigRaceTime = long.Parse(string.Join("", raceTimes.Select(rt => rt)));
var bigRecordDistance = long.Parse(string.Join("", recordDistances.Select(rd => rd)));

Console.WriteLine($"Part 1: {Part1(raceTimes, recordDistances)}");
Console.WriteLine($"Part 2: {Part2(bigRaceTime, bigRecordDistance)}");
return;

static long Part1(IReadOnlyList<long> raceTimes, IReadOnlyList<long> recordDistances)
{
    // for each millisecond spent pushing the button, the boat gains a distance of that time in millimeters.
    var possibleWinningTimes = new List<long>();
    for (var race = 0; race < raceTimes.Count; race++)
    {
        var raceTime = raceTimes[race];
        var recordDistance = recordDistances[race];
        
        possibleWinningTimes.Add(GetWinningTimes(raceTime, recordDistance));
    }

    return possibleWinningTimes.Aggregate((value, next) => value * next);
}

static long Part2(long bigRaceTime, long bigRecordDistance)
{
    return GetWinningTimes(bigRaceTime, bigRecordDistance);
}

static long GetWinningTimes(long raceTime, long recordDistance)
{
    var winningTimes = 0;
    for (var ms = 0; ms < raceTime; ms++)
    {
        var timeLeft = raceTime - ms;
        var distance = timeLeft * ms;
        if (distance > recordDistance)
            winningTimes++;
    }

    return winningTimes;
}