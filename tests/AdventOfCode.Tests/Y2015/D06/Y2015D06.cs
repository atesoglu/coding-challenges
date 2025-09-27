using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D06;

[ChallengeName("Probably a Fire Hazard")]
public partial class Y2015D06
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D06\Y2015D06-input.txt", Encoding.UTF8);
    private static readonly Regex Regex = ActionRegex();

    [Fact]
    public void PartOne()
    {
        var output = Run(_lines, turnOn: _ => 1, turnOff: _ => 0, toggle: v => 1 - v);

        output.Should().Be(377891);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Run(_lines, turnOn: v => v + 1, turnOff: v => Math.Max(0, v - 1), toggle: v => v + 2);

        output.Should().Be(14110788);
    }

    private static int Run(IEnumerable<string> lines, Func<int, int> turnOn, Func<int, int> turnOff, Func<int, int> toggle)
    {
        var grid = new int[1000 * 1000];

        foreach (var line in lines)
        {
            var match = Regex.Match(line);
            if (!match.Success)
            {
                throw new Exception($"Invalid line: {line}");
            }

            var action = match.Groups[1].Value;
            var x1 = int.Parse(match.Groups[2].Value);
            var y1 = int.Parse(match.Groups[3].Value);
            var x2 = int.Parse(match.Groups[4].Value);
            var y2 = int.Parse(match.Groups[5].Value);

            for (var row = x1; row <= x2; row++)
            {
                for (var col = y1; col <= y2; col++)
                {
                    grid[row * 1000 + col] = action switch
                    {
                        "turn on" => turnOn(grid[row * 1000 + col]),
                        "turn off" => turnOff(grid[row * 1000 + col]),
                        "toggle" => toggle(grid[row * 1000 + col]),
                        _ => throw new Exception($"Unknown action: {action}")
                    };
                }
            }
        }

        return grid.Sum();
    }

    [GeneratedRegex(@"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)")]
    private static partial Regex ActionRegex();
}