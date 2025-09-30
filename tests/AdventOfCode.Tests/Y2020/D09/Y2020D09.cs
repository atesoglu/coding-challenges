using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D09;

[ChallengeName("Encoding Error")]
public class Y2020D09
{
    private readonly string _input = File.ReadAllText(@"Y2020\D09\Y2020D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = SolvePartOne(_input);

        output.Should().Be(530627549);
    }

    [Fact]
    public void PartTwo()
    {
        var output = SolvePartTwo(_input);

        output.Should().Be(77730285);
    }


    private IEnumerable<int> Range(int min, int lim) => Enumerable.Range(min, lim - min);

    private object SolvePartOne(string input)
    {
        var numbers = input.Split("\n").Select(long.Parse).ToArray();

        bool Mismatch(int i) => (
            from j in Range(i - 25, i)
            from k in Range(j + 1, i)
            select numbers[j] + numbers[k]
        ).All(sum => sum != numbers[i]);

        return numbers[Range(25, input.Length).First(Mismatch)];
    }

    private object SolvePartTwo(string input)
    {
        var d = (long)SolvePartOne(input);
        var lines = input.Split("\n").Select(long.Parse).ToList();

        foreach (var j in Range(0, lines.Count))
        {
            var s = lines[j];
            foreach (var k in Range(j + 1, lines.Count))
            {
                s += lines[k];
                if (s > d)
                {
                    break;
                }
                else if (s == d)
                {
                    var range = lines.GetRange(j, k - j + 1);
                    return range.Min() + range.Max();
                }
            }
        }

        throw new Exception();
    }
}