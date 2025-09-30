using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D01;

[ChallengeName("Trebuchet?!")]
public class Y2023D01
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2023\D01\Y2023D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateCalibrationSum(_lines, @"\d");

        output.Should().Be(54667);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CalculateCalibrationSum(_lines, @"\d|one|two|three|four|five|six|seven|eight|nine");

        output.Should().Be(54203);
    }

    private int CalculateCalibrationSum(IEnumerable<string> lines, string pattern) => (
        from line in lines
        let first = Regex.Match(line, pattern)
        let last = Regex.Match(line, pattern, RegexOptions.RightToLeft)
        select ParseToken(first.Value) * 10 + ParseToken(last.Value)
    ).Sum();

    private static int ParseToken(string token) => token switch
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
        var digits => int.Parse(digits)
    };
}