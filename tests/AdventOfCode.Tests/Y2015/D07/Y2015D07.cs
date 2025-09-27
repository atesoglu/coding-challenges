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
        var output = Parse()["a"](new State());

        output.Should().Be(3176);
    }

    [Fact]
    public void PartTwo()
    {
        var calc = Parse();

        var output = calc["a"](new State { ["b"] = calc["a"](new State()) });

        output.Should().Be(14710);
    }

    private Calc Parse()
    {
        var calc = new Calc();

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

            foreach (var (pattern, operation) in gates)
            {
                if (Gate(calc, line, pattern, operation) != null)
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

        return calc;
    }

    private static Calc Gate(Calc calc, string line, string pattern, Func<int[], int> op)
    {
        var match = Regex.Match(line, pattern);
        if (!match.Success)
        {
            return null;
        }

        var parts = match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray();
        var pinOut = parts.Last();
        var pins = parts.Take(parts.Length - 1).ToArray();
        calc[pinOut] = (state) =>
        {
            if (!state.ContainsKey(pinOut))
            {
                var args = pins.Select(pin => int.TryParse(pin, out var i) ? i : calc[pin](state)).ToArray();
                state[pinOut] = op(args);
            }

            return state[pinOut];
        };
        return calc;
    }

    private class State : Dictionary<string, int>
    {
    }

    private class Calc : Dictionary<string, Func<State, int>>
    {
    }
}