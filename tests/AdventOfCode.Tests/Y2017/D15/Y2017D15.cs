using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D15;

[ChallengeName("Dueling Generators")]
public class Y2017D15
{
    private readonly string _input = File.ReadAllText(@"Y2017\D15\Y2017D15-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = MatchCount(Combine(ParseGenerators(_input)).Take(40000000));

        output.Should().Be(592);
    }

    [Fact]
    public void PartTwo()
    {
        var generators = ParseGenerators(_input);

        var output = MatchCount(Combine((generators.a.Where(a => (a & 3) == 0), generators.b.Where(a => (a & 7) == 0))).Take(5000000));

        output.Should().Be(320);
    }

    private static IEnumerable<(long, long)> Combine((IEnumerable<long> a, IEnumerable<long> b) items) => Enumerable.Zip(items.a, items.b, (a, b) => (a, b));

    private static int MatchCount(IEnumerable<(long a, long b)> items) => items.Count(item => (item.a & 0xffff) == (item.b & 0xffff));

    private (IEnumerable<long> a, IEnumerable<long> b) ParseGenerators(string input)
    {
        var lines = input.Split('\n');
        var startA = int.Parse(lines[0].Substring("Generator A starts with ".Length));
        var startB = int.Parse(lines[1].Substring("Generator B starts with ".Length));

        return (Generator(startA, 16807), Generator(startB, 48271));
    }

    private static IEnumerable<long> Generator(int start, int mul)
    {
        var mod = 2147483647;

        long state = start;
        while (true)
        {
            state = (state * mul) % mod;
            yield return state;
        }
    }
}