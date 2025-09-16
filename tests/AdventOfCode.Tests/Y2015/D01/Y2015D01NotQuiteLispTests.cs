using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D01;

public class Y2015D01NotQuiteLispTests
{
    private readonly string _input = File.ReadAllText(@"Y2015\D01\Y2015D01NotQuiteLisp-input.txt", Encoding.UTF8);

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
    public void PartOneWithSampleData(string input, int expected)
    {
        var output = Y2015D01NotQuiteLisp.PartOne(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("(())", 0)]
    [InlineData("()()", 0)]
    public void PartTwoWithSampleData(string input, int expected)
    {
        var output = Y2015D01NotQuiteLisp.PartTwo(input);

        output.Should().Be(expected);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var output = Y2015D01NotQuiteLisp.PartOne(_input);

        output.Should().Be(74);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var output = Y2015D01NotQuiteLisp.PartTwo(_input);

        output.Should().Be(1795);
    }
}