using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2028.D23;

[ChallengeName("Y2028D23ChallengeName")]
public class Y2028D23
{
    private readonly string _input = File.ReadAllText(@"Y2028\D23\Y2028D23-input.txt", Encoding.UTF8);

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