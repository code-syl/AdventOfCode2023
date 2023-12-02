namespace Day02;

public class Game
{
    public required int Id { get; set; }
    public List<Set> Sets { get; set; } = new();
}

public static class GameFactory
{
    public static Game CreateGame(string input)
    {
        var id = Convert.ToInt32(string.Join("", input[5..].TakeWhile(c => c != ':')));
        var sets = input.Split(": ")[1].Split("; ").Select(SetFactory.CreateSet).ToList();
        
        return new Game
        {
            Id = id, 
            Sets = sets
        };
    }
}