using System.Text;
using FluentAssertions;
using System.Linq;

namespace AdventOfCode.Tests.Y2019.D05;

[ChallengeName("Sunny with a Chance of Asteroids")]
public class Y2019D05
{
    private readonly string _input = File.ReadAllText(@"Y2019\D05\Y2019D05-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => new IntCodeMachine(input).Run(1).Last();

    private object PartTwo(string input) => new IntCodeMachine(input).Run(5).Last();
}