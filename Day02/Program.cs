using Day02;

var lines = await File.ReadAllLinesAsync("input.txt");
var games = lines.Select(GameFactory.CreateGame).ToList();

// Part 1: Which games are possible when there are only 12 red, 13 green, and 14 blue cubes, given input.txt as input.
//         Get the sum of the IDs of those games.
Part1(games);
// Part 2: For each game, find the minimum set of cubes needed to play that game (per color). 
//         Get the power of those minimum sets (multiply each cube amount per set).
//         Get the sum of the powers of these sets.
Part2(games);
return;

static void Part1(IEnumerable<Game> games)
{
    var sum = games.Where(game => 
        game.Sets.All(set => set is { Red: <= 12, Green: <= 13, Blue: <= 14 })
    ).Sum(game => game.Id);
    
    Console.WriteLine(sum);
}

static void Part2(IEnumerable<Game> games)
{
    // get minimum needed for each color per game (for example, 1 red, 4 green, 3 blue)
    // multiply those numbers with each other (for example 1*4*3=12)
    // get the sum of these multiplications (for example 12+...)
    var sum = games.Select(game =>
    (
        Red: game.Sets.Max(set => set.Red),
        Green: game.Sets.Max(set => set.Green),
        Blue: game.Sets.Max(set => set.Blue)
    )).Select(colors => colors.Red * colors.Green * colors.Blue).Sum();

    Console.WriteLine(sum);
} 