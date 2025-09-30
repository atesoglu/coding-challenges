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

        output.Should().Be(168539636);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)");

        output.Should().Be(97529391);
    }

    private long Solve(string input, string pattern)
    {
        var matches = Regex.Matches(input, pattern, RegexOptions.Multiline);
        var enabled = true;
        long total = 0;

        foreach (Match match in matches)
        {
            var token = match.Value;
            if (token == "don't()")
            {
                enabled = false;
                continue;
            }
            if (token == "do()")
            {
                enabled = true;
                continue;
            }

            if (enabled)
            {
                var left = int.Parse(match.Groups[1].Value);
                var right = int.Parse(match.Groups[2].Value);
                total += left * right;
            }
        }

        return total;
    }
}