using System.Text.RegularExpressions;

namespace AdventOfCode.Tests.Y2035.D20;

[ChallengeName("TODO: Add Challenge Name")]
public partial class Y2035D20
{
    private static readonly Regex Command = CommandRegex();
    private readonly int[,] _grid = new int[1000, 1000];

    public int PartOne(string input)
    {
        return ProcessCommand(input, turnOn: _ => 1, turnOff: _ => 0, toggle: v => v == 0 ? 1 : 0);
    }

    public int PartTwo(string input)
    {
        // brightness levels
        return ProcessCommand(input, turnOn: v => v + 1, turnOff: v => v > 0 ? v - 1 : 0, toggle: v => v + 2);
    }

    private int ProcessCommand(string line, Func<int, int> turnOn, Func<int, int> turnOff, Func<int, int> toggle)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return 0;
        }

        var match = Command.Match(line);
        if (!match.Success)
        {
            throw new InvalidOperationException($"Bad command: {line}");
        }

        var action = match.Groups[1].Value;
        var x1 = int.Parse(match.Groups[2].Value);
        var y1 = int.Parse(match.Groups[3].Value);
        var x2 = int.Parse(match.Groups[4].Value);
        var y2 = int.Parse(match.Groups[5].Value);

        var op = action switch
        {
            "turn on" => turnOn,
            "turn off" => turnOff,
            "toggle" => toggle,
            _ => throw new InvalidOperationException()
        };

        var changedAmount = 0;
        for (var x = x1; x <= x2; x++)
        {
            for (var y = y1; y <= y2; y++)
            {
                var old = _grid[x, y];
                var updated = op(old);
                _grid[x, y] = updated;

                changedAmount += updated - old; // net brightness/on-count change
            }
        }

        return changedAmount;
    }

    [GeneratedRegex(@"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)", RegexOptions.Compiled)]
    private static partial Regex CommandRegex();
}
