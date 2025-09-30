using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D23;

[ChallengeName("Opening the Turing Lock")]
public class Y2015D23
{
    private readonly string _input = File.ReadAllText(@"Y2015\D23\Y2015D23-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        ExecuteInstructions(_input, initialA: 0).Should().Be(184);
    }

    [Fact]
    public void PartTwo()
    {
        ExecuteInstructions(_input, initialA: 1).Should().Be(231);
    }

    private static long ExecuteInstructions(string input, long initialA)
    {
        var registers = new Dictionary<string, long>
        {
            ["a"] = initialA,
            ["b"] = 0
        };

        var instructions = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        long instructionPointer = 0;

        while (instructionPointer >= 0 && instructionPointer < instructions.Length)
        {
            var parts = instructions[instructionPointer].Replace(",", "").Split(' ');

            switch (parts[0])
            {
                case "hlf":
                    registers[parts[1]] /= 2;
                    instructionPointer++;
                    break;

                case "tpl":
                    registers[parts[1]] *= 3;
                    instructionPointer++;
                    break;

                case "inc":
                    registers[parts[1]]++;
                    instructionPointer++;
                    break;

                case "jmp":
                    instructionPointer += ReadRegister(parts[1]);
                    break;

                case "jie":
                    instructionPointer += ReadRegister(parts[1]) % 2 == 0 ? ReadRegister(parts[2]) : 1;
                    break;

                case "jio":
                    instructionPointer += ReadRegister(parts[1]) == 1 ? ReadRegister(parts[2]) : 1;
                    break;

                default:
                    throw new InvalidOperationException($"Unknown instruction: {instructions[instructionPointer]}");
            }
        }

        return registers["b"];

        long ReadRegister(string name) => long.TryParse(name, out var value) ? value : registers.GetValueOrDefault(name, 0);
    }
}