﻿using System.Text;
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
        var (state, program) = Parse(_input);
        var output = string.Join(",", Run(state, program));

        output.Should().Be("6,0,6,3,0,2,3,1,6");
    }

    [Fact]
    public void PartTwo()
    {
        var (_, program) = Parse(_input);
        var output = GenerateA(program, program).Min();

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

    List<long> Run(List<long> state, List<long> program)
    {
        var combo = (int op) => op < 4 ? op : state[op - 4];
        var res = new List<long>();
        for (var ip = 0; ip < program.Count; ip += 2)
        {
            switch ((Opcode)program[ip], (int)program[ip + 1])
            {
                case (Opcode.Adv, var op): state[0] = state[0] >> (int)combo(op); break;
                case (Opcode.Bdv, var op): state[1] = state[0] >> (int)combo(op); break;
                case (Opcode.Cdv, var op): state[2] = state[0] >> (int)combo(op); break;
                case (Opcode.Bxl, var op): state[1] = state[1] ^ op; break;
                case (Opcode.Bst, var op): state[1] = combo(op) % 8; break;
                case (Opcode.Jnz, var op): ip = state[0] == 0 ? ip : op - 2; break;
                case (Opcode.Bxc, var op): state[1] = state[1] ^ state[2]; break;
                case (Opcode.Out, var op): res.Add(combo(op) % 8); break;
            }
        }

        return res;
    }

    /*
    Determines register A for the given output. The search works recursively and in
    reverse order, starting from the last number to be printed and ending with the first.
    */
    IEnumerable<long> GenerateA(List<long> program, List<long> output)
    {
        if (!output.Any())
        {
            yield return 0;
            yield break;
        }

        foreach (var ah in GenerateA(program, output[1..]))
        {
            for (var al = 0; al < 8; al++)
            {
                var a = ah * 8 + al;
                if (Run([a, 0, 0], program).SequenceEqual(output))
                {
                    yield return a;
                }
            }
        }
    }

    (List<long> state, List<long> program) Parse(string input)
    {
        var blocks = input.Split("\n\n").Select(ParseNums).ToArray();
        return (blocks[0], blocks[1]);
    }

    List<long> ParseNums(string st) =>
        Regex.Matches(st, @"\d+", RegexOptions.Multiline)
            .Select(m => long.Parse(m.Value))
            .ToList();
}