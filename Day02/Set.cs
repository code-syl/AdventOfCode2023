namespace Day02;

public class Set
{
    public int Blue { get; set; } = 0;
    public int Red { get; set; } = 0;
    public int Green { get; set; } = 0;
}

public static class SetFactory
{
    public static Set CreateSet(string input)
    {
        // example input: 7 blue, 6 green, 3 red
        var valueColorPairs = input.Split(", ").Select(vcp =>
        {
            var split = vcp.Split(" ");
            return (Value: Convert.ToInt32(split[0]), Color: split[1]);
        });

        var set = new Set();
        foreach (var vcp in valueColorPairs)
        {
            switch (vcp.Color)
            {
                case "blue": set.Blue = vcp.Value; break;
                case "green": set.Green = vcp.Value; break;
                case "red": set.Red = vcp.Value; break;
            }
        }

        return set;
    }
}