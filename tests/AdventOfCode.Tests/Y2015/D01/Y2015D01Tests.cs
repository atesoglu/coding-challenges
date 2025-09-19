using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D01;

public class Y2015D01Tests
{
    private readonly Y2015D01 _solution = new Y2015D01();
    private readonly string _input = File.ReadAllText(@"Y2015\D01\Y2015D01-input.txt", Encoding.UTF8);

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
        var output = _solution.PartOne(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("(())", 0)]
    [InlineData("()()", 0)]
    public void PartTwoWithSampleData(string input, int expected)
    {
        var output = _solution.PartTwo(input);

        output.Should().Be(expected);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var output = _solution.PartOne(_input);

        output.Should().Be(74);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var output = _solution.PartTwo(_input);

        output.Should().Be(1795);
    }
}