using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D01;

[ChallengeName("Report Repair")]
public class Y2020D01
{
    private readonly string _input = File.ReadAllText(@"Y2020\D01\Y2020D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var numbers = Numbers(_input);

        var output = (
            from x in numbers
            let y = 2020 - x
            where numbers.Contains(y)
            select x * y
        ).First();

        output.Should().Be(787776);
    }

    [Fact]
    public void PartTwo()
    {
        var numbers = Numbers(_input);

        var output = (
            from x in numbers
            from y in numbers
            let z = 2020 - x - y
            where numbers.Contains(z)
            select x * y * z
        ).First();

        output.Should().Be(262738554);
    }

    private static HashSet<int> Numbers(string input)
    {
        return input.Split('\n').Select(int.Parse).ToHashSet<int>();
    }
}