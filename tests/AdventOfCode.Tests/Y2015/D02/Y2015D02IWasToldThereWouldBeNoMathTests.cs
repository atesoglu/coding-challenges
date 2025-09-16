using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D02;

public class Y2015D02IWasToldThereWouldBeNoMathTests
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D02\Y2015D02IWasToldThereWouldBeNoMath-input.txt", Encoding.UTF8);

    [Theory]
    [InlineData(2, 3, 4, 58)]
    [InlineData(1, 1, 10, 43)]
    public void PartOneWithSampleData(int length, int width, int height, int expected)
    {
        var output = Y2015D02IWasToldThereWouldBeNoMath.PartOne(length, width, height);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData(2, 3, 4, 34)]
    [InlineData(1, 1, 10, 14)]
    public void PartTwoWithSampleData(int length, int width, int height, int expected)
    {
        var output = Y2015D02IWasToldThereWouldBeNoMath.PartTwo(length, width, height);

        output.Should().Be(expected);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var sum = 0;
        foreach (var line in _lines)
        {
            var split = line.Split('x');
            sum += Y2015D02IWasToldThereWouldBeNoMath.PartOne(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        sum.Should().Be(1588178);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var sum = 0;
        foreach (var line in _lines)
        {
            var split = line.Split('x');
            sum += Y2015D02IWasToldThereWouldBeNoMath.PartTwo(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        sum.Should().Be(3783758);
    }
}