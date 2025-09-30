using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D25;

[ChallengeName("Clock Signal")]
public class Y2016D25
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2016\D25\Y2016D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = FindLowestInputForClockSignal(_lines, signalLength: 100);
        output.Should().Be(196);
    }

    /// <summary>
    /// Finds the lowest positive integer input that produces an alternating 0/1 clock signal.
    /// </summary>
    private static int FindLowestInputForClockSignal(string[] program, int signalLength)
    {
        for (var input = 0;; input++)
        {
            var expectedBit = 0;
            var matchedLength = 0;

            foreach (var actualBit in RunClockSignal(program, input).Take(signalLength))
            {
                if (actualBit != expectedBit)
                    break;

                expectedBit = 1 - expectedBit;
                matchedLength++;
            }

            if (matchedLength == signalLength)
                return input;
        }
    }

    /// <summary>
    /// Executes the assembunny program and yields values produced by 'out' instructions.
    /// </summary>
    private static IEnumerable<int> RunClockSignal(string[] programLines, int initialA)
    {
        var instructions = programLines.Select(line => line.Split(' ')).ToArray();
        var registers = new Dictionary<string, int> { ["a"] = initialA };
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

                case "jnz":
                    instructionPointer += GetValue(registers, parts[1]) != 0 ? GetValue(registers, parts[2]) : 1;
                    break;

                case "out":
                    yield return GetValue(registers, parts[1]);
                    instructionPointer++;
                    break;

                default:
                    throw new InvalidOperationException($"Unknown assembunny instruction: {string.Join(" ", parts)}");
            }
        }
    }

    /// <summary>
    /// Fetches the value of a register or parses a literal integer.
    /// </summary>
    private static int GetValue(Dictionary<string, int> registers, string token) => int.TryParse(token, out var n) ? n : registers.GetValueOrDefault(token);

    /// <summary>
    /// Sets the value of a register (ignores literals).
    /// </summary>
    private static void SetRegister(Dictionary<string, int> registers, string register, int value)
    {
        if (!int.TryParse(register, out _))
            registers[register] = value;
    }
}