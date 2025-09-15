using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D02;

public class Y2015D02IWasToldThereWouldBeNoMathTests
{
    [Theory]
    [InlineData(2, 3, 4, 58)]
    [InlineData(1, 1, 10, 43)]
    public void TestPartOne(int length, int width, int height, int expected)
    {
        var output = Y2015D02IWasToldThereWouldBeNoMath.PartOne(length, width, height);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData(2, 3, 4, 34)]
    [InlineData(1, 1, 10, 14)]
    public void TestPartTwo(int length, int width, int height, int expected)
    {
        var output = Y2015D02IWasToldThereWouldBeNoMath.PartTwo(length, width, height);

        output.Should().Be(expected);
    }

    [Fact]
    public async Task SolvePartOne()
    {
        var lines = await File.ReadAllLinesAsync(@"Y2015\D02\Y2015D02IWasToldThereWouldBeNoMath-input.txt", Encoding.UTF8);

        var sum = 0;
        foreach (var line in lines)
        {
            var split = line.Split('x');
            sum += Y2015D02IWasToldThereWouldBeNoMath.PartOne(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        sum.Should().Be(1588178);
    }

    [Fact]
    public async Task SolvePartTwo()
    {
        var lines = await File.ReadAllLinesAsync(@"Y2015\D02\Y2015D02IWasToldThereWouldBeNoMath-input.txt", Encoding.UTF8);

        var sum = 0;
        foreach (var line in lines)
        {
            var split = line.Split('x');
            sum += Y2015D02IWasToldThereWouldBeNoMath.PartTwo(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        sum.Should().Be(3783758);
    }
}