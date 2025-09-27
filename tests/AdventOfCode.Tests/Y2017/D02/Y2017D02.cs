using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D02;

[ChallengeName("Corruption Checksum")]
public class Y2017D02
{
    private readonly string _input = File.ReadAllText(@"Y2017\D02\Y2017D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = (
            from line in _input.Split('\n')
            let numbers = line.Split('\t').Select(int.Parse)
            select numbers.Max() - numbers.Min()
        ).Sum();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = (
            from line in _input.Split('\n')
            let numbers = line.Split('\t').Select(int.Parse)
            from a in numbers
            from b in numbers
            where a > b && a % b == 0
            select a / b
        ).Sum();

        output.Should().Be(0);
    }
}