using System.Collections.Immutable;
using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Tests.Y2024.D08;

[ChallengeName("Resonant Collinearity")]
public class Y2024D08
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D08\Y2024D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetUniquePositions(_lines, GetAntinodes1).Count();

        output.Should().Be(413);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetUniquePositions(_lines, GetAntinodes2).Count();

        output.Should().Be(1417);
    }


    HashSet<Complex> GetUniquePositions(IEnumerable<string> lines, GetAntinodes getAntinodes)
    {
        var map = BuildMap(lines);

        var antennaLocations = (
            from pos in map.Keys
            where char.IsAsciiLetterOrDigit(map[pos])
            select pos
        ).ToArray();

        return (
            from srcAntenna in antennaLocations
            from dstAntenna in antennaLocations
            where srcAntenna != dstAntenna && map[srcAntenna] == map[dstAntenna]
            from antinode in getAntinodes(srcAntenna, dstAntenna, map)
            select antinode
        ).ToHashSet();
    }

    delegate IEnumerable<Complex> GetAntinodes(Complex srcAntenna, Complex dstAntenna, Map map);

    IEnumerable<Complex> GetAntinodes1(Complex srcAntenna, Complex dstAntenna, Map map)
    {
        var dir = dstAntenna - srcAntenna;
        var antinote = dstAntenna + dir;
        if (map.Keys.Contains(antinote))
        {
            yield return antinote;
        }
    }

    IEnumerable<Complex> GetAntinodes2(Complex srcAntenna, Complex dstAntenna, Map map)
    {
        var dir = dstAntenna - srcAntenna;
        var antinote = dstAntenna;
        while (map.Keys.Contains(antinote))
        {
            yield return antinote;
            antinote += dir;
        }
    }

    Map BuildMap(IEnumerable<string> lines)
    {
        var rowArray = lines.ToArray();
        return (
            from y in Enumerable.Range(0, rowArray.Length)
            from x in Enumerable.Range(0, rowArray[0].Length)
            select new KeyValuePair<Complex, char>(x - y * Complex.ImaginaryOne, rowArray[y][x])
        ).ToImmutableDictionary();
    }
}