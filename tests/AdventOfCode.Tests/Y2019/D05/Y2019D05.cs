using System.Text;
using FluentAssertions;
using System.Linq;
using AdventOfCode.Tests.Y2019.D02;

namespace AdventOfCode.Tests.Y2019.D05;

[ChallengeName("Sunny with a Chance of Asteroids")]
public class Y2019D05
{
    private readonly string _input = File.ReadAllText(@"Y2019\D05\Y2019D05-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = new IntCodeMachine(_input).Run(1).Last();

        output.Should().Be(11049715);
    }

    [Fact]
    public void PartTwo()
    {
        var output = new IntCodeMachine(_input).Run(5).Last();

        output.Should().Be(2140710);
    }
}