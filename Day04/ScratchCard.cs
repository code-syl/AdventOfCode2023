namespace Day04;

public class ScratchCard
{
    public required int Id { get; set; }
    public List<int> WinningNumbers { get; set; } = new List<int>();
    public List<int> SelectedNumbers { get; set; } = new List<int>();
    public int Copies { get; set; } = 1;
}