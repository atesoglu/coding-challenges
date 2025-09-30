using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D19;

[ChallengeName("Go With The Flow")]
public class Y2018D19
{
    private string _input = File.ReadAllText(@"Y2018\D19\Y2018D19-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        // Normalize line endings to just "\n"
        _input = _input.Replace("\r\n", "\n").TrimEnd();

        var ip = 0;
        var ipReg = int.Parse(_input.Split("\n").First().Substring("#ip ".Length));
        var prg = _input.Split("\n").Skip(1).ToArray();
        var regs = new int[6];
        while (ip >= 0 && ip < prg.Length)
        {
            var args = prg[ip].Split(" ");
            regs[ipReg] = ip;
            regs = Step(regs, args[0], args.Skip(1).Select(int.Parse).ToArray());
            ip = regs[ipReg];
            ip++;
        }

        var output = regs[0];

        output.Should().Be(1344);
    }

    [Fact]
    public void PartTwo()
    {
        // Normalize line endings to just "\n"
        _input = _input.Replace("\r\n", "\n").TrimEnd();

        var t = 10551292;
        var r0 = 0;
        for (var x = 1; x <= t; x++)
        {
            if (t % x == 0)
                r0 += x;
        }

        var output = r0;

        output.Should().Be(19030032);
    }

    private static int[] Step(int[] regs, string op, int[] stm)
    {
        regs = regs.ToArray();
        regs[stm[2]] = op switch
        {
            "addr" => regs[stm[0]] + regs[stm[1]],
            "addi" => regs[stm[0]] + stm[1],
            "mulr" => regs[stm[0]] * regs[stm[1]],
            "muli" => regs[stm[0]] * stm[1],
            "banr" => regs[stm[0]] & regs[stm[1]],
            "bani" => regs[stm[0]] & stm[1],
            "borr" => regs[stm[0]] | regs[stm[1]],
            "bori" => regs[stm[0]] | stm[1],
            "setr" => regs[stm[0]],
            "seti" => stm[0],
            "gtir" => stm[0] > regs[stm[1]] ? 1 : 0,
            "gtri" => regs[stm[0]] > stm[1] ? 1 : 0,
            "gtrr" => regs[stm[0]] > regs[stm[1]] ? 1 : 0,
            "eqir" => stm[0] == regs[stm[1]] ? 1 : 0,
            "eqri" => regs[stm[0]] == stm[1] ? 1 : 0,
            "eqrr" => regs[stm[0]] == regs[stm[1]] ? 1 : 0,
            _ => throw new ArgumentException()
        };
        return regs;
    }
}