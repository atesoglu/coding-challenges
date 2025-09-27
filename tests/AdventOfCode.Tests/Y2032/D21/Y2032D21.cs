using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2032.D21;

[ChallengeName("Y2032D21ChallengeName")]
public class Y2032D21
{
    private readonly string _input = File.ReadAllText(@"Y2032\D21\Y2032D21-input.txt", Encoding.UTF8);

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