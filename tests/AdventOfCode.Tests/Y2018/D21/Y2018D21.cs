using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2018.D21;

[ChallengeName("Chronal Conversion")]
public class Y2018D21
{
    private readonly string _input = File.ReadAllText(@"Y2018\D21\Y2018D21-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => Run(input).First();
    private object PartTwo(string input) => Run(input).Last();

    private IEnumerable<long> Run(string input)
    {
        var breakpoint = 28;
        var seen = new List<long>();

        foreach (var regs in Trace(input, breakpoint))
        {
            if (seen.Contains(regs[3]))
            {
                break;
            }

            seen.Add(regs[3]);
            yield return regs[3];
        }
    }

    private IEnumerable<long[]> Trace(string input, int breakpoint)
    {
        var lines = input.Split("\n");
        var ipReg = int.Parse(lines.First().Split(" ")[1]);
        var program = lines.Skip(1).Select(Compile).ToArray();
        var regs = new long[] { 0, 0, 0, 0, 0, 0 };

        while (true)
        {
            if (regs[ipReg] == breakpoint)
            {
                yield return regs;
            }

            program[regs[ipReg]](regs);
            regs[ipReg]++;
        }
    }

    Action<long[]> Compile(string line)
    {
        var parts = line.Split(" ");
        var op = parts[0];
        var args = parts.Skip(1).Select(long.Parse).ToArray();
        return op switch
        {
            "addr" => regs => regs[args[2]] = regs[args[0]] + regs[args[1]],
            "addi" => regs => regs[args[2]] = regs[args[0]] + args[1],
            "mulr" => regs => regs[args[2]] = regs[args[0]] * regs[args[1]],
            "muli" => regs => regs[args[2]] = regs[args[0]] * args[1],
            "banr" => regs => regs[args[2]] = regs[args[0]] & regs[args[1]],
            "bani" => regs => regs[args[2]] = regs[args[0]] & args[1],
            "borr" => regs => regs[args[2]] = regs[args[0]] | regs[args[1]],
            "bori" => regs => regs[args[2]] = regs[args[0]] | args[1],
            "setr" => regs => regs[args[2]] = regs[args[0]],
            "seti" => regs => regs[args[2]] = args[0],
            "gtir" => regs => regs[args[2]] = args[0] > regs[args[1]] ? 1 : 0,
            "gtri" => regs => regs[args[2]] = regs[args[0]] > args[1] ? 1 : 0,
            "gtrr" => regs => regs[args[2]] = regs[args[0]] > regs[args[1]] ? 1 : 0,
            "eqir" => regs => regs[args[2]] = args[0] == regs[args[1]] ? 1 : 0,
            "eqri" => regs => regs[args[2]] = regs[args[0]] == args[1] ? 1 : 0,
            "eqrr" => regs => regs[args[2]] = regs[args[0]] == regs[args[1]] ? 1 : 0,
            _ => throw new ArgumentException()
        };
    }
}