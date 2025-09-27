using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2028.D11;

[ChallengeName("Y2028D11ChallengeName")]
public class Y2028D11
{
    private readonly string _input = File.ReadAllText(@"Y2028\D11\Y2028D11-input.txt", Encoding.UTF8);

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