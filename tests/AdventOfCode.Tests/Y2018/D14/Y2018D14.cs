using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2018.D14;

[ChallengeName("Chocolate Charts")]
public class Y2018D14
{
    private readonly string _input = File.ReadAllText(@"Y2018\D14\Y2018D14-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => Window(10).ElementAt(int.Parse(input)).st;

    private object PartTwo(string input) => Window(input.Length).First(item => item.st == input).i;

    IEnumerable<(int i, string st)> Window(int w)
    {
        var st = "";
        var i = 0;
        foreach (var score in Scores())
        {
            i++;
            st += score;
            if (st.Length > w)
            {
                st = st.Substring(st.Length - w);
            }

            if (st.Length == w)
            {
                yield return (i - w, st);
            }
        }
    }

    IEnumerable<int> Scores()
    {
        var scores = new List<int>();
        Func<int, int> add = (i) =>
        {
            scores.Add(i);
            return i;
        };

        var elf1 = 0;
        var elf2 = 1;

        yield return add(3);
        yield return add(7);

        while (true)
        {
            var sum = scores[elf1] + scores[elf2];
            if (sum >= 10)
            {
                yield return add(sum / 10);
            }

            yield return add(sum % 10);

            elf1 = (elf1 + scores[elf1] + 1) % scores.Count;
            elf2 = (elf2 + scores[elf2] + 1) % scores.Count;
        }
    }
}