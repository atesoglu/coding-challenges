using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2032.D02;

[ChallengeName("Y2032D02ChallengeName")]
public class Y2032D02
{
    private readonly string _input = File.ReadAllText(@"Y2032\D02\Y2032D02-input.txt", Encoding.UTF8);

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