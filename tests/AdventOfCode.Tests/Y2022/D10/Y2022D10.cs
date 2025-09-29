using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D10;

[ChallengeName("Cathode-Ray Tube")]
public class Y2022D10
{
    private readonly string _input = File.ReadAllText(@"Y2022\D10\Y2022D10-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var sample = new[] { 20, 60, 100, 140, 180, 220 };
        var output = Signal(_input)
            .Where(signal => sample.Contains(signal.cycle))
            .Select(signal => signal.x * signal.cycle)
            .Sum();

        output.Should().Be(14320);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Signal(_input)
            .Select(signal =>
            {
                var spriteMiddle = signal.x;
                var screenColumn = (signal.cycle - 1) % 40;
                return Math.Abs(spriteMiddle - screenColumn) < 2 ? '#' : ' ';
            })
            .Chunk(40)
            .Select(line => new string(line))
            .Aggregate("", (screen, line) => screen + line + "\n")
            .Ocr().ToString();

        output.Should().Be("PCPBKAPJ");
    }

    IEnumerable<(int cycle, int x)> Signal(string input)
    {
        var (cycle, x) = (1, 1);
        foreach (var line in input.Split("\n"))
        {
            var parts = line.Split(" ");
            switch (parts[0])
            {
                case "noop":
                    yield return (cycle++, x);
                    break;
                case "addx":
                    yield return (cycle++, x);
                    yield return (cycle++, x);
                    x += int.Parse(parts[1]);
                    break;
                default:
                    throw new ArgumentException(parts[0]);
            }
        }
    }
}