using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D05;

[ChallengeName("Doesn't He Have Intern-Elves For This?")]
public class Y2015D05
{
    private static readonly HashSet<char> Vowels = ['a', 'e', 'i', 'o', 'u'];
    private static readonly string[] Forbidden = ["ab", "cd", "pq", "xy"];

    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D05\Y2015D05-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            var vowelCount = line.Count(c => Vowels.Contains(c));
            var hasDouble = false;

            for (var i = 0; i < line.Length; i++)
            {
                if (i > 0 && line[i] == line[i - 1])
                {
                    hasDouble = true;
                    break;
                }
            }

            var hasForbidden = Forbidden.Any(line.Contains);

            var isNice = vowelCount >= 3 && hasDouble && !hasForbidden;

            output += isNice ? 1 : 0;
        }

        output.Should().Be(238);
    }

    [Fact]
    public void PartTwo()
    {
        var output = 0;
        foreach (var line in _lines)
        {
            var pairs = new Dictionary<string, int>();
            var hasPair = false;
            var repeats = false;

            for (var i = 0; i < line.Length - 1; i++)
            {
                var pair = line.Substring(i, 2);
                if (pairs.TryGetValue(pair, out var lastIndex))
                {
                    hasPair = i - lastIndex >= 2 || hasPair;
                }
                else
                {
                    pairs[pair] = i;
                }

                repeats = i < line.Length - 2 && line[i] == line[i + 2] || repeats;
            }

            var isNice = hasPair && repeats;

            output += isNice ? 1 : 0;
        }

        output.Should().Be(69);
    }
}