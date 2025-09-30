using System.Collections.Immutable;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D06;

[ChallengeName("Custom Customs")]
public class Y2020D06
{
    private readonly string _lines = File.ReadAllText(@"Y2020\D06\Y2020D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_lines, (a, b) => a.Union(b));

        output.Should().Be(6775);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_lines, (a, b) => a.Intersect(b));

        output.Should().Be(3356);
    }

    private static int Solve(string input, Func<ImmutableHashSet<char>, ImmutableHashSet<char>, ImmutableHashSet<char>> combine)
    {
        return (
            from grp in input.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.None)
            let answers =
                from line in grp.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries)
                select line.Trim().ToImmutableHashSet()
            select answers.Aggregate(combine).Count
        ).Sum();
    }
}