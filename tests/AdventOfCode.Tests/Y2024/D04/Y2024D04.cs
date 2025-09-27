using System.Collections.Immutable;
using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Tests.Y2024.D04;

[ChallengeName("Ceres Search")]
public class Y2024D04
{
    private readonly string _input = File.ReadAllText(@"Y2024\D04\Y2024D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var mat = GetMap(_input);

        var output = (
            from pt in mat.Keys
            from dir in new[] { Right, Right + Down, Down + Left, Down }
            where Matches(mat, pt, dir, "XMAS")
            select 1
        ).Count();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var mat = GetMap(_input);

        var output = (
            from pt in mat.Keys
            where
                Matches(mat, pt + Up + Left, Down + Right, "MAS") &&
                Matches(mat, pt + Down + Left, Up + Right, "MAS")
            select 1
        ).Count();

        output.Should().Be(0);
    }


    Complex Up = -Complex.ImaginaryOne;
    Complex Down = Complex.ImaginaryOne;
    Complex Left = -1;
    Complex Right = 1;


    // check if the pattern (or its reverse) can be read in the given direction 
    // starting from pt
    bool Matches(Map map, Complex pt, Complex dir, string pattern)
    {
        var chars = Enumerable.Range(0, pattern.Length)
            .Select(i => map.GetValueOrDefault(pt + i * dir))
            .ToArray();
        return
            Enumerable.SequenceEqual(chars, pattern) ||
            Enumerable.SequenceEqual(chars, pattern.Reverse());
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    Map GetMap(string input)
    {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, map[y][x])
        ).ToImmutableDictionary();
    }
}