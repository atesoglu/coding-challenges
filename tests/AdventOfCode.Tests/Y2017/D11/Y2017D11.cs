using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D11;

[ChallengeName("Hex Ed")]
public class Y2017D11
{
    private readonly string _input = File.ReadAllText(@"Y2017\D11\Y2017D11-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Distances(_input).Last();

        output.Should().Be(715);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Distances(_input).Max();

        output.Should().Be(1512);
    }


    private IEnumerable<int> Distances(string input) => from w in Wander(input) select (Math.Abs(w.x) + Math.Abs(w.y) + Math.Abs(w.z)) / 2;

    private static IEnumerable<(int x, int y, int z)> Wander(string input)
    {
        var (x, y, z) = (0, 0, 0);
        foreach (var dir in input.Split(','))
        {
            switch (dir)
            {
                case "n": (x, y, z) = (x + 0, y + 1, z - 1); break;
                case "ne": (x, y, z) = (x + 1, y + 0, z - 1); break;
                case "se": (x, y, z) = (x + 1, y - 1, z + 0); break;
                case "s": (x, y, z) = (x + 0, y - 1, z + 1); break;
                case "sw": (x, y, z) = (x - 1, y + 0, z + 1); break;
                case "nw": (x, y, z) = (x - 1, y + 1, z + 0); break;
                default: throw new ArgumentException(dir);
            }

            yield return (x, y, z);
        }
    }
}