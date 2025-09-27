using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using Signal = (string sender, string receiver, bool value);

namespace AdventOfCode.Tests.Y2023.D20;

[ChallengeName("Pulse Propagation")]
public class Y2023D20
{
    private readonly string _input = File.ReadAllText(@"Y2023\D20\Y2023D20-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var gates = ParseGates(_input);
        var values = (
            from _ in Enumerable.Range(0, 1000)
            from signal in Trigger(gates)
            select signal.value
        ).ToArray();

        var output = values.Count(v => v) * values.Count(v => !v);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        // The input has a special structure. Broadcaster feeds 4 disconnected
        // substructures which are channeled into a single nand gate at the end.
        // The nand gate is connected into rx. I checked that the substructures
        // work in a loop, that has prime length. Just need to multiply them all.
        var gates = ParseGates(_input);
        var nand = gates["rx"].inputs.Single();
        var branches = gates[nand].inputs;

        var output = branches.Aggregate(1L, (m, branch) => m * LoopLength(_input, branch));

        output.Should().Be(0);
    }

    int LoopLength(string input, string output)
    {
        var gates = ParseGates(input);
        for (var i = 1;; i++)
        {
            var signals = Trigger(gates);
            if (signals.Any(s => s.sender == output && s.value))
            {
                return i;
            }
        }
    }

    // emits a button press, executes until things settle down and returns 
    // all signals for investigation.
    IEnumerable<Signal> Trigger(Dictionary<string, Gate> gates)
    {
        var q = new Queue<Signal>();
        q.Enqueue(new Signal("button", "broadcaster", false));

        while (q.TryDequeue(out var signal))
        {
            yield return signal;

            var handler = gates[signal.receiver];
            foreach (var signalT in handler.handle(signal))
            {
                q.Enqueue(signalT);
            }
        }
    }

    Dictionary<string, Gate> ParseGates(string input)
    {
        input += "\nrx ->"; // an extra rule for rx with no output

        var descriptions =
            from line in input.Split('\n')
            let words = Regex.Matches(line, "\\w+").Select(m => m.Value).ToArray()
            select (kind: line[0], name: words.First(), outputs: words[1..]);

        var inputs = (string name) => (
            from d in descriptions where d.outputs.Contains(name) select d.name
        ).ToArray();

        return descriptions.ToDictionary(
            d => d.name,
            d => d.kind switch
            {
                '&' => NandGate(d.name, inputs(d.name), d.outputs),
                '%' => FlipFlop(d.name, inputs(d.name), d.outputs),
                _ => Repeater(d.name, inputs(d.name), d.outputs)
            }
        );
    }

    Gate NandGate(string name, string[] inputs, string[] outputs)
    {
        // initially assign low value for each input:
        var state = inputs.ToDictionary(input => input, _ => false);

        return new Gate(inputs, (Signal signal) =>
        {
            state[signal.sender] = signal.value;
            var value = !state.Values.All(b => b);
            return outputs.Select(o => new Signal(name, o, value));
        });
    }

    Gate FlipFlop(string name, string[] inputs, string[] outputs)
    {
        var state = false;

        return new Gate(inputs, (Signal signal) =>
        {
            if (!signal.value)
            {
                state = !state;
                return outputs.Select(o => new Signal(name, o, state));
            }
            else
            {
                return [];
            }
        });
    }

    Gate Repeater(string name, string[] inputs, string[] outputs)
    {
        return new Gate(inputs, (Signal s) =>
            from o in outputs select new Signal(name, o, s.value)
        );
    }
}
record Gate(string[] inputs, Func<Signal, IEnumerable<Signal>> handle);