namespace AdventOfCode.Tests.Y2023.D01;

public static class Y2023D01Trebuchet
{
    private static readonly Dictionary<string, string> Replacer = new Dictionary<string, string>
    {
        { "0", "zero" },
        { "1", "one" },
        { "2", "two" },
        { "3", "three" },
        { "4", "four" },
        { "5", "five" },
        { "6", "six" },
        { "7", "seven" },
        { "8", "eight" },
        { "9", "nine" }
    };

    public static int PartOne(string input)
    {
        var digits = new string(input.Where(char.IsDigit).ToArray());
        return int.Parse($"{digits[0]}{digits[^1]}");
    }

    public static int PartTwo(string input)
    {
        var edited = Replacer.Aggregate(input, (current, pair) => current.Replace(pair.Value, $"{pair.Value}{pair.Key}{pair.Value}"));

        return PartOne(edited);
    }
}