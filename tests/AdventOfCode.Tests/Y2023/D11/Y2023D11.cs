using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2023.D11;

[ChallengeName("Cosmic Expansion")]
public class Y2023D11
{
    private readonly string _input = File.ReadAllText(@"Y2023\D11\Y2023D11-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, 1);

        output.Should().Be(9769724);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, 999999);

        output.Should().Be(603020563700);
    }


    private long Solve(string input, int expansion)
    {
        var map = input.Split("\n");

        var isRowEmpty = EmptyRows(map).ToHashSet().Contains;
        var isColEmpty = EmptyCols(map).ToHashSet().Contains;

        var galaxies = FindAll(map, '#');
        return (
            from g1 in galaxies
            from g2 in galaxies
            select
                Distance(g1.irow, g2.irow, expansion, isRowEmpty) +
                Distance(g1.icol, g2.icol, expansion, isColEmpty)
        ).Sum() / 2;
    }

    private static long Distance(int i1, int i2, int expansion, Func<int, bool> isEmpty)
    {
        var a = Math.Min(i1, i2);
        var d = Math.Abs(i1 - i2);
        return d + expansion * Enumerable.Range(a, d).Count(isEmpty);
    }

    private static IEnumerable<int> EmptyRows(string[] map) =>
        from irow in Enumerable.Range(0, map.Length)
        where map[irow].All(ch => ch == '.')
        select irow;

    private static IEnumerable<int> EmptyCols(string[] map) =>
        from icol in Enumerable.Range(0, map[0].Length)
        where map.All(row => row[icol] == '.')
        select icol;

    private static IEnumerable<Position> FindAll(string[] map, char ch) =>
        from irow in Enumerable.Range(0, map.Length)
        from icol in Enumerable.Range(0, map[0].Length)
        where map[irow][icol] == ch
        select new Position(irow, icol);
}

internal record Position(int irow, int icol);