using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D05;

[ChallengeName("Binary Boarding")]
public class Y2020D05
{
    private readonly string _input = File.ReadAllText(@"Y2020\D05\Y2020D05-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => Seats(input).Max();

    private object PartTwo(string input)
    {
        var seats = Seats(input);
        var (min, max) = (seats.Min(), seats.Max());
        return Enumerable.Range(min, max - min + 1).Single(id => !seats.Contains(id));
    }

    HashSet<int> Seats(string input) =>
        input
            .Replace("B", "1")
            .Replace("F", "0")
            .Replace("R", "1")
            .Replace("L", "0")
            .Split("\n")
            .Select(row => Convert.ToInt32(row, 2))
            .ToHashSet();
}