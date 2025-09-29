using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D04;

[ChallengeName("Security Through Obscurity")]
public class Y2016D04
{
    private readonly string _input = File.ReadAllText(@"Y2016\D04\Y2016D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = (
            from i in Parse(_input)
            let name = i.name.Replace("-", "")
            let computedChecksum = string.Join("", (from ch in name group ch by ch into g orderby -g.Count(), g.Key select g.Key).Take(5))
            where computedChecksum == i.checksum
            select i.sectorid
        ).Sum();

        output.Should().Be(137896);
    }

    [Fact]
    public void PartTwo()
    {
        var output = (
            from i in Parse(_input)
            let name = string.Join("", from ch in i.name select ch == '-' ? ' ' : (char)('a' + (ch - 'a' + i.sectorid) % 26))
            where name.Contains("northpole")
            select i.sectorid
        ).Single();

        output.Should().Be(501);
    }

    IEnumerable<(string name, int sectorid, string checksum)> Parse(string input)
    {
        var rx = new Regex(@"([^\d]+)\-(\d+)\[(.*)\]");

        return from line in input.Split('\n')
            let m = rx.Match(line)
            select (m.Groups[1].Value, int.Parse(m.Groups[2].Value), m.Groups[3].Value);
    }
}