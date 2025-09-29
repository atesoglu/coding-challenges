using System.Collections.Immutable;
using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Tests.Y2024.D04;

[ChallengeName("Ceres Search")]
public class Y2024D04
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D04\Y2024D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var grid = BuildGrid(_lines);

        var directions = new[] { Right, Right + Down, Down + Left, Down };
        var output = grid.Keys
            .SelectMany(point => directions, (point, dir) => (point, dir))
            .Count(pair => Matches(grid, pair.point, pair.dir, "XMAS"));

        output.Should().Be(2406);
    }

    [Fact]
    public void PartTwo()
    {
        var grid = BuildGrid(_lines);

        var output = grid.Keys.Count(pt =>
            Matches(grid, pt + Up + Left, Down + Right, "MAS") &&
            Matches(grid, pt + Down + Left, Up + Right, "MAS")
        );

        output.Should().Be(1807);
    }


    Complex Up = -Complex.ImaginaryOne;
    Complex Down = Complex.ImaginaryOne;
    Complex Left = -1;
    Complex Right = 1;

    bool Matches(Map map, Complex pt, Complex dir, string pattern)
    {
        var characters = Enumerable.Range(0, pattern.Length)
            .Select(i => map.GetValueOrDefault(pt + i * dir))
            .ToArray();
        return characters.SequenceEqual(pattern) || characters.SequenceEqual(pattern.Reverse());
    }

    Map BuildGrid(string[] lines)
    {
        return (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, lines[y][x])
        ).ToImmutableDictionary();
    }
}