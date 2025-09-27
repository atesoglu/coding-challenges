using System.Text;
using FluentAssertions;
using System.Linq;

namespace AdventOfCode.Tests.Y2019.D09;

[ChallengeName("Sensor Boost")]
public class Y2019D09
{
    private readonly string _input = File.ReadAllText(@"Y2019\D09\Y2019D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input) => new IntCodeMachine(input).Run(1).Single();
    private object PartTwo(string input) => new IntCodeMachine(input).Run(2).Single();
}