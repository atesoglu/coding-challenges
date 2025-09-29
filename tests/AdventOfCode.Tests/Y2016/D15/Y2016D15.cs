using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D15;

[ChallengeName("Timing is Everything")]
public class Y2016D15
{
    private readonly string _input = File.ReadAllText(@"Y2016\D15\Y2016D15-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Iterate(Parse(_input)).First(v => v.ok).t;

        output.Should().Be(376777);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Iterate(Parse(_input).Concat(new[] { (pos: 0, mod: 11) }).ToArray()).First(v => v.ok).t;

        output.Should().Be(3903937);
    }

    (int pos, int mod)[] Parse(string input) => (
        from line in input.Split('\n')
        let m = Regex.Match(line, @"Disc #\d has (\d+) positions; at time=0, it is at position (\d+).")
        select (pos: int.Parse(m.Groups[2].Value), mod: int.Parse(m.Groups[1].Value))
    ).ToArray();

    IEnumerable<(int t, bool ok)> Iterate((int pos, int mod)[] discs)
    {
        for (var t = 0;; t++)
        {
            var ok = Enumerable.Range(0, discs.Length)
                .All(i => (discs[i].pos + t + i + 1) % discs[i].mod == 0);
            yield return (t, ok);
        }
    }
}