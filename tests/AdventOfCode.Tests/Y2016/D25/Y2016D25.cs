using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D25;

[ChallengeName("Clock Signal")]
public class Y2016D25
{
    private readonly string _input = File.ReadAllText(@"Y2016\D25\Y2016D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = 0;

        for (int a = 0;; a++)
        {
            var length = 0;
            var expectedBit = 0;
            foreach (var actualBit in Run(_input, a).Take(100))
            {
                if (actualBit == expectedBit)
                {
                    expectedBit = 1 - expectedBit;
                    length++;
                }
                else
                {
                    break;
                }
            }

            if (length == 100)
            {
                output = a;
                break;
            }
        }

        output.Should().Be(0);
    }


    IEnumerable<int> Run(string input, int a)
    {
        var prg = Parse(input);
        var regs = new Dictionary<string, int>();
        var ip = 0;

        int getReg(string reg)
        {
            return int.TryParse(reg, out var n) ? n
                : regs.ContainsKey(reg) ? regs[reg]
                : 0;
        }

        void setReg(string reg, int value)
        {
            if (!int.TryParse(reg, out var _))
            {
                regs[reg] = value;
            }
        }

        setReg("a", a);

        while (ip < prg.Length)
        {
            var stm = prg[ip];
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
                case "out":
                    yield return getReg(stm[1]);
                    ip++;
                    break;
                case "dec":
                    setReg(stm[1], getReg(stm[1]) - 1);
                    ip++;
                    break;
                case "jnz":
                    ip += getReg(stm[1]) != 0 ? getReg(stm[2]) : 1;
                    break;
                default:
                    throw new Exception("Cannot parse " + string.Join(" ", stm));
            }
        }
    }

    string[][] Parse(string input) =>
        input.Split('\n').Select(line => line.Split(' ')).ToArray();
}