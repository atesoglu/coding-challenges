using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D08;

[ChallengeName("Handheld Halting")]
public class Y2020D08
{
    private readonly string _input = File.ReadAllText(@"Y2020\D08\Y2020D08-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => Run(Parse(input)).acc;

    private object PartTwo(string input) =>
        Patches(Parse(input))
            .Select(Run)
            .First(res => res.terminated).acc;

    Stm[] Parse(string input) =>
        input.Split("\n")
            .Select(line => line.Split(" "))
            .Select(parts => new Stm(parts[0], int.Parse(parts[1])))
            .ToArray();

    IEnumerable<Stm[]> Patches(Stm[] program) =>
        Enumerable.Range(0, program.Length)
            .Where(line => program[line].op != "acc")
            .Select(lineToPatch =>
                program.Select((stm, line) =>
                    line != lineToPatch ? stm :
                    stm.op == "jmp" ? stm with { op = "nop" } :
                    stm.op == "nop" ? stm with { op = "jmp" } :
                    throw new Exception()
                ).ToArray()
            );

    (int acc, bool terminated) Run(Stm[] program)
    {
        var (ip, acc, seen) = (0, 0, new HashSet<int>());

        while (true)
        {
            if (ip >= program.Length)
            {
                return (acc, true);
            }
            else if (seen.Contains(ip))
            {
                return (acc, false);
            }
            else
            {
                seen.Add(ip);
                var stm = program[ip];
                switch (stm.op)
                {
                    case "nop": ip++; break;
                    case "acc":
                        ip++;
                        acc += stm.arg;
                        break;
                    case "jmp": ip += stm.arg; break;
                }

                ;
            }
        }
    }
}