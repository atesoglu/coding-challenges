using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2017.D08;

[ChallengeName("I Heard You Like Registers")]
public class Y2017D08
{
    private readonly string _input = File.ReadAllText(@"Y2017\D08\Y2017D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Run(_input).lastMax;

        output.Should().Be(5102);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Run(_input).runningMax;

        output.Should().Be(6056);
    }


    private (int runningMax, int lastMax) Run(string input)
    {
        var regs = new Dictionary<string, int>();
        var runningMax = 0;
        foreach (var line in input.Split('\n'))
        {
            //hsv inc 472 if hsv >= 4637
            var words = line.Split(' ');
            var (regDst, op, num, regCond, cond, condNum) = (words[0], words[1], int.Parse(words[2]), words[4], words[5], int.Parse(words[6]));
            if (!regs.ContainsKey(regDst))
            {
                regs[regDst] = 0;
            }

            if (!regs.ContainsKey(regCond))
            {
                regs[regCond] = 0;
            }

            var conditionHolds = cond switch
            {
                ">=" => regs[regCond] >= condNum,
                "<=" => regs[regCond] <= condNum,
                "==" => regs[regCond] == condNum,
                "!=" => regs[regCond] != condNum,
                ">" => regs[regCond] > condNum,
                "<" => regs[regCond] < condNum,
                _ => throw new NotImplementedException(cond)
            };
            if (conditionHolds)
            {
                regs[regDst] +=
                    op == "inc" ? num :
                    op == "dec" ? -num :
                    throw new NotImplementedException(op);
            }

            runningMax = Math.Max(runningMax, regs[regDst]);
        }

        return (runningMax, regs.Values.Max());
    }
}