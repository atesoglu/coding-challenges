using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D02;

[ChallengeName("Inventory Management System")]
public class Y2018D02
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2018\D02\Y2018D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var doubles = (from line in _lines where CheckLine(line, 2) select line).Count();
        var tripples = (from line in _lines where CheckLine(line, 3) select line).Count();

        var output = doubles * tripples;

        output.Should().Be(9139);
    }

    [Fact]
    public void PartTwo()
    {
        var output = (from i in Enumerable.Range(0, _lines.Length)
                from j in Enumerable.Range(i + 1, _lines.Length - i - 1)
                let line1 = _lines[i]
                let line2 = _lines[j]
                where Diff(line1, line2) == 1
                select Common(line1, line2)
            ).Single();

        output.Should().Be("uqcidadzwtnhsljvxyobmkfyr");
    }

    private static bool CheckLine(string line, int n)
    {
        return (from ch in line
            group ch by ch
            into g
            select g.Count()).Any(cch => cch == n);
    }

    private static int Diff(string line1, string line2)
    {
        return line1.Zip(line2,
            (chA, chB) => chA == chB
        ).Count(x => x == false);
    }

    private static string Common(string line1, string line2)
    {
        return string.Join("", line1.Zip(line2, (chA, chB) => chA == chB ? chA.ToString() : ""));
    }
}