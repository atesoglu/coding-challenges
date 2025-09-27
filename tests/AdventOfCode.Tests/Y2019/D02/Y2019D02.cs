using System.Text;
using FluentAssertions;
using System;

namespace AdventOfCode.Tests.Y2019.D02;

[ChallengeName("1202 Program Alarm")]
public class Y2019D02
{
    private readonly string _input = File.ReadAllText(@"Y2019\D02\Y2019D02-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => ExecIntCode(new IntCodeMachine(input), 12, 2);

    private object PartTwo(string input)
    {
        var icm = new IntCodeMachine(input);

        for (var sum = 0;; sum++)
        {
            for (var verb = 0; verb <= sum; verb++)
            {
                var noun = sum - verb;
                var res = ExecIntCode(icm, noun, verb);
                if (res == 19690720)
                {
                    return 100 * noun + verb;
                }
            }
        }

        throw new Exception();
    }

    long ExecIntCode(IntCodeMachine icm, int noun, int verb)
    {
        icm.Reset();
        icm.memory[1] = noun;
        icm.memory[2] = verb;
        icm.Run();
        return icm.memory[0];
    }
}