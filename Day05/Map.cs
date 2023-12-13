namespace Day05;

public class Map
{
    public string Label { get; set; }
    public List<Set> Sets { get; set; }

    public Map(string data)
    {
        var lines = data.Split("\r\n");
        if (lines.Length <= 0)
            throw new ArgumentException("The data is dirty.");

        Label = lines[0];
        Sets = lines[1..]
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => new Set(line))
            .ToList();
    }

    public long Transform(long source) =>
        Sets.FirstOrDefault(set => set.IsInRange(source))?.Transform(source) ?? source;
}