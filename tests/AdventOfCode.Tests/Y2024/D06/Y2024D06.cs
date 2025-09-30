using System.Collections.Immutable;
using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Tests.Y2024.D06;

[ChallengeName("Guard Gallivant")]
public class Y2024D06
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D06\Y2024D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (map, start) = BuildMapAndStart(_lines);
        var output = Walk(map, start).positions.Count();

        output.Should().Be(4722);
    }

    [Fact]
    public void PartTwo()
    {
        var (map, start) = BuildMapAndStart(_lines);
        var output = Walk(map, start).positions
            .AsParallel()
            .Count(pos => Walk(map.SetItem(pos, '#'), start).isLoop);

        output.Should().Be(1602);
    }


    private Complex Up = Complex.ImaginaryOne;
    private Complex TurnRight = -Complex.ImaginaryOne;

    private (IEnumerable<Complex> positions, bool isLoop) Walk(Map map, Complex pos)
    {
        var seen = new HashSet<(Complex pos, Complex dir)>();
        var dir = Up;
        while (map.ContainsKey(pos) && !seen.Contains((pos, dir)))
        {
            seen.Add((pos, dir));
            if (map.GetValueOrDefault(pos + dir) == '#')
            {
                dir *= TurnRight;
            }
            else
            {
                pos += dir;
            }
        }

        return (
            positions: seen.Select(s => s.pos).Distinct(),
            isLoop: seen.Contains((pos, dir))
        );
    }

    private (Map map, Complex start) BuildMapAndStart(string[] lines)
    {
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(-Up * y + x, lines[y][x])
        ).ToImmutableDictionary();

        var start = map.First(x => x.Value == '^').Key;

        return (map, start);
    }
}