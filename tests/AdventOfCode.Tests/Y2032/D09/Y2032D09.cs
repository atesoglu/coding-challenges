using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2032.D09;

[ChallengeName("Y2032D09ChallengeName")]
public class Y2032D09
{
    private readonly string _input = File.ReadAllText(@"Y2032\D09\Y2032D09-input.txt", Encoding.UTF8);

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