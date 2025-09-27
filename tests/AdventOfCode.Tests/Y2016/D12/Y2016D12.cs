using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D12;

[ChallengeName("Leonardo's Monorail")]
public class Y2016D12
{
    private readonly string _input = File.ReadAllText(@"Y2016\D12\Y2016D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, 0);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, 1);

        output.Should().Be(0);
    }


    int Solve(string input, int c)
    {
        var regs = new Dictionary<string, int>();
        int ip = 0;

        int getReg(string reg)
        {
            return int.TryParse(reg, out var n) ? n
                : regs.ContainsKey(reg) ? regs[reg]
                : 0;
        }

        void setReg(string reg, int value)
        {
            regs[reg] = value;
        }

        setReg("c", c);

        var prog = input.Split('\n').ToArray();
        while (ip >= 0 && ip < prog.Length)
        {
            var line = prog[ip];
            var stm = line.Split(' ');
            switch (stm[0])
            {
                case "cpy":
                    setReg(stm[2], getReg(stm[1]));
                    ip++;
                    break;
                case "inc":
                    setReg(stm[1], getReg(stm[1]) + 1);
                    ip++;
                    break;
                case "dec":
                    setReg(stm[1], getReg(stm[1]) - 1);
                    ip++;
                    break;
                case "jnz":
                    ip += getReg(stm[1]) != 0 ? getReg(stm[2]) : 1;
                    break;
                default: throw new Exception("Cannot parse " + line);
            }
        }

        return getReg("a");
    }
}