using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D06;

[ChallengeName("Signals and Noise")]
public class Y2016D06
{
    private readonly string _input = File.ReadAllText(@"Y2016\D06\Y2016D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Decode(_input).mostFrequent;

        output.Should().Be("wkbvmikb");
    }

    [Fact]
    public void PartTwo()
    {
        var output = Decode(_input).leastFrequent;

        output.Should().Be("evakwaga");
    }


    (string mostFrequent, string leastFrequent) Decode(string input)
    {
        var lines = input.Split('\n');
        var mostFrequent = "";
        var leastFrequent = "";
        for (var i = 0; i < lines[0].Length; i++)
        {
            var items = (from line in lines group line by line[i] into g orderby g.Count() select g.Key);
            mostFrequent += items.Last();
            leastFrequent += items.First();
        }

        return (mostFrequent: mostFrequent, leastFrequent: leastFrequent);
    }
}