using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D01;

public class Y2015D01NotQuiteLispTests
{
    [Theory]
    [InlineData("(())", 0)]
    [InlineData("()()", 0)]
    [InlineData("(((", 3)]
    [InlineData("(()(()(", 3)]
    [InlineData("))(((((", 3)]
    [InlineData("())", -1)]
    [InlineData("))(", -1)]
    [InlineData(")))", -3)]
    [InlineData(")())())", -3)]
    public void TestPartOne(string input, int expected)
    {
        var output = Y2015D01NotQuiteLisp.PartOne(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("(())", 0)]
    [InlineData("()()", 0)]
    public void TestPartTwo(string input, int expected)
    {
        var output = Y2015D01NotQuiteLisp.PartTwo(input);

        output.Should().Be(expected);
    }

    [Fact]
    public async Task SolvePartOne()
    {
        var input = await File.ReadAllTextAsync(@"Y2015\D01\Y2015D01NotQuiteLisp-input.txt", Encoding.UTF8);
        var output = Y2015D01NotQuiteLisp.PartOne(input);

        output.Should().Be(74);
    }

    [Fact]
    public async Task SolvePartTwo()
    {
        var input = await File.ReadAllTextAsync(@"Y2015\D01\Y2015D01NotQuiteLisp-input.txt", Encoding.UTF8);
        var output = Y2015D01NotQuiteLisp.PartTwo(input);

        output.Should().Be(1795);
    }
}