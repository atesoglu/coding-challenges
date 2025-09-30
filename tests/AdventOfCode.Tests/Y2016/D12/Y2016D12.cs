using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D12;

[ChallengeName("Leonardo's Monorail")]
public class Y2016D12
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2016\D12\Y2016D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ExecuteBootSequence(0);

        output.Should().Be(318009);
    }

    [Fact]
    public void PartTwo()
    {
        var output = ExecuteBootSequence(1);

        output.Should().Be(9227663);
    }


    /// <summary>
    /// Executes the assembunny boot sequence for the monorail system.
    /// </summary>
    /// <param name="initialIgnitionKey">The initial value for register 'c', representing the ignition key.</param>
    /// <returns>The value left in register 'a' after execution (the monorail password).</returns>
    private int ExecuteBootSequence(int initialIgnitionKey)
    {
        // Registers a, b, c, d. Register 'c' may be initialized to the ignition key.
        var registers = new Dictionary<string, int> { ["c"] = initialIgnitionKey };
        var instructionPointer = 0;

        while (instructionPointer >= 0 && instructionPointer < _lines.Length)
        {
            var instructionParts = _lines[instructionPointer].Split(' ');
            var operation = instructionParts[0];

            switch (operation)
            {
                case "cpy": // copy value x into register y
                    registers[instructionParts[2]] = GetValue(instructionParts[1]);
                    instructionPointer++;
                    break;

                case "inc": // increment register x by 1
                    registers[instructionParts[1]] = GetValue(instructionParts[1]) + 1;
                    instructionPointer++;
                    break;

                case "dec": // decrement register x by 1
                    registers[instructionParts[1]] = GetValue(instructionParts[1]) - 1;
                    instructionPointer++;
                    break;

                case "jnz": // jump relative offset if x is not zero
                    instructionPointer += GetValue(instructionParts[1]) != 0 ? GetValue(instructionParts[2]) : 1;
                    break;

                default:
                    throw new InvalidOperationException($"Unknown assembunny instruction: {_lines[instructionPointer]}");
            }
        }

        // The password is the final value in register 'a'
        return registers.GetValueOrDefault("a");

        // Helper: fetch the integer value from either a literal or a register
        int GetValue(string token) => int.TryParse(token, out var number) ? number : registers.GetValueOrDefault(token);
    }
}