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
        var output = 0;
        foreach (var line in _lines)
        {
            string u = Regex.Unescape(line.Substring(1, line.Length - 2));
            output += line.Length - u.Length;
        }

        output.Should().Be(1333);
    }

    [Fact]
    public void PartTwo()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            string u = "\"" + line.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
            output += u.Length - line.Length;
        }

        output.Should().Be(2046);
    }
}