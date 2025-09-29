using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Tests.Y2023.D13;

[ChallengeName("Point of Incidence")]
public class Y2023D13
{
    private readonly string _input = File.ReadAllText(@"Y2023\D13\Y2023D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, 0);

        output.Should().Be(27202);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, 1);

        output.Should().Be(41566);
    }


    Complex Right = 1;
    Complex Down = Complex.ImaginaryOne;
    Complex Ortho(Complex dir) => dir == Right ? Down : Right;

    double Solve(string input, int allowedSmudges) => (
        from block in input.Split("\n\n")
        let map = ParseMap(block)
        select GetScore(map, allowedSmudges)
    ).Sum();

    // place a mirror along the edges of the map, find the one with the allowed smudges 
    double GetScore(Map map, int allowedSmudges) => (
        from dir in new Complex[] { Right, Down }
        from mirror in Positions(map, dir, dir)
        where FindSmudges(map, mirror, dir) == allowedSmudges
        select mirror.Real + 100 * mirror.Imaginary
    ).First();

    // cast a ray from each postion along the mirror and count the smudges
    int FindSmudges(Map map, Complex mirror, Complex rayDir) => (
        from ray0 in Positions(map, mirror, Ortho(rayDir))
        let rayA = Positions(map, ray0, rayDir)
        let rayB = Positions(map, ray0 - rayDir, -rayDir)
        select Enumerable.Zip(rayA, rayB).Count(p => map[p.First] != map[p.Second])
    ).Sum();

    // allowed positions of the map from 'start' going in 'dir'
    IEnumerable<Complex> Positions(Map map, Complex start, Complex dir)
    {
        for (var pos = start; map.ContainsKey(pos); pos += dir)
        {
            yield return pos;
        }
    }

    Map ParseMap(string input)
    {
        var rows = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, rows.Length)
            from icol in Enumerable.Range(0, rows[0].Length)
            let pos = new Complex(icol, irow)
            let cell = rows[irow][icol]
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToDictionary();
    }
}