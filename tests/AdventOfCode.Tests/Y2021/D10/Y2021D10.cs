using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2021.D10;

[ChallengeName("Syntax Scoring")]
public class Y2021D10
{
    private readonly string _input = File.ReadAllText(@"Y2021\D10\Y2021D10-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetScores(_input, getSyntaxErrorScore: true).Sum();

        output.Should().Be(345441);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Median(GetScores(_input, getSyntaxErrorScore: false));

        output.Should().Be(3235371166);
    }


    private long Median(IEnumerable<long> items) =>
        items.OrderBy(x => x).ElementAt(items.Count() / 2);

    IEnumerable<long> GetScores(string input, bool getSyntaxErrorScore) =>
        input.Split("\n").Select(line => GetScore(line, getSyntaxErrorScore)).Where(score => score > 0);

    long GetScore(string line, bool getSyntaxErrorScore)
    {
        // standard stack based approach
        var stack = new Stack<char>();

        foreach (var ch in line)
        {
            switch ((stack.FirstOrDefault(), ch))
            {
                // matching closing parenthesis:
                case ('(', ')'): stack.Pop(); break;
                case ('[', ']'): stack.Pop(); break;
                case ('{', '}'): stack.Pop(); break;
                case ('<', '>'): stack.Pop(); break;
                // return early if syntax error found:
                case (_, ')'): return getSyntaxErrorScore ? 3 : 0;
                case (_, ']'): return getSyntaxErrorScore ? 57 : 0;
                case (_, '}'): return getSyntaxErrorScore ? 1197 : 0;
                case (_, '>'): return getSyntaxErrorScore ? 25137 : 0;
                // otherwise, it's an opening parenthesis:
                case (_, _): stack.Push(ch); break;
            }
        }

        if (getSyntaxErrorScore)
        {
            return 0;
        }
        else
        {
            return stack
                .Select(item => 1 + "([{<".IndexOf(item)) // convert chars to digits
                .Aggregate(0L, (acc, item) => acc * 5 + item); // get base 5 number
        }
    }
}