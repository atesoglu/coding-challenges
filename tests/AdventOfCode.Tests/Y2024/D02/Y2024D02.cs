using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D02;

[ChallengeName("Red-Nosed Reports")]
public class Y2024D02
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D02\Y2024D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ParseSamples(_lines).Count(IsMonotonicWithStepWithinRange);

        output.Should().Be(639);
    }

    [Fact]
    public void PartTwo()
    {
        var output = ParseSamples(_lines).Count(samples => GenerateSingleRemovalVariations(samples).Any(IsMonotonicWithStepWithinRange));

        output.Should().Be(674);
    }

    IEnumerable<int[]> ParseSamples(IEnumerable<string> lines) =>
        lines.Select(line => line.Split(" ").Select(int.Parse).ToArray());

    IEnumerable<int[]> GenerateSingleRemovalVariations(int[] samples) =>
        Enumerable.Range(0, samples.Length + 1)
            .Select(i => samples.Take(i - 1).Concat(samples.Skip(i)).ToArray());

    bool IsMonotonicWithStepWithinRange(int[] samples)
    {
        var consecutivePairs = Enumerable.Zip(samples, samples.Skip(1)).ToArray();
        var nonDecreasingValid = consecutivePairs.All(pair =>
            1 <= pair.Second - pair.First && pair.Second - pair.First <= 3);
        var nonIncreasingValid = consecutivePairs.All(pair =>
            1 <= pair.First - pair.Second && pair.First - pair.Second <= 3);
        return nonDecreasingValid || nonIncreasingValid;
    }
}