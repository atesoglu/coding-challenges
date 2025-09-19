using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D03;

public class Y2015D03Tests
{
    private readonly Y2015D03 _challenge = new Y2015D03();
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D03\Y2015D03-input.txt", Encoding.UTF8);

    [Theory]
    [InlineData(2, 3, 4, 58)]
    [InlineData(1, 1, 10, 43)]
    public void PartOneWithSampleData(int length, int width, int height, int expected)
    {
        var output = _challenge.PartOne(length, width, height);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData(2, 3, 4, 34)]
    [InlineData(1, 1, 10, 14)]
    public void PartTwoWithSampleData(int length, int width, int height, int expected)
    {
        var output = _challenge.PartTwo(length, width, height);

        output.Should().Be(expected);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            var split = line.Split('x');
            output += _challenge.PartOne(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        output.Should().Be(1588178);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            var split = line.Split('x');
            output += _challenge.PartTwo(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        output.Should().Be(3783758);
    }
}