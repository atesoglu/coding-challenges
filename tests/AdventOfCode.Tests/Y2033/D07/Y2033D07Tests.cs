using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2033.D07;

public class Y2033D07Tests
{
    private readonly Y2033D07 _challenge = new Y2033D07();
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2033\D07\Y2033D07-input.txt", Encoding.UTF8);

    [Theory]
    [InlineData("turn on 0,0 through 999,999", 1000000)]
    [InlineData("toggle 0,0 through 999,0", 1000)]
    [InlineData("turn off 499,499 through 500,500", 0)]
    public void PartOneWithSampleData(string input, int expected)
    {
        var output = _challenge.PartOne(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("turn on 0,0 through 0,0", 1)]
    [InlineData("toggle 0,0 through 999,999", 2000000)]
    public void PartTwoWithSampleData(string input, int expected)
    {
        var output = _challenge.PartTwo(input);

        output.Should().Be(expected);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var output = _lines.Sum(line => _challenge.PartOne(line));

        output.Should().Be(377891);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var output = _lines.Sum(line => _challenge.PartTwo(line));

        output.Should().Be(14110788);
    }
}
