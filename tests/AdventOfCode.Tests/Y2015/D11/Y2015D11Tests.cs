using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D11;

public class Y2015D11Tests
{
    private readonly Y2015D11 _challenge = new Y2015D11();
    private readonly string _input = File.ReadAllText(@"Y2015\D11\Y2015D11-input.txt", Encoding.UTF8);

    [Theory]
    [InlineData("abcdef", 609043)]
    [InlineData("pqrstuv", 1048970)]
    public void PartOneWithSampleData(string input, int expected)
    {
        var output = _challenge.PartOne(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("abcdef", 6742839)]
    [InlineData("pqrstuv", 5714438)]
    public void PartTwoWithSampleData(string input, int expected)
    {
        var output = _challenge.PartTwo(input);

        output.Should().Be(expected);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var output = _challenge.PartOne(_input);

        output.Should().Be(346386);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var output = _challenge.PartTwo(_input);

        output.Should().Be(9958218);
    }
}