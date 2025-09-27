using System.Collections.Immutable;
using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Tests.Y2024.D08;

[ChallengeName("Resonant Collinearity")]
public class Y2024D08
{
    private readonly string _input = File.ReadAllText(@"Y2024\D08\Y2024D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetUniquePositions(_input, GetAntinodes1).Count();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetUniquePositions(_input, GetAntinodes2).Count();

        output.Should().Be(0);
    }


    HashSet<Complex> GetUniquePositions(string input, GetAntinodes getAntinodes)
    {
        var map = GetMap(input);

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

    // returns the antinode positions of srcAntenna on the dstAntenna side
    delegate IEnumerable<Complex> GetAntinodes(Complex srcAntenna, Complex dstAntenna, Map map);

    // in part 1 we just look at the immediate neighbour
    IEnumerable<Complex> GetAntinodes1(Complex srcAntenna, Complex dstAntenna, Map map)
    {
        var dir = dstAntenna - srcAntenna;
        var antinote = dstAntenna + dir;
        if (map.Keys.Contains(antinote))
        {
            yield return antinote;
        }
    }

    // in part 2 this becomes a cycle, plus dstAntenna is also a valid position now
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

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    Map GetMap(string input)
    {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(x - y * Complex.ImaginaryOne, map[y][x])
        ).ToImmutableDictionary();
    }
}