using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D11;

[ChallengeName("Reactor")]
public class Y2025D11
{
    private readonly string _input = File.ReadAllText(@"Y2025\D11\Y2025D11-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var g = Parse(_input);
        PathCount(g, "you", "out").Should().Be(585);
    }

    [Fact]
    public void PartTwo()
    {
        var g = Parse(_input);

        var result =
            PathCount(g, "svr", "fft") * PathCount(g, "fft", "dac") * PathCount(g, "dac", "out") +
            PathCount(g, "svr", "dac") * PathCount(g, "dac", "fft") * PathCount(g, "fft", "out");

        result.Should().Be(349322478796032);
    }

    private static long PathCount(Dictionary<string, string[]> g, string from, string to, Dictionary<string, long>? cache = null)
    {
        cache ??= new Dictionary<string, long>();

        if (cache.TryGetValue(from, out var v))
        {
            return v;
        }

        v = from == to ? 1 : g.GetValueOrDefault(from, Array.Empty<string>()).Sum(next => PathCount(g, next, to, cache));

        return cache[from] = v;
    }

    private static Dictionary<string, string[]> Parse(string input) => input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
        .Select(line =>
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return new KeyValuePair<string, string[]>(parts[0].TrimEnd(':'), parts[1..]);
        })
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}