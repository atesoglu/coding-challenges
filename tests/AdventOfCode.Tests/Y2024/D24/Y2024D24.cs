using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D24;

using Circuit = Dictionary<string, Gate>;

[ChallengeName("Crossed Wires")]
public class Y2024D24
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D24\Y2024D24-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (inputs, circuit) = Parse(_lines);

        var outputs = from label in circuit.Keys where label.StartsWith("z") select label;

        var res = 0L;
        foreach (var label in outputs.OrderByDescending(label => label))
        {
            res = res * 2 + Eval(label, circuit, inputs);
        }

        var output = res;

        output.Should().Be(51715173446832);
    }

    [Fact]
    public void PartTwo()
    {
        var circuit = Parse(_lines).circuit;
        var output = string.Join(",", Fix(circuit).OrderBy(label => label));

        output.Should().Be("dpg,kmb,mmf,tvp,vdk,z10,z15,z25");
    }

    private static int Eval(string label, Circuit circuit, Dictionary<string, int> inputs)
    {
        if (inputs.TryGetValue(label, out var res))
        {
            return res;
        }
        else
        {
            return circuit[label] switch
            {
                Gate(var in1, "AND", var in2) => Eval(in1, circuit, inputs) & Eval(in2, circuit, inputs),
                Gate(var in1, "OR", var in2) => Eval(in1, circuit, inputs) | Eval(in2, circuit, inputs),
                Gate(var in1, "XOR", var in2) => Eval(in1, circuit, inputs) ^ Eval(in2, circuit, inputs),
                _ => throw new Exception(circuit[label].ToString()),
            };
        }
    }

    private IEnumerable<string> Fix(Circuit circuit)
    {
        var cin = Output(circuit, "x00", "AND", "y00");
        for (var i = 1; i < 45; i++)
        {
            var x = $"x{i:D2}";
            var y = $"y{i:D2}";
            var z = $"z{i:D2}";

            var xor1 = Output(circuit, x, "XOR", y);
            var and1 = Output(circuit, x, "AND", y);
            var xor2 = Output(circuit, cin, "XOR", xor1);
            var and2 = Output(circuit, cin, "AND", xor1);

            if (xor2 == null && and2 == null)
            {
                return SwapAndFix(circuit, xor1, and1);
            }

            var carry = Output(circuit, and1, "OR", and2);
            if (xor2 != z)
            {
                return SwapAndFix(circuit, z, xor2);
            }
            else
            {
                cin = carry;
            }
        }

        return [];
    }

    private IEnumerable<string> SwapAndFix(Circuit circuit, string out1, string out2)
    {
        (circuit[out1], circuit[out2]) = (circuit[out2], circuit[out1]);
        return Fix(circuit).Concat([out1, out2]);
    }

    private static string Output(Circuit circuit, string x, string kind, string y) =>
        circuit.SingleOrDefault(pair =>
            (pair.Value.in1 == x && pair.Value.kind == kind && pair.Value.in2 == y) ||
            (pair.Value.in1 == y && pair.Value.kind == kind && pair.Value.in2 == x)
        ).Key;

    private static (Dictionary<string, int> inputs, Circuit circuit) Parse(IEnumerable<string> lines)
    {
        var inputs = new Dictionary<string, int>();
        var circuit = new Circuit();

        var inputText = string.Join("\n", lines);
        var blocks = inputText.Split("\n\n");

        foreach (var line in blocks[0].Split("\n"))
        {
            var parts = line.Split(": ");
            inputs.Add(parts[0], int.Parse(parts[1]));
        }

        foreach (var line in blocks[1].Split("\n"))
        {
            var parts = Regex.Matches(line, "[a-zA-z0-9]+").Select(m => m.Value).ToArray();
            circuit.Add(parts[3], new Gate(in1: parts[0], kind: parts[1], in2: parts[2]));
        }

        return (inputs, circuit);
    }
}

internal record struct Gate(string in1, string kind, string in2);