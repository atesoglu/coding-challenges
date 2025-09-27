using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D02;

[ChallengeName("Red-Nosed Reports")]
public class Y2024D02
{
    private readonly string _input = File.ReadAllText(@"Y2024\D02\Y2024D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ParseSamples(_input).Count(Valid);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = ParseSamples(_input).Count(samples => Attenuate(samples).Any(Valid));

        output.Should().Be(0);
    }


    IEnumerable<int[]> ParseSamples(string input) =>
        from line in input.Split("\n")
        let samples = line.Split(" ").Select(int.Parse)
        select samples.ToArray();

    // Generates all possible variations of the input sequence by omitting 
    // either zero or one element from it.
    IEnumerable<int[]> Attenuate(int[] samples) =>
        from i in Enumerable.Range(0, samples.Length + 1)
        let before = samples.Take(i - 1)
        let after = samples.Skip(i)
        select Enumerable.Concat(before, after).ToArray();

    // Checks the monothinicity condition by examining consecutive elements
    bool Valid(int[] samples)
    {
        var pairs = Enumerable.Zip(samples, samples.Skip(1));
        return
            pairs.All(p => 1 <= p.Second - p.First && p.Second - p.First <= 3) ||
            pairs.All(p => 1 <= p.First - p.Second && p.First - p.Second <= 3);
    }
}