using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2023.D09;

[ChallengeName("Mirage Maintenance")]
public class Y2023D09
{
    private readonly string _input = File.ReadAllText(@"Y2023\D09\Y2023D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, ExtrapolateRight);

        output.Should().Be(1842168671);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, ExtrapolateLeft);

        output.Should().Be(903);
    }

    long Solve(string input, Func<long[], long> extrapolate) =>
        input.Split("\n").Select(ParseNumbers).Select(extrapolate).Sum();

    long[] ParseNumbers(string line) =>
        line.Split(" ").Select(long.Parse).ToArray();

    // It's a common trick to zip a sequence with the skipped version of itself
    long[] Diff(long[] numbers) =>
        numbers.Zip(numbers.Skip(1)).Select(p => p.Second - p.First).ToArray();

    // I went a bit further and recurse until there are no numbers left. It's
    // more compact this way and doesn't affect the runtime much.
    long ExtrapolateRight(long[] numbers) =>
        !numbers.Any() ? 0 : ExtrapolateRight(Diff(numbers)) + numbers.Last();

    long ExtrapolateLeft(long[] numbers) =>
        ExtrapolateRight(numbers.Reverse().ToArray());
}