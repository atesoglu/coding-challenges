using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2022.D04;

[ChallengeName("Camp Cleanup")]
public class Y2022D04
{
    private readonly string _input = File.ReadAllText(@"Y2022\D04\Y2022D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = DuplicatedWorkCount(_input, Contains);

        output.Should().Be(556);
    }

    [Fact]
    public void PartTwo()
    {
        var output = DuplicatedWorkCount(_input, Overlaps);

        output.Should().Be(876);
    }


    // Each line of the input represents two ranges - job done by two elves.
    // We need to find those lines where the elves did some work twice.
    // Part 1 and 2 differs in how we define 'duplicated work'.
    private record struct Range(int from, int to);

    // True if r1 contains r2 [ { } ]
    bool Contains(Range r1, Range r2) => r1.from <= r2.from && r2.to <= r1.to;

    // True if r1 overlaps r2 { [ } ], the other direction is not checked.
    bool Overlaps(Range r1, Range r2) => r1.to >= r2.from && r1.from <= r2.to;

    // DuplicatedWorkCount parses each input line into ranges and applies 
    // rangeCheck on them to find duplicated work. RangeCheck doesnt have to be 
    // symmetrical in its arguments, but DuplicatedWorkCount makes it so calling
    // it twice with the arguments swapped.
    private int DuplicatedWorkCount(
        string input,
        Func<Range, Range, bool> rangeCheck
    )
    {
        // E.g. '36-41,35-40' becomes [Range(36, 41), Range(35, 40)]
        var parseRanges = (string line) =>
            from range in line.Split(',')
            let fromTo = range.Split('-').Select(int.Parse)
            select new Range(fromTo.First(), fromTo.Last());

        return input
            .Split("\n")
            .Select(parseRanges)
            .Count(ranges =>
                rangeCheck(ranges.First(), ranges.Last()) ||
                rangeCheck(ranges.Last(), ranges.First())
            );
    }
}