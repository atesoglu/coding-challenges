using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2017.D03;

[ChallengeName("Spiral Memory")]
public class Y2017D03
{
    private readonly string _input = File.ReadAllText(@"Y2017\D03\Y2017D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (x, y) = SpiralCoordinates().ElementAt(int.Parse(_input) - 1);

        var output = Math.Abs(x) + Math.Abs(y);

        output.Should().Be(326);
    }

    [Fact]
    public void PartTwo()
    {
        var num = int.Parse(_input);
        var output = SpiralSums().First(v => v > num);

        output.Should().Be(363010);
    }

    private IEnumerable<(int, int)> SpiralCoordinates()
    {
        var (x, y) = (0, 0);
        var (dx, dy) = (1, 0);

        for (var edgeLength = 1;; edgeLength++)
        {
            for (var run = 0; run < 2; run++)
            {
                for (var step = 0; step < edgeLength; step++)
                {
                    yield return (x, y);
                    (x, y) = (x + dx, y - dy);
                }

                (dx, dy) = (-dy, dx);
            }
        }
    }

    private IEnumerable<int> SpiralSums()
    {
        var mem = new Dictionary<(int, int), int>();
        mem[(0, 0)] = 1;

        foreach (var coord in SpiralCoordinates())
        {
            var sum = (from coordT in Window(coord) where mem.ContainsKey(coordT) select mem[coordT]).Sum();
            mem[coord] = sum;
            yield return sum;
        }
    }

    private IEnumerable<(int, int)> Window((int x, int y) coord) =>
        from dx in new[] { -1, 0, 1 }
        from dy in new[] { -1, 0, 1 }
        select (coord.x + dx, coord.y + dy);
}