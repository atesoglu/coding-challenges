using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D05;

[ChallengeName("Hydrothermal Venture")]
public class Y2021D05
{
    private readonly string _input = File.ReadAllText(@"Y2021\D05\Y2021D05-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetIntersections(ParseLines(_input, skipDiagonals: true)).Count();

        output.Should().Be(6007);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetIntersections(ParseLines(_input, skipDiagonals: false)).Count();

        output.Should().Be(19349);
    }


    private IEnumerable<Vec2> GetIntersections(IEnumerable<IEnumerable<Vec2>> lines) =>
        // group all the points and return the intersections:
        lines.SelectMany(pt => pt).GroupBy(pt => pt).Where(g => g.Count() > 1).Select(g => g.Key);

    private IEnumerable<IEnumerable<Vec2>> ParseLines(string input, bool skipDiagonals) =>
        from line in input.Split("\n")
        // parse out numbers first:
        let ns = (
            from st in line.Split(", ->".ToArray(), StringSplitOptions.RemoveEmptyEntries)
            select int.Parse(st)
        ).ToArray()

        // line properties:
        let start = new Vec2(ns[0], ns[1])
        let end = new Vec2(ns[2], ns[3])
        let displacement = new Vec2(end.x - start.x, end.y - start.y)
        let length = 1 + Math.Max(Math.Abs(displacement.x), Math.Abs(displacement.y))
        let dir = new Vec2(Math.Sign(displacement.x), Math.Sign(displacement.y))

        // represent lines with a set of points:
        let points =
            from i in Enumerable.Range(0, length)
            select new Vec2(start.x + i * dir.x, start.y + i * dir.y)

        // skip diagonals in part 1:
        where !skipDiagonals || dir.x == 0 || dir.y == 0
        select points;
}

internal record Vec2(int x, int y);
