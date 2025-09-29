using System.Text;
using FluentAssertions;
using System.Linq;
using AdventOfCode.Tests.Y2019.D02;

namespace AdventOfCode.Tests.Y2019.D09;

[ChallengeName("Sensor Boost")]
public class Y2019D09
{
    private readonly string _input = File.ReadAllText(@"Y2019\D09\Y2019D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = new IntCodeMachine(_input).Run(1).Single();

        output.Should().Be(2494485073);
    }

    [Fact]
    public void PartTwo()
    {
        var output = new IntCodeMachine(_input).Run(2).Single();

        output.Should().Be(44997);
    }
}