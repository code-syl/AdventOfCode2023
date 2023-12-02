using Day02;

await Part1();

static async Task Part1()
{
    // parse input
    var lines = await File.ReadAllLinesAsync("input.txt");
    var games = lines.Select(GameFactory.CreateGame);
    
    // which games are possible with only 12 red, 13 green, and 14 blue cubes
    var possibleGames = 
        games.Where(game => game.Sets.All(set => set is { Red: <= 12, Green: <= 13, Blue: <= 14 }));
    var sumOfGameIds = possibleGames.Sum(game => game.Id);
    
    Console.WriteLine(sumOfGameIds);
}