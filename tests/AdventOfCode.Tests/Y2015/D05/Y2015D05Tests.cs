using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D05;

public class Y2015D05Tests
{
    private readonly Y2015D05 _challenge = new Y2015D05();
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D05\Y2015D05-input.txt", Encoding.UTF8);

    [Theory]
    [InlineData("ugknbfddgicrmopn", true)]
    [InlineData("aaa", true)]
    [InlineData("jchzalrnumimnmhp", false)]
    [InlineData("haegwjzuvuyypxyu", false)]
    [InlineData("dvszwmarrgswjxmb", false)]
    public void PartOneWithSampleData(string input, bool expected)
    {
        var output = _challenge.PartOne(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("qjhvhtzxzqqjkmpb", true)]
    [InlineData("xxyxx", true)]
    [InlineData("uurcxstgmygtbstg", false)]
    [InlineData("ieodomkazucvgmuy", false)]
    public void PartTwoWithSampleData(string input, bool expected)
    {
        var output = _challenge.PartTwo(input);

        output.Should().Be(expected);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            output += _challenge.PartOne(line) ? 1 : 0;
        }

        output.Should().Be(238);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            output += _challenge.PartTwo(line) ? 1 : 0;
        }

        output.Should().Be(69);
    }
}