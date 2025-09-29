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
        var mostFrequent = "";
        var leastFrequent = "";

        for (var i = 0; i < _lines.First().Length; i++)
        {
            var items = _lines.GroupBy(line => line[i]).OrderBy(g => g.Count()).Select(g => g.Key);

            mostFrequent += items.Last();
            leastFrequent += items.First();
        }

        return (MostFrequent: mostFrequent, LeastFrequent: leastFrequent);
    }
}