const string input = "input.txt";
using var streamReader = new StreamReader(input);

var data = (await streamReader.ReadToEndAsync()).Split("\r\n");
const StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
var raceTimes = data[0].Split(':')[1].Split(' ', splitOptions).Select(int.Parse).ToList();
var recordDistances = data[1].Split(':')[1].Split(' ', splitOptions).Select(int.Parse).ToList();

Console.WriteLine($"Part 1: {Part1(raceTimes, recordDistances)}");
return;

static int Part1(IReadOnlyList<int> raceTimes, IReadOnlyList<int> recordDistances)
{
    // for each millisecond spent pushing the button, the boat gains a distance of that time in millimeters.
    var possibleWinningTimes = new List<int>();
    for (var race = 0; race < raceTimes.Count; race++)
    {
        var numberOfWinningTimesForThisRace = 0;
        var raceTime = raceTimes[race];
        var recordDistance = recordDistances[race];

        for (var ms = 0; ms <= raceTime; ms++)
        {
            var timeLeft = raceTime - ms;
            var distance = timeLeft * ms;
            if (distance > recordDistance)
                numberOfWinningTimesForThisRace++;
        }
        
        possibleWinningTimes.Add(numberOfWinningTimesForThisRace);
    }

    return possibleWinningTimes.Aggregate((value, next) => value * next);
}