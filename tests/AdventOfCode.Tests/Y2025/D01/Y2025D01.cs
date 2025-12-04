using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D01;

[ChallengeName("Secret Entrance")]
public class Y2025D01
{
    private readonly string _input = File.ReadAllText(@"Y2025\D01\Y2025D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Dial(Parse1(_input)).Count(x => x == 0);

        output.Should().Be(1026);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Dial(Parse2(_input)).Count(x => x == 0);

        output.Should().Be(5923);
    }

    // Parse Part 1: each line is a single rotation like "R12" or "L3"
    private static IEnumerable<int> Parse1(string input)
    {
        foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var dir = line[0];
            var amount = int.Parse(line.Substring(1));

            yield return dir == 'R' ? amount : -amount;
        }
    }

    // Parse Part 2: expand each rotation into individual +1 or -1 steps
    private static IEnumerable<int> Parse2(string input)
    {
        foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var dir = line[0];
            var amount = int.Parse(line.Substring(1));

            var step = dir == 'R' ? 1 : -1;

            for (var i = 0; i < amount; i++)
                yield return step;
        }
    }

    // Apply rotations to the dial and return each resulting position
    private static IEnumerable<int> Dial(IEnumerable<int> rotations)
    {
        var pos = 50;

        foreach (var r in rotations)
        {
            pos = (pos + r) % 100;
            if (pos < 0) pos += 100;

            yield return pos;
        }
    }
}