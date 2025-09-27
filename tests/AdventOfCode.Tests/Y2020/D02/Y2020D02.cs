using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D02;

[ChallengeName("Password Philosophy")]
public class Y2020D02
{
    private readonly string _input = File.ReadAllText(@"Y2020\D02\Y2020D02-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => ValidCount(input, (PasswordEntry pe) =>
    {
        var count = pe.password.Count(ch => ch == pe.ch);
        return pe.a <= count && count <= pe.b;
    });

    private object PartTwo(string input) => ValidCount(input, (PasswordEntry pe) => { return (pe.password[pe.a - 1] == pe.ch) ^ (pe.password[pe.b - 1] == pe.ch); });

    int ValidCount(string input, Func<PasswordEntry, bool> isValid) =>
        input
            .Split("\n")
            .Select(line =>
            {
                var parts = line.Split(' ');
                var range = parts[0].Split('-').Select(int.Parse).ToArray();
                var ch = parts[1][0];
                return new PasswordEntry(range[0], range[1], ch, parts[2]);
            })
            .Count(isValid);
}