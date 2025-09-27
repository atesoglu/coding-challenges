using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D04;

[ChallengeName("Scratchcards")]
public class Y2023D04
{
    private readonly string _input = File.ReadAllText(@"Y2023\D04\Y2023D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = (
            from line in _input.Split("\n")
            let card = ParseCard(line)
            where card.matches > 0
            select Math.Pow(2, card.matches - 1)
        ).Sum();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        // Quite imperatively, just walk over the cards keeping track of the counts.


        var cards = _input.Split("\n").Select(ParseCard).ToArray();
        var counts = cards.Select(_ => 1).ToArray();

        for (var i = 0; i < cards.Length; i++)
        {
            var (card, count) = (cards[i], counts[i]);
            for (var j = 0; j < card.matches; j++)
            {
                counts[i + j + 1] += count;
            }
        }

        var output = counts.Sum();

        output.Should().Be(0);
    }

    // Only the match count is relevant for a card
    Card ParseCard(string line)
    {
        var parts = line.Split(':', '|');
        var l = from m in Regex.Matches(parts[1], @"\d+") select m.Value;
        var r = from m in Regex.Matches(parts[2], @"\d+") select m.Value;
        return new Card(l.Intersect(r).Count());
    }
}
record Card(int matches);