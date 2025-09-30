using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D05;

[ChallengeName("Alchemical Reduction")]
public class Y2018D05
{
    private readonly string _input = File.ReadAllText(@"Y2018\D05\Y2018D05-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = React(_input);
        output.Should().Be(9822);
    }

    [Fact]
    public void PartTwo()
    {
        var output = (from ch in "abcdefghijklmnopqrstuvwxyz" select React(_input, ch)).Min();
        output.Should().Be(5726);
    }

    private int React(string input, char? skip = null)
    {
        var stack = new Stack<char>();

        foreach (var ch in input)
        {
            if (skip.HasValue && char.ToLowerInvariant(ch) == skip.Value)
                continue;

            if (stack.Count > 0)
            {
                var top = stack.Peek();
                if (char.ToLowerInvariant(ch) == char.ToLowerInvariant(top) &&
                    ch != top)
                {
                    stack.Pop();
                    continue;
                }
            }

            stack.Push(ch);
        }

        return stack.Count;
    }
}