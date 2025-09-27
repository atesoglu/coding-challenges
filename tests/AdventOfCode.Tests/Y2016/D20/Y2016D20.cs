using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2016.D20;

[ChallengeName("Firewall Rules")]
public class Y2016D20
{
    private readonly string _input = File.ReadAllText(@"Y2016\D20\Y2016D20-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var k = 0L;
        foreach (var range in Parse(_input))
        {
            if (k < range.min)
            {
                break;
            }
            else if (range.min <= k && k <= range.max)
            {
                k = range.max + 1;
            }
        }

        var output = k;

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var k = 0L;
        var sum = 0L;
        foreach (var range in Parse(_input))
        {
            if (k < range.min)
            {
                sum += range.min - k;
                k = range.max + 1;
            }
            else if (range.min <= k && k <= range.max)
            {
                k = range.max + 1;
            }
        }

        var lim = 4294967296L;
        if (lim > k)
        {
            sum += lim - k;
        }

        var output = sum;

        output.Should().Be(0);
    }

    IEnumerable<(long min, long max)> Parse(string input) => (
        from line in input.Split('\n')
        let parts = line.Split('-')
        let min = long.Parse(parts[0])
        let max = long.Parse(parts[1])
        orderby min
        select (min, max)
    ).AsEnumerable();
}