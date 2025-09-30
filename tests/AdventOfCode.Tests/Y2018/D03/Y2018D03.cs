using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D03;

[ChallengeName("No Matter How You Slice It")]
public class Y2018D03
{
    private readonly string _input = File.ReadAllText(@"Y2018\D03\Y2018D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Decorate(_input).overlapArea;

        output.Should().Be(113576);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Decorate(_input).intactId;

        output.Should().Be(825);
    }


    private (int overlapArea, int intactId) Decorate(string input)
    {
        // #1 @ 55,885: 22x10
        var rx = new Regex(@"(?<id>\d+) @ (?<x>\d+),(?<y>\d+): (?<width>\d+)x(?<height>\d+)");
        var mtx = new int[1000, 1000];

        var overlapArea = 0;

        var ids = new HashSet<int>();
        foreach (var line in input.Split("\n"))
        {
            var parts = rx.Match(line);
            var id = int.Parse(parts.Groups["id"].Value);
            var x = int.Parse(parts.Groups["x"].Value);
            var y = int.Parse(parts.Groups["y"].Value);
            var width = int.Parse(parts.Groups["width"].Value);
            var height = int.Parse(parts.Groups["height"].Value);

            ids.Add(id);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (mtx[x + i, y + j] == 0)
                    {
                        mtx[x + i, y + j] = id;
                    }
                    else if (mtx[x + i, y + j] == -1)
                    {
                        ids.Remove(id);
                    }
                    else
                    {
                        ids.Remove(mtx[x + i, y + j]);
                        ids.Remove(id);
                        overlapArea++;

                        mtx[x + i, y + j] = -1;
                    }
                }
            }
        }

        return (overlapArea, ids.Single());
    }
}