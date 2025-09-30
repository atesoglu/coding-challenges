using System.Numerics;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D21;

[ChallengeName("Step Counter")]
public class Y2023D21
{
    private readonly string _input = File.ReadAllText(@"Y2023\D21\Y2023D21-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Steps(ParseMap(_input)).ElementAt(64);

        output.Should().Be(3682);
    }

    [Fact]
    public void PartTwo()
    {
        // Exploiting some nice properties of the input it reduces to quadratic
        // interpolation over 3 points: k * 131 + 65 for k = 0, 1, 2
        // I used the Newton method.
        var steps = Steps(ParseMap(_input)).Take(328).ToArray();

        (decimal x0, decimal y0) = (65, steps[65]);
        (decimal x1, decimal y1) = (196, steps[196]);
        (decimal x2, decimal y2) = (327, steps[327]);

        var y01 = (y1 - y0) / (x1 - x0);
        var y12 = (y2 - y1) / (x2 - x1);
        var y012 = (y12 - y01) / (x2 - x0);

        var n = 26501365;

        var output = decimal.Round(y0 + y01 * (n - x0) + y012 * (n - x0) * (n - x1));

        output.Should().Be(609012263058042);
    }

    // walks around and returns the number of available positions at each step
    private IEnumerable<long> Steps(HashSet<Complex> map)
    {
        var positions = new HashSet<Complex> { new Complex(65, 65) };
        while (true)
        {
            yield return positions.Count;
            positions = Step(map, positions);
        }
    }

    private HashSet<Complex> Step(HashSet<Complex> map, HashSet<Complex> positions)
    {
        Complex[] dirs = [1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne];

        var res = new HashSet<Complex>();
        foreach (var pos in positions)
        {
            foreach (var dir in dirs)
            {
                var posT = pos + dir;
                var tileCol = Mod(posT.Real, 131);
                var tileRow = Mod(posT.Imaginary, 131);
                if (map.Contains(new Complex(tileCol, tileRow)))
                {
                    res.Add(posT);
                }
            }
        }

        return res;
    }

    // the double % takes care of negative numbers
    private static double Mod(double n, int m) => ((n % m) + m) % m;

    private static HashSet<Complex> ParseMap(string input)
    {
        var lines = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            where lines[irow][icol] != '#'
            select new Complex(icol, irow)
        ).ToHashSet();
    }
}