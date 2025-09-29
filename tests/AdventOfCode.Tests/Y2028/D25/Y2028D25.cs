using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2028.D25;

[ChallengeName("Y2028D25ChallengeName")]
public class Y2028D25
{
    private readonly string _input = File.ReadAllText(@"Y2028\D25\Y2028D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = 0;

        output.Should().Be(0);
    }
}