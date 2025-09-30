using System.Text;
using FluentAssertions;
using System.Linq;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<(string, int), long>;

namespace AdventOfCode.Tests.Y2024.D11;

[ChallengeName("Plutonian Pebbles")]
public class Y2024D11
{
    private readonly string _input = File.ReadAllText(@"Y2024\D11\Y2024D11-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateStoneCount(_input, 25);

        output.Should().Be(218079);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CalculateStoneCount(_input, 75);

        output.Should().Be(259755538429618);
    }

    private long CalculateStoneCount(string input, int blinks)
    {
        var cache = new Cache();
        return input.Split(" ").Sum(number => EvaluateEngraving(long.Parse(number), blinks, cache));
    }

    private static long EvaluateEngraving(long number, int blinks, Cache cache) =>
        cache.GetOrAdd((number.ToString(), blinks), key =>
            key switch
            {
                (_, 0) => 1,

                ("0", _) =>
                    EvaluateEngraving(1, blinks - 1, cache),

                (var stringValue, _) when stringValue.Length % 2 == 0 =>
                    EvaluateEngraving(long.Parse(stringValue[0..(stringValue.Length / 2)]), blinks - 1, cache) +
                    EvaluateEngraving(long.Parse(stringValue[(stringValue.Length / 2)..]), blinks - 1, cache),

                _ =>
                    EvaluateEngraving(2024 * number, blinks - 1, cache)
            }
        );
}