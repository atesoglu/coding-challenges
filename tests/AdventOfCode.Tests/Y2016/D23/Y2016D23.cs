using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D23;

[ChallengeName("Safe Cracking")]
public class Y2016D23
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2016\D23\Y2016D23-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ExecuteSafeProgram(7);
        output.Should().Be(12703);
    }

    [Fact]
    public void PartTwo()
    {
        var output = ExecuteSafeProgram(12);
        output.Should().Be(479009263);
    }

    private int ExecuteSafeProgram(int initialEggs)
    {
        var instructions = ParseProgram(PatchProgram(_lines));
        var registers = new Dictionary<string, int> { ["a"] = initialEggs };
        var instructionPointer = 0;

        while (instructionPointer >= 0 && instructionPointer < instructions.Length)
        {
            var parts = instructions[instructionPointer];
            var operation = parts[0];

            switch (operation)
            {
                case "cpy":
                    SetRegister(registers, parts[2], GetValue(registers, parts[1]));
                    instructionPointer++;
                    break;

                case "inc":
                    SetRegister(registers, parts[1], GetValue(registers, parts[1]) + 1);
                    instructionPointer++;
                    break;

                case "dec":
                    SetRegister(registers, parts[1], GetValue(registers, parts[1]) - 1);
                    instructionPointer++;
                    break;

                case "mul":
                    SetRegister(registers, parts[2], GetValue(registers, parts[1]) * GetValue(registers, parts[2]));
                    instructionPointer++;
                    break;

                case "jnz":
                    instructionPointer += GetValue(registers, parts[1]) != 0 ? GetValue(registers, parts[2]) : 1;
                    break;

                case "tgl":
                    var targetIndex = instructionPointer + GetValue(registers, parts[1]);
                    if (targetIndex >= 0 && targetIndex < instructions.Length)
                    {
                        ToggleInstruction(instructions[targetIndex]);
                    }

                    instructionPointer++;
                    break;

                default:
                    throw new InvalidOperationException($"Unknown assembunny instruction: {string.Join(" ", parts)}");
            }
        }

        return registers.GetValueOrDefault("a");
    }

    // Helper: get value from register or literal
    private static int GetValue(Dictionary<string, int> registers, string token) => int.TryParse(token, out var n) ? n : registers.GetValueOrDefault(token);

    // Helper: set register value
    private static void SetRegister(Dictionary<string, int> registers, string register, int value)
    {
        if (!int.TryParse(register, out _))
            registers[register] = value;
    }

    // Helper: toggle assembunny instruction
    private static void ToggleInstruction(string[] instruction)
    {
        instruction[0] = instruction[0] switch
        {
            "cpy" => "jnz",
            "inc" => "dec",
            "dec" => "inc",
            "jnz" => "cpy",
            "tgl" => "inc",
            _ => instruction[0]
        };
    }

    // Patch program to optimize multiplication
    private static string[] PatchProgram(string[] lines)
    {
        lines[5] = "cpy c a";
        lines[6] = "mul d a";
        lines[7] = "cpy 0 d";
        lines[8] = "cpy 0 c";
        return lines;
    }

    // Parse program into instructions
    private static string[][] ParseProgram(string[] lines) => lines.Select(line => line.Split(' ')).ToArray();
}