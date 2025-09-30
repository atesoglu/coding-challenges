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
        var output = Seats(_input).Max();

        output.Should().Be(822);
    }

    [Fact]
    public void PartTwo()
    {
        var seats = Seats(_input);
        var (min, max) = (seats.Min(), seats.Max());

        var output = Enumerable.Range(min, max - min + 1).Single(id => !seats.Contains(id));

        output.Should().Be(705);
    }

    private HashSet<int> Seats(string input) =>
        input
            .Replace("B", "1")
            .Replace("F", "0")
            .Replace("R", "1")
            .Replace("L", "0")
            .Split("\n")
            .Select(row => Convert.ToInt32(row, 2))
            .ToHashSet();
}