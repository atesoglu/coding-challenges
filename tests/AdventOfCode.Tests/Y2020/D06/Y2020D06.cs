using System.Text;
using FluentAssertions;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D06;

[ChallengeName("Custom Customs      ")]
public class Y2020D06
{
    private readonly string _input = File.ReadAllText(@"Y2020\D06\Y2020D06-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => Solve(input, (a, b) => a.Union(b));
    private object PartTwo(string input) => Solve(input, (a, b) => a.Intersect(b));

    int Solve(string input, Func<ImmutableHashSet<char>, ImmutableHashSet<char>, ImmutableHashSet<char>> combine)
    {
        return (
            from grp in input.Split("\n\n")
            let answers = from line in grp.Split("\n") select line.ToImmutableHashSet()
            select answers.Aggregate(combine).Count
        ).Sum();
    }
}