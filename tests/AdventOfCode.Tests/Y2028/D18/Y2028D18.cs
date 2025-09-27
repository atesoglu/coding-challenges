using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2028.D18;

[ChallengeName("Y2028D18ChallengeName")]
public class Y2028D18
{
    private readonly string _input = File.ReadAllText(@"Y2028\D18\Y2028D18-input.txt", Encoding.UTF8);

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