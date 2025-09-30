using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D07;

[ChallengeName("Some Assembly Required")]
public class Y2015D07
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D07\Y2015D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = BuildCircuit()["a"](new SignalState());

        output.Should().Be(3176);
    }

    [Fact]
    public void PartTwo()
    {
        var calc = BuildCircuit();

        var output = calc["a"](new SignalState { ["b"] = calc["a"](new SignalState()) });

        output.Should().Be(14710);
    }

    private Circuit BuildCircuit()
    {
        var circuit = new Circuit();

        // Define gate patterns and their corresponding operations
        var gates = new (string pattern, Func<int[], int> operation)[]
        {
            (@"(\w+) AND (\w+) -> (\w+)", pin => pin[0] & pin[1]),
            (@"(\w+) OR (\w+) -> (\w+)", pin => pin[0] | pin[1]),
            (@"(\w+) RSHIFT (\w+) -> (\w+)", pin => pin[0] >> pin[1]),
            (@"(\w+) LSHIFT (\w+) -> (\w+)", pin => pin[0] << pin[1]),
            (@"NOT (\w+) -> (\w+)", pin => ~pin[0]),
            (@"(\w+) -> (\w+)", pin => pin[0])
        };

        foreach (var line in _lines)
        {
            var matched = false;

            foreach (var (instructionPattern, gateOperation) in gates)
            {
                if (TryAddGateFromInstruction(circuit, line, instructionPattern, gateOperation) != null)
                {
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                throw new Exception($"Cannot parse line: {line}");
            }
        }

        return circuit;
    }

    private static Circuit? TryAddGateFromInstruction(Circuit circuit, string line, string instructionPattern, Func<int[], int> operation)
    {
        var match = Regex.Match(line, instructionPattern);
        if (!match.Success)
        {
            return null;
        }

        var parts = match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray();
        var outputWire = parts.Last();
        var inputWires = parts.Take(parts.Length - 1).ToArray();
        circuit[outputWire] = (state) =>
        {
            if (!state.ContainsKey(outputWire))
            {
                var inputValues = inputWires.Select(pin => int.TryParse(pin, out var i) ? i : circuit[pin](state)).ToArray();
                state[outputWire] = operation(inputValues);
            }

            return state[outputWire];
        };
        return circuit;
    }

    private class SignalState : Dictionary<string, int>
    {
    }

    private class Circuit : Dictionary<string, Func<SignalState, int>>
    {
    }
}