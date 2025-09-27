using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2033.D20;

[ChallengeName("Y2033D20ChallengeName")]
public class Y2033D20
{
    private readonly string _input = File.ReadAllText(@"Y2033\D20\Y2033D20-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = 0;

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = 0;

        output.Should().Be(0);
    }
}