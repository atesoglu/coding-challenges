using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D02;

[ChallengeName("I Was Told There Would Be No Math")]
public class Y2015D02
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D02\Y2015D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            var split = line.Split('x');
            var length = int.Parse(split[0]);
            var width = int.Parse(split[1]);
            var height = int.Parse(split[2]);

            var surface = 2 * length * width + 2 * width * height + 2 * height * length;

            // Calculate the area of the smallest side of the box, which is used as extra slack paper.
            // There are three sides: length*width, width*height, and height*length.
            // The smallest of these three areas is the extra paper needed.
            var slack = Math.Min(length * width, Math.Min(width * height, height * length));

            output += surface + slack;
        }

        output.Should().Be(1588178);
    }

    [Fact]
    public void PartTwo()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            var split = line.Split('x');
            var length = int.Parse(split[0]);
            var width = int.Parse(split[1]);
            var height = int.Parse(split[2]);

            var dimensions = new[] { length, width, height };
            Array.Sort(dimensions);

            var smallestPerimeter = dimensions[0] * 2 + dimensions[1] * 2;
            var bowLength = dimensions[0] * dimensions[1] * dimensions[2];

            output += smallestPerimeter + bowLength;
        }

        output.Should().Be(3783758);
    }
}