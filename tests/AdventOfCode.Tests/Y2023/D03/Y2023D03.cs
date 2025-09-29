using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D03;

[ChallengeName("Gear Ratios")]
public class Y2023D03
{
    private readonly string _input = File.ReadAllText(@"Y2023\D03\Y2023D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        // Introduce a Parse function that returns the interesting 'blocks' of texts
        // and positions using a regex. Then just filter and match these according
        // to the problem spec.
        var rows = _input.Split("\n");
        var symbols = Parse(rows, new Regex(@"[^.0-9]"));
        var nums = Parse(rows, new Regex(@"\d+"));

        var output = (
            from n in nums
            where symbols.Any(s => Adjacent(s, n))
            select n.Int
        ).Sum();

        output.Should().Be(531561);
    }

    [Fact]
    public void PartTwo()
    {
        var rows = _input.Split("\n");
        var gears = Parse(rows, new Regex(@"\*"));
        var numbers = Parse(rows, new Regex(@"\d+"));

        var output = (
            from g in gears
            let neighbours = from n in numbers where Adjacent(n, g) select n.Int
            where neighbours.Count() == 2
            select neighbours.First() * neighbours.Last()
        ).Sum();

        output.Should().Be(83279367);
    }

    // checks that the parts are touching each other, i.e. rows are within 1 
    // step and also the columns (using https://stackoverflow.com/a/3269471).
    bool Adjacent(Part p1, Part p2) =>
        Math.Abs(p2.Irow - p1.Irow) <= 1 &&
        p1.Icol <= p2.Icol + p2.Text.Length &&
        p2.Icol <= p1.Icol + p1.Text.Length;

    // returns the matches of rx with its coordinates
    Part[] Parse(string[] rows, Regex rx) => (
        from irow in Enumerable.Range(0, rows.Length)
        from match in rx.Matches(rows[irow])
        select new Part(match.Value, irow, match.Index)
    ).ToArray();
}record Part(string Text, int Irow, int Icol) {
    public int Int => int.Parse(Text);
}