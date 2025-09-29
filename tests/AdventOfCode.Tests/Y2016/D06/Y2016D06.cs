using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D06;

[ChallengeName("Signals and Noise")]
public class Y2016D06
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2016\D06\Y2016D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Decode().MostFrequent;

        output.Should().Be("wkbvmikb");
    }

    [Fact]
    public void PartTwo()
    {
        var output = Decode().LeastFrequent;

        output.Should().Be("evakwaga");
    }


    private (string MostFrequent, string LeastFrequent) Decode()
    {
        var columnCount = _lines.First().Length;
        var mostFrequentBuilder = new StringBuilder();
        var leastFrequentBuilder = new StringBuilder();

        for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
        {
            // Get all characters at the current column
            var charactersInColumn = _lines.Select(line => line[columnIndex]);

            // Group by character and sort by frequency
            var frequencyGroups = charactersInColumn
                .GroupBy(c => c)
                .OrderBy(g => g.Count()) // ascending by frequency
                .ToList();

            // Least frequent is first, most frequent is last
            var leastFrequentChar = frequencyGroups.First().Key;
            var mostFrequentChar = frequencyGroups.Last().Key;

            leastFrequentBuilder.Append(leastFrequentChar);
            mostFrequentBuilder.Append(mostFrequentChar);
        }

        return (mostFrequentBuilder.ToString(), leastFrequentBuilder.ToString());
    }
}