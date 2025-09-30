using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D08;

[ChallengeName("Handheld Halting")]
public class Y2020D08
{
    private readonly string _input = File.ReadAllText(@"Y2020\D08\Y2020D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Run(Parse(_input)).acc;

        output.Should().Be(1859);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Patches(Parse(_input))
            .Select(Run)
            .First(res => res.terminated).acc;

        output.Should().Be(1235);
    }


    private static Stm[] Parse(string input) =>
        input.Split("\n")
            .Select(line => line.Split(" "))
            .Select(parts => new Stm(parts[0], int.Parse(parts[1])))
            .ToArray();

    private static IEnumerable<Stm[]> Patches(Stm[] program) =>
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

    private (int acc, bool terminated) Run(Stm[] program)
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

internal record Stm(string op, int arg);