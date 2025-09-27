using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2018.D05;

[ChallengeName("Alchemical Reduction")]
public class Y2018D05
{
    private readonly string _input = File.ReadAllText(@"Y2018\D05\Y2018D05-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input) => React(input);

    private object PartTwo(string input) => (from ch in "abcdefghijklmnopqrstuvwxyz" select React(input, ch)).Min();

    char ToLower(char ch) => ch <= 'Z' ? (char)(ch - 'A' + 'a') : ch;

    int React(string input, char? skip = null)
    {
        var stack = new Stack<char>("âŠ¥");

        foreach (var ch in input)
        {
            var top = stack.Peek();
            if (ToLower(ch) == skip)
            {
                continue;
            }
            else if (top != ch && ToLower(ch) == ToLower(top))
            {
                stack.Pop();
            }
            else
            {
                stack.Push(ch);
            }
        }

        return stack.Count() - 1;
    }
}