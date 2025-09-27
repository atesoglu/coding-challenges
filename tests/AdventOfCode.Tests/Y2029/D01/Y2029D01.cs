using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2029.D01;

[ChallengeName("Y2029D01ChallengeName")]
public class Y2029D01
{
    private readonly string _input = File.ReadAllText(@"Y2029\D01\Y2029D01-input.txt", Encoding.UTF8);

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