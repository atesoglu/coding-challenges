using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D03;

[ChallengeName("Rucksack Reorganization")]
public class Y2022D03
{
    private readonly string _input = File.ReadAllText(@"Y2022\D03\Y2022D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        // A line can be divided into two 'compartments' of equal length. We
        // need to find the common item (letter) in them, and convert it to a
        // number called 'priority'. Do this for each line and sum the
        // priorities.
        // We use 'chunk' to split a line in half.

        var output = _input.Split("\n")
            .Select(line => line.Chunk(line.Length / 2)) // ðŸ¥©
            .Select(GetCommonItemPriority)
            .Sum();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        // Here we need to find the common item in three consecutive lines,
        // convert it to priority as before, and sum it up along the whole
        // input.
        // This is again conveniently done using the chunk function.
        var output = _input.Split("\n")
            .Chunk(3)
            .Select(GetCommonItemPriority)
            .Sum();

        output.Should().Be(0);
    }

    private int GetCommonItemPriority(IEnumerable<IEnumerable<char>> texts) => (
        from ch in texts.First()
        where texts.All(text => text.Contains(ch))
        select ch < 'a' ? ch - 'A' + 27 : ch - 'a' + 1
    ).First();
}