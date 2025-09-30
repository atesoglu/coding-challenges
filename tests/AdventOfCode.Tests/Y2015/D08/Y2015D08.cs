using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D08;

[ChallengeName("Matchsticks")]
public class Y2015D08
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D08\Y2015D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = (from line in _lines let u = Regex.Unescape(line.Substring(1, line.Length - 2)) select line.Length - u.Length).Sum();

        output.Should().Be(1333);
    }

    [Fact]
    public void PartTwo()
    {
        var output = (from line in _lines let u = "\"" + line.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"" select u.Length - line.Length).Sum();

        output.Should().Be(2046);
    }
}