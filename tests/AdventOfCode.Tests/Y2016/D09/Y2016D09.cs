using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D09;

[ChallengeName("Explosives in Cyberspace")]
public class Y2016D09
{
    private readonly string _input = File.ReadAllText(@"Y2016\D09\Y2016D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Expand(_input, 0, _input.Length, false);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Expand(_input, 0, _input.Length, true);

        output.Should().Be(0);
    }

    long Expand(string input, int i, int lim, bool recursive)
    {
        var res = 0L;
        while (i < lim)
        {
            if (input[i] == '(')
            {
                var j = input.IndexOf(')', i + 1);
                var m = Regex.Match(input.Substring(i + 1, j - i - 1), @"(\d+)x(\d+)");
                var length = int.Parse(m.Groups[1].Value);
                var mul = int.Parse(m.Groups[2].Value);
                res += recursive ? Expand(input, j + 1, j + length + 1, recursive) * mul : length * mul;
                i = j + length + 1;
            }
            else
            {
                res++;
                i++;
            }
        }

        return res;
    }
}