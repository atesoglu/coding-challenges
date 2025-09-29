using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D01;

[ChallengeName("Historian Hysteria")]
public class Y2024D01
{
    private readonly string _input = File.ReadAllText(@"Y2024\D01\Y2024D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        // go over the sorted columns pairwise and sum the difference of the pairs
        var output = Enumerable.Zip(Column(_input, 0), Column(_input, 1))
            .Select(p => Math.Abs(p.First - p.Second))
            .Sum();

        output.Should().Be(2066446);
    }

    [Fact]
    public void PartTwo()
    {
        // sum the elements of the left column weighted by its occurrences in the right
        // â­ .Net 9 comes with a new CountBy function
        var weights = Column(_input, 1).CountBy(x => x).ToDictionary();

        var output = Column(_input, 0).Select(num => weights.GetValueOrDefault(num) * num).Sum();

        output.Should().Be(24931009);
    }

    IEnumerable<int> Column(string input, int column) =>
        from line in input.Split("\n")
        let nums = line.Split("   ").Select(int.Parse).ToArray()
        orderby nums[column]
        select nums[column];
}