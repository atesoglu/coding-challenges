using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D03;

[ChallengeName("Mull It Over")]
public class Y2024D03
{
    private readonly string _input = File.ReadAllText(@"Y2024\D03\Y2024D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, @"mul\((\d{1,3}),(\d{1,3})\)");

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)");

        output.Should().Be(0);
    }

    long Solve(string input, string rx)
    {
        // overly functionaly approach...
        var matches = Regex.Matches(input, rx, RegexOptions.Multiline);
        return matches.Aggregate(
            (enabled: true, res: 0L),
            (acc, m) =>
                (m.Value, acc.res, acc.enabled) switch
                {
                    ("don't()", _, _) => (false, acc.res),
                    ("do()", _, _) => (true, acc.res),
                    (_, var res, true) =>
                        (true, res + int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)),
                    _ => acc
                },
            acc => acc.res
        );
    }
}