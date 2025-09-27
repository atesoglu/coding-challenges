using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Tests.Y2019.D04;

[ChallengeName("Secure Container")]
public class Y2019D04
{
    private readonly string _input = File.ReadAllText(@"Y2019\D04\Y2019D04-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => Solve(input, true);
    private object PartTwo(string input) => Solve(input, false);

    private int Solve(string input, bool trippletsAllowed)
    {
        var args = input.Split("-").Select(int.Parse).ToArray();
        return (
            from i in Enumerable.Range(args[0], args[1] - args[0] + 1)
            where OK(i.ToString(), trippletsAllowed)
            select i
        ).Count();
    }

    private bool OK(string password, bool trippletsAllowed)
    {
        if (string.Join("", password.OrderBy(ch => ch)) != password)
        {
            return false;
        }

        return (
            from sequence in Split(password)
            where sequence.Length >= 2 && (trippletsAllowed || sequence.Length == 2)
            select sequence
        ).Any();
    }

    private IEnumerable<string> Split(string st)
    {
        var ich = 0;
        while (ich < st.Length)
        {
            var sequence = Regex.Match(st.Substring(ich), @$"[{st[ich]}]+").Value;
            yield return sequence;
            ich += sequence.Length;
        }
    }
}