using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D07;

[ChallengeName("Camel Cards")]
public class Y2023D07
{
    private readonly string _input = File.ReadAllText(@"Y2023\D07\Y2023D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, Part1Points);

        output.Should().Be(248179786);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, Part2Points);

        output.Should().Be(247885995);
    }


    // Each 'hand' gets points based on the card's individual value and  
    // pattern value.

    private (long, long) Part1Points(string hand) => (PatternValue(hand), CardValue(hand, "123456789TJQKA"));

    private (long, long) Part2Points(string hand)
    {
        var cards = "J123456789TQKA";
        var patternValue =
            cards.Select(ch => PatternValue(hand.Replace('J', ch))).Max();
        return (patternValue, CardValue(hand, cards));
    }

    // map cards to their indices in cardOrder. E.g. for 123456789TJQKA
    // A8A8A becomes (13)(7)(13)(7)(13), 9A34Q becomes (8)(13)(2)(3)(11)
    private long CardValue(string hand, string cardOrder) =>
        Pack(hand.Select(card => cardOrder.IndexOf(card)));

    // map cards to the number of their occurrences in the hand then order them 
    // such thatA8A8A becomes 33322, 9A34Q becomes 11111 and K99AA becomes 22221
    private long PatternValue(string hand) =>
        Pack(hand.Select(card => hand.Count(x => x == card)).OrderDescending());

    private long Pack(IEnumerable<int> numbers) =>
        numbers.Aggregate(1L, (a, v) => (a * 256) + v);

    private int Solve(string input, Func<string, (long, long)> getPoints)
    {
        var bidsByRanking = (
            from line in input.Split("\n")
            let hand = line.Split(" ")[0]
            let bid = int.Parse(line.Split(" ")[1])
            orderby getPoints(hand)
            select bid
        );

        return bidsByRanking.Select((bid, rank) => (rank + 1) * bid).Sum();
    }
}