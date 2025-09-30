using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D23;

[ChallengeName("Coprocessor Conflagration")]
public class Y2017D23
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2017\D23\Y2017D23-input.txt");

    [Fact]
    public void PartOne()
    {
        var regs = new Dictionary<string, int>();
        var ip = 0;

        int getReg(string reg) => int.TryParse(reg, out var n) ? n : regs.GetValueOrDefault(reg, 0);
        void setReg(string reg, int value) => regs[reg] = value;

        var prog = _lines;
        var mulCount = 0;

        while (ip >= 0 && ip < prog.Length)
        {
            var parts = prog[ip].Split(' ');
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
                default:
                    throw new Exception("Cannot parse " + prog[ip]);
            }
        }

        mulCount.Should().Be(6241);
    }

    [Fact]
    public void PartTwo()
    {
        var h = 0;

        // Loop over b = 108100..125100 stepping by 17
        for (var b = 108100; b <= 125100; b += 17)
        {
            if (!IsPrime(b))
            {
                h++;
            }
        }

        h.Should().Be(909);
    }

    // Checks if a number is prime
    private static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;

        var limit = (int)Math.Sqrt(n);
        for (var i = 3; i <= limit; i += 2)
        {
            if (n % i == 0) return false;
        }

        return true;
    }
}