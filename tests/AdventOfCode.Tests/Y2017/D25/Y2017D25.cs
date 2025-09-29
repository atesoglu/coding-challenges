using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D25;

[ChallengeName("The Halting Problem")]
public class Y2017D25
{
    private readonly string _input = File.ReadAllText(@"Y2017\D25\Y2017D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var machine = Parse(_input);
        var tape = new Dictionary<int, int>();
        var pos = 0;
        while (machine.iterations > 0)
        {
            var read = tape.TryGetValue(pos, out var t) ? t : 0;
            var (write, dir, newState) = machine.prg[(machine.state, read)];
            machine.state = newState;
            tape[pos] = write;
            pos += dir;
            machine.iterations--;
        }

        var output = tape.Select(kvp => kvp.Value).Sum();

        output.Should().Be(4385);
    }

    Machine Parse(string input)
    {
        var lines = input.Split('\n').Where(line => !string.IsNullOrEmpty(line)).ToArray();
        var iline = 0;

        var machine = new Machine();

        String(@"Begin in state (\w).", out machine.state);
        Int(@"Perform a diagnostic checksum after (\d+) steps.", out machine.iterations);

        while (String(@"In state (\w):", out var state))
        {
            while (Int(@"If the current value is (\d):", out var read))
            {
                Int(@"- Write the value (\d).", out var write);
                String(@"- Move one slot to the (left|right).", out var dir);
                String(@" - Continue with state (\w).", out var newState);
                machine.prg[(state, read)] = (write, dir == "left" ? -1 : 1, newState);
            }
        }

        bool Int(string pattern, out int r)
        {
            r = 0;
            return String(pattern, out var st) && int.TryParse(st, out r);
        }

        bool String(string pattern, out string st)
        {
            st = null;
            if (iline >= lines.Length)
            {
                return false;
            }

            var m = Regex.Match(lines[iline], pattern);
            if (m.Success)
            {
                iline++;
                st = m.Groups[1].Value;
            }

            return m.Success;
        }

        return machine;
    }

    class Machine
    {
        public string state;
        public int iterations;

        public Dictionary<(string state, int read), (int write, int dir, string state)> prg =
            new Dictionary<(string, int), (int, int, string)>();
    }
}