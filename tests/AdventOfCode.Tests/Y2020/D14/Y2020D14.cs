using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D14;

[ChallengeName("Docking Data")]
public class Y2020D14
{
    private readonly string _input = File.ReadAllText(@"Y2020\D14\Y2020D14-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var mem = new Dictionary<long, long>();
        var orMask = 0L;
        var andMask = 0xffffffffffffffL;
        foreach (var line in _input.Split("\n"))
        {
            if (line.StartsWith("mask"))
            {
                var mask = line.Split(" = ")[1];
                andMask = Convert.ToInt64(mask.Replace("X", "1"), 2);
                orMask = Convert.ToInt64(mask.Replace("X", "0"), 2);
            }
            else
            {
                var num = Regex.Matches(line, "\\d+").Select(match => long.Parse(match.Value)).ToArray();
                mem[num[0]] = num[1] & andMask | orMask;
            }
        }

        var output = mem.Values.Sum();

        output.Should().Be(10885823581193);
    }

    [Fact]
    public void PartTwo()
    {
        var mem = new Dictionary<long, long>();
        var mask = "";
        foreach (var line in _input.Split("\n"))
        {
            if (line.StartsWith("mask"))
            {
                mask = line.Split(" = ")[1];
            }
            else
            {
                var num = Regex.Matches(line, "\\d+").Select(match => long.Parse(match.Value)).ToArray();
                var (baseAddr, value) = (num[0], num[1]);
                foreach (var addr in Addresses(baseAddr, mask, 35))
                {
                    mem[addr] = value;
                }
            }
        }

        var output = mem.Values.Sum();

        output.Should().Be(3816594901962);
    }

    private static IEnumerable<long> Addresses(long baseAddr, string mask, int i)
    {
        if (i == -1)
        {
            yield return 0;
        }
        else
        {
            foreach (var prefix in Addresses(baseAddr, mask, i - 1))
            {
                if (mask[i] == '0')
                {
                    yield return (prefix << 1) + ((baseAddr >> 35 - i) & 1);
                }
                else if (mask[i] == '1')
                {
                    yield return (prefix << 1) + 1;
                }
                else
                {
                    yield return (prefix << 1);
                    yield return (prefix << 1) + 1;
                }
            }
        }
    }
}