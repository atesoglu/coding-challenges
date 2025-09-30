using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<(char currentKey, char nextKey, int depth), long>;
using Keypad = System.Collections.Generic.Dictionary<AdventOfCode.Tests.Y2024.D21.Vec2, char>;

namespace AdventOfCode.Tests.Y2024.D21;

internal record struct Vec2(int x, int y);

[ChallengeName("Keypad Conundrum")]
public class Y2024D21
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D21\Y2024D21-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateTotalCost(_lines, 2);

        output.Should().Be(231564);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CalculateTotalCost(_lines, 25);

        output.Should().Be(281212077733592);
    }

    private long CalculateTotalCost(IEnumerable<string> lines, int depth)
    {
        var keypad1 = ParseKeypad("789\n456\n123\n 0A");
        var keypad2 = ParseKeypad(" ^A\n<v>");
        var keypads = Enumerable.Repeat(keypad2, depth).Prepend(keypad1).ToArray();

        var cache = new Cache();
        var res = 0L;

        foreach (var line in lines)
        {
            var num = int.Parse(line[..^1]);
            res += num * EncodeKeys(line, keypads, cache);
        }

        return res;
    }

    private long EncodeKeys(string keys, Keypad[] keypads, Cache cache)
    {
        if (keypads.Length == 0)
        {
            return keys.Length;
        }
        else
        {
            var currentKey = 'A';
            var length = 0L;

            foreach (var nextKey in keys)
            {
                length += EncodeKey(currentKey, nextKey, keypads, cache);
                currentKey = nextKey;
            }
            Debug.Assert(currentKey == 'A', "The robot should point at the 'A' key");
            return length;
        }
    }

    private long EncodeKey(char currentKey, char nextKey, Keypad[] keypads, Cache cache) =>
        cache.GetOrAdd((currentKey, nextKey, keypads.Length), _ =>
        {
            var keypad = keypads[0];

            var currentPos = keypad.Single(kvp => kvp.Value == currentKey).Key;
            var nextPos = keypad.Single(kvp => kvp.Value == nextKey).Key;

            var dy = nextPos.y - currentPos.y;
            var vert = new string(dy < 0 ? 'v' : '^', Math.Abs(dy));

            var dx = nextPos.x - currentPos.x;
            var horiz = new string(dx < 0 ? '<' : '>', Math.Abs(dx));

            var cost = long.MaxValue;
            if (keypad[new Vec2(currentPos.x, nextPos.y)] != ' ')
            {
                cost = Math.Min(cost, EncodeKeys($"{vert}{horiz}A", keypads[1..], cache));
            }

            if (keypad[new Vec2(nextPos.x, currentPos.y)] != ' ')
            {
                cost = Math.Min(cost, EncodeKeys($"{horiz}{vert}A", keypads[1..], cache));
            }

            return cost;
        });

    private Keypad ParseKeypad(string keypad)
    {
        var lines = keypad.Split("\n");
        return (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Vec2, char>(new Vec2(x, -y), lines[y][x])
        ).ToDictionary();
    }
}