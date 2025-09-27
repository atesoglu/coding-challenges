using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2030.D01;

[ChallengeName("Y2030D01ChallengeName")]
public class Y2030D01
{
    private readonly string _input = File.ReadAllText(@"Y2030\D01\Y2030D01-input.txt", Encoding.UTF8);

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