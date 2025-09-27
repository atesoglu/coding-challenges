using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D23;

[ChallengeName("Coprocessor Conflagration")]
public class Y2017D23
{
    private readonly string _input = File.ReadAllText(@"Y2017\D23\Y2017D23-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
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

        var prog = _input.Split('\n');
        var mulCount = 0;
        while (ip >= 0 && ip < prog.Length)
        {
            var line = prog[ip];
            var parts = line.Split(' ');
            switch (parts[0])
            {
                case "set":
                    setReg(parts[1], getReg(parts[2]));
                    ip++;
                    break;
                case "sub":
                    setReg(parts[1], getReg(parts[1]) - getReg(parts[2]));
                    ip++;
                    break;
                case "mul":
                    mulCount++;
                    setReg(parts[1], getReg(parts[1]) * getReg(parts[2]));
                    ip++;
                    break;
                case "jnz":
                    ip += getReg(parts[1]) != 0 ? getReg(parts[2]) : 1;
                    break;
                default: throw new Exception("Cannot parse " + line);
            }
        }

        var output = mulCount;

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var c = 0;
        for (int b = 107900; b <= 124900; b += 17)
        {
            if (!IsPrime(b))
            {
                c++;
            }
        }

        var output = c;

        output.Should().Be(0);
    }

    bool IsPrime(int n)
    {
        for (int j = 2; j * j <= n; j++)
        {
            if (n % j == 0) return false;
        }

        return true;
    }
}