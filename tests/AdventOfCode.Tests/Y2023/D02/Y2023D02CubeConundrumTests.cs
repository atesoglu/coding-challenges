using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D02;

public class Y2023D02CubeConundrumTests
{
    [Theory]
    [InlineData("1abc2", 12)]
    [InlineData("pqr3stu8vwx", 38)]
    [InlineData("a1b2c3d4e5f", 15)]
    [InlineData("treb7uchet", 77)]
    public void TestPartOne(string input, int expected)
    {
        var output = Y2023D02CubeConundrum.PartOne(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("two1nine", 29)]
    [InlineData("eightwothree", 83)]
    [InlineData("abcone2threexyz", 13)]
    [InlineData("xtwone3four", 24)]
    [InlineData("4nineeightseven2", 42)]
    [InlineData("zoneight234", 14)]
    [InlineData("7pqrstsixteen", 76)]
    public void TestPartTwo(string input, int expected)
    {
        var output = Y2023D02CubeConundrum.PartTwo(input);

        output.Should().Be(expected);
    }

    [Fact]
    public async Task SolvePartOne()
    {
        var lines = await File.ReadAllLinesAsync(@"Y2023\D02\Y2023D02CubeConundrum-input.txt", Encoding.UTF8);

        var sum = lines.Sum(Y2023D02CubeConundrum.PartOne);

        sum.Should().Be(54667);
    }

    [Fact]
    public async Task SolvePartTwo()
    {
        var lines = await File.ReadAllLinesAsync(@"Y2023\D02\Y2023D02CubeConundrum-input.txt", Encoding.UTF8);

        var sum = lines.Sum(Y2023D02CubeConundrum.PartTwo);

        sum.Should().Be(54203);
    }
}