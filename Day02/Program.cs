using Day02;

// parse input
var lines = await File.ReadAllLinesAsync("input.txt");
var games = lines.Select(GameFactory.CreateGame).ToList();

// solve
Part1(games);
Part2(games);
return;

static void Part1(IEnumerable<Game> games)
{
    // which games are possible with only 12 red, 13 green, and 14 blue cubes
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