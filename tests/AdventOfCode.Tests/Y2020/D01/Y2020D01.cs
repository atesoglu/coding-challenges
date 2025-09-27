using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D01;

[ChallengeName("Report Repair")]
public class Y2020D01
{
    private readonly string _input = File.ReadAllText(@"Y2020\D01\Y2020D01-input.txt", Encoding.UTF8);

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


    private object PartOne(string input)
    {
        var numbers = Numbers(input);
        return (
            from x in numbers
            let y = 2020 - x
            where numbers.Contains(y)
            select x * y
        ).First();
    }

    private object PartTwo(string input)
    {
        var numbers = Numbers(input);
        return (
            from x in numbers
            from y in numbers
            let z = 2020 - x - y
            where numbers.Contains(z)
            select x * y * z
        ).First();
    }

    HashSet<int> Numbers(string input)
    {
        return input.Split('\n').Select(int.Parse).ToHashSet<int>();
    }
}