using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D12;

[ChallengeName("Subterranean Sustainability")]
public class Y2018D12
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2018\D12\Y2018D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Iterate(20);

        output.Should().Be(2166);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Iterate(50000000000);

        output.Should().Be(2100000000061);
    }


    private long Iterate(long iterations)
    {
        var (state, rules) = Parse();

        var dLeftPos = 0L;

        while (iterations > 0)
        {
            var prevState = state;
            state = Step(state, rules);
            iterations--;
            dLeftPos = state.left - prevState.left;
            if (state.pots == prevState.pots)
            {
                state = new State { left = state.left + iterations * dLeftPos, pots = state.pots };
                break;
            }
        }

        return Enumerable.Range(0, state.pots.Length).Select(i => state.pots[i] == '#' ? i + state.left : 0).Sum();
    }

    private static State Step(State state, Dictionary<string, string> rules)
    {
        var pots = "....." + state.pots + ".....";
        var newPots = "";
        for (var i = 2; i < pots.Length - 2; i++)
        {
            var x = pots.Substring(i - 2, 5);
            newPots += rules.TryGetValue(x, out var ch) ? ch : ".";
        }

        var firstFlower = newPots.IndexOf("#");
        var newLeft = firstFlower + state.left - 3;

        newPots = newPots.Substring(firstFlower);
        newPots = newPots.Substring(0, newPots.LastIndexOf("#") + 1);
        var res = new State { left = newLeft, pots = newPots };

        return res;
    }

    private (State state, Dictionary<string, string> rules) Parse()
    {
        var state = new State { left = 0, pots = _lines[0].Substring("initial state: ".Length) };
        var rules = (from line in _lines.Skip(2) let parts = line.Split(" => ") select new { key = parts[0], value = parts[1] }).ToDictionary(x => x.key, x => x.value);
        return (state, rules);
    }

    private class State
    {
        public long left;
        public string pots;
    }
}