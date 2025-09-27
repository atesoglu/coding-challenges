using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2018.D06;

[ChallengeName("Chronal Coordinates")]
public class Y2018D06
{
    private readonly string _input = File.ReadAllText(@"Y2018\D06\Y2018D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input)
    {
        var coords = Parse(input);

        var minX = coords.Min(coord => coord.x) - 1;
        var maxX = coords.Max(coord => coord.x) + 1;
        var minY = coords.Min(coord => coord.y) - 1;
        var maxY = coords.Max(coord => coord.y) + 1;

        var area = new int[coords.Length];

        foreach (var x in Enumerable.Range(minX, maxX - minX + 1))
        {
            foreach (var y in Enumerable.Range(minY, maxY - minX + 1))
            {
                var d = coords.Select(coord => Dist((x, y), coord)).Min();
                var closest = Enumerable.Range(0, coords.Length).Where(i => Dist((x, y), coords[i]) == d).ToArray();

                if (closest.Length != 1)
                {
                    continue;
                }

                if (x == minX || x == maxX || y == minY || y == maxY)
                {
                    foreach (var icoord in closest)
                    {
                        if (area[icoord] != -1)
                        {
                            area[icoord] = -1;
                        }
                    }
                }
                else
                {
                    foreach (var icoord in closest)
                    {
                        if (area[icoord] != -1)
                        {
                            area[icoord]++;
                        }
                    }
                }
            }
        }

        return area.Max();
    }

    private object PartTwo(string input)
    {
        var coords = Parse(input);

        var minX = coords.Min(coord => coord.x) - 1;
        var maxX = coords.Max(coord => coord.x) + 1;
        var minY = coords.Min(coord => coord.y) - 1;
        var maxY = coords.Max(coord => coord.y) + 1;

        var area = 0;

        foreach (var x in Enumerable.Range(minX, maxX - minX + 1))
        {
            foreach (var y in Enumerable.Range(minY, maxY - minX + 1))
            {
                var d = coords.Select(coord => Dist((x, y), coord)).Sum();
                if (d < 10000)
                    area++;
            }
        }

        return area;
    }

    int Dist((int x, int y) c1, (int x, int y) c2)
    {
        return Math.Abs(c1.x - c2.x) + Math.Abs(c1.y - c2.y);
    }

    (int x, int y)[] Parse(string input) => (
        from line in input.Split("\n")
        let coords = line.Split(", ").Select(int.Parse).ToArray()
        select (coords[0], coords[1])
    ).ToArray();
}