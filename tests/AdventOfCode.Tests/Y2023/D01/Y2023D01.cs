using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D01;

[ChallengeName("Trebuchet?!")]
public class Y2023D01
{
    private readonly string _input = File.ReadAllText(@"Y2023\D01\Y2023D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, @"\d");

        output.Should().Be(54667);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, @"\d|one|two|three|four|five|six|seven|eight|nine");

        output.Should().Be(54203);
    }


    int Solve(string input, string rx) => (
        from line in input.Split("\n")
        let first = Regex.Match(line, rx)
        let last = Regex.Match(line, rx, RegexOptions.RightToLeft)
        select ParseMatch(first.Value) * 10 + ParseMatch(last.Value)
    ).Sum();

    int ParseMatch(string st) => st switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        var d => int.Parse(d)
    };
}