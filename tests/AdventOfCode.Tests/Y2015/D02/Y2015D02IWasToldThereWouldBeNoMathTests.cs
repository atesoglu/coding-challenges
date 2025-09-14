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
    [InlineData(2, 3, 4, 58)]
    [InlineData(1, 1, 10, 43)]
    public void TestPartTwo(int length, int width, int height, int expected)
    {
        var output = Y2015D02IWasToldThereWouldBeNoMath.PartTwo(length, width, height);

        output.Should().Be(expected);
    }

    [Fact]
    public async Task SolvePartOne()
    {
        var input = await File.ReadAllTextAsync(@"Y2015\D01\Y2015D02IWasToldThereWouldBeNoMath-input.txt", Encoding.UTF8);
        int length = 0;
        int width = 0;
        int height = 0;
        int expected = 0;
        var output = Y2015D02IWasToldThereWouldBeNoMath.PartOne(length, width, height);

        output.Should().Be(74);
    }

    [Fact]
    public async Task SolvePartTwo()
    {
        var input = await File.ReadAllTextAsync(@"Y2015\D01\Y2015D02IWasToldThereWouldBeNoMath-input.txt", Encoding.UTF8);
        int length = 0;
        int width = 0;
        int height = 0;
        int expected = 0;
        var output = Y2015D02IWasToldThereWouldBeNoMath.PartTwo(length, width, height);

        output.Should().Be(1795);
    }
}