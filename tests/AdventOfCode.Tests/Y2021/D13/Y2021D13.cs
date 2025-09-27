using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D13;

[ChallengeName("Transparent Origami")]
public class Y2021D13
{
    private readonly string _input = File.ReadAllText(@"Y2021\D13\Y2021D13-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => GetFolds(input).First().Count();
    private object PartTwo(string input) => ToString(GetFolds(input).Last()).Ocr();

    IEnumerable<HashSet<Point>> GetFolds(string input)
    {
        var blocks = input.Split("\n\n");
        // parse points into a hashset
        var points = (
            from line in blocks[0].Split("\n")
            let coords = line.Split(",")
            select new Point(int.Parse(coords[0]), int.Parse(coords[1]))
        ).ToHashSet();

        // fold line by line, yielding a new hashset
        foreach (var line in blocks[1].Split("\n"))
        {
            var rule = line.Split("=");
            if (rule[0].EndsWith("x"))
            {
                points = FoldX(int.Parse(rule[1]), points);
            }
            else
            {
                points = FoldY(int.Parse(rule[1]), points);
            }

            yield return points;
        }
    }

    string ToString(HashSet<Point> d)
    {
        var res = "";
        var height = d.MaxBy(p => p.y).y;
        var width = d.MaxBy(p => p.x).x;
        for (var y = 0; y <= height; y++)
        {
            for (var x = 0; x <= width; x++)
            {
                res += d.Contains(new Point(x, y)) ? '#' : ' ';
            }

            res += "\n";
        }

        return res;
    }

    HashSet<Point> FoldX(int x, HashSet<Point> d) =>
        d.Select(p => p.x > x ? p with { x = 2 * x - p.x } : p).ToHashSet();

    HashSet<Point> FoldY(int y, HashSet<Point> d) =>
        d.Select(p => p.y > y ? p with { y = 2 * y - p.y } : p).ToHashSet();
}