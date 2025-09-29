using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D01;

[ChallengeName("Historian Hysteria")]
public class Y2024D01
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D01\Y2024D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Enumerable.Zip(GetSortedColumnValues(_lines, 0), GetSortedColumnValues(_lines, 1))
            .Select(pair => Math.Abs(pair.First - pair.Second))
            .Sum();

        output.Should().Be(2066446);
    }

    [Fact]
    public void PartTwo()
    {
        var countsByRightValue = GetSortedColumnValues(_lines, 1).CountBy(value => value).ToDictionary();

        var output = GetSortedColumnValues(_lines, 0)
            .Select(leftValue => countsByRightValue.GetValueOrDefault(leftValue) * leftValue)
            .Sum();

        output.Should().Be(24931009);
    }

    private static IEnumerable<int> GetSortedColumnValues(IEnumerable<string> lines, int columnIndex)
    {
        return lines
            .Select(line => line.Split("   ").Select(int.Parse).ToArray())
            .OrderBy(values => values[columnIndex])
            .Select(values => values[columnIndex]);
    }
}