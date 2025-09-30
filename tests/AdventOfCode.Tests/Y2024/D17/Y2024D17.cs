using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D17;

[ChallengeName("Chronospatial Computer")]
public class Y2024D17
{
    private readonly string _input = File.ReadAllText(@"Y2024\D17\Y2024D17-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (state, program) = ParseInput(_input);
        var output = string.Join(",", ExecuteProgram(state, program));

        output.Should().Be("6,0,6,3,0,2,3,1,6");
    }

    [Fact]
    public void PartTwo()
    {
        var (_, program) = ParseInput(_input);
        var output = FindValidRegisters(program, program).Min();

        output.Should().Be(236539226447469);
    }


    private enum Opcode
    {
        Adv,
        Bxl,
        Bst,
        Jnz,
        Bxc,
        Out,
        Bdv,
        Cdv
    }

    private static List<long> ExecuteProgram(List<long> state, List<long> program)
    {
        var getOperand = (int operand) => operand < 4 ? operand : state[operand - 4];
        var output = new List<long>();
        for (var instructionPointer = 0; instructionPointer < program.Count; instructionPointer += 2)
        {
            switch ((Opcode)program[instructionPointer], (int)program[instructionPointer + 1])
            {
                case (Opcode.Adv, var operand): state[0] = state[0] >> (int)getOperand(operand); break;
                case (Opcode.Bdv, var operand): state[1] = state[0] >> (int)getOperand(operand); break;
                case (Opcode.Cdv, var operand): state[2] = state[0] >> (int)getOperand(operand); break;
                case (Opcode.Bxl, var operand): state[1] = state[1] ^ operand; break;
                case (Opcode.Bst, var operand): state[1] = getOperand(operand) % 8; break;
                case (Opcode.Jnz, var operand): instructionPointer = state[0] == 0 ? instructionPointer : operand - 2; break;
                case (Opcode.Bxc, var operand): state[1] = state[1] ^ state[2]; break;
                case (Opcode.Out, var operand): output.Add(getOperand(operand) % 8); break;
            }
        }

        return output;
    }

    private IEnumerable<long> FindValidRegisters(List<long> program, List<long> expectedOutput)
    {
        if (!expectedOutput.Any())
        {
            yield return 0;
            yield break;
        }

        foreach (var highBits in FindValidRegisters(program, expectedOutput[1..]))
        {
            for (var lowBits = 0; lowBits < 8; lowBits++)
            {
                var registerA = highBits * 8 + lowBits;
                if (ExecuteProgram([registerA, 0, 0], program).SequenceEqual(expectedOutput))
                {
                    yield return registerA;
                }
            }
        }
    }

    private (List<long> state, List<long> program) ParseInput(string input)
    {
        var blocks = input.Split("\n\n").Select(ParseNumbers).ToArray();
        return (blocks[0], blocks[1]);
    }

    private List<long> ParseNumbers(string text) =>
        Regex.Matches(text, @"\d+", RegexOptions.Multiline)
            .Select(match => long.Parse(match.Value))
            .ToList();
}