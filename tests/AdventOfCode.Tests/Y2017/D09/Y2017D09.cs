using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D09;

[ChallengeName("Stream Processing")]
public class Y2017D09
{
    private readonly string _input = File.ReadAllText(@"Y2017\D09\Y2017D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = BlockScores(_input).Sum();

        output.Should().Be(11898);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Classify(_input).Where((x) => x.garbage).Count();

        output.Should().Be(5601);
    }

    private IEnumerable<int> BlockScores(string input)
    {
        var score = 0;
        foreach (var ch in Classify(input).Where((x) => !x.garbage).Select(x => x.ch))
        {
            if (ch == '}')
            {
                score--;
            }
            else if (ch == '{')
            {
                score++;
                yield return score;
            }
        }
    }

    private static IEnumerable<(char ch, bool garbage)> Classify(string input)
    {
        var skip = false;
        var garbage = false;
        foreach (var ch in input)
        {
            if (garbage)
            {
                if (skip)
                {
                    skip = false;
                }
                else
                {
                    if (ch == '>')
                    {
                        garbage = false;
                    }
                    else if (ch == '!')
                    {
                        skip = true;
                    }
                    else
                    {
                        yield return (ch, garbage);
                    }
                }
            }
            else
            {
                if (ch == '<')
                {
                    garbage = true;
                }
                else
                {
                    yield return (ch, garbage);
                }
            }
        }
    }
}