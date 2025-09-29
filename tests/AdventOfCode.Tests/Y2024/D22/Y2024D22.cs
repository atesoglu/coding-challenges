using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D22;

[ChallengeName("Monkey Market")]
public class Y2024D22
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D22\Y2024D22-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ParseSeeds(_lines).Select(x => (long)GenerateSecretNumbers(x).Last()).Sum();

        output.Should().Be(20215960478);
    }

    [Fact]
    public void PartTwo()
    {
        var buyingOptions = new Dictionary<(int, int, int, int), int>();

        foreach (var seed in ParseSeeds(_lines))
        {
            var optionsBySeller = BuildBuyingOptions(seed);
            foreach (var sequence in optionsBySeller.Keys)
            {
                buyingOptions[sequence] = buyingOptions.GetValueOrDefault(sequence) + optionsBySeller[sequence];
            }
        }

        var output = buyingOptions.Values.Max();

        output.Should().Be(2221);
    }

    Dictionary<(int, int, int, int), int> BuildBuyingOptions(int seed)
    {
        var bananasSold = GetBananasPerStep(seed).ToArray();
        var buyingOptions = new Dictionary<(int, int, int, int), int>();

        var differences = Diff(bananasSold);
        for (var i = 0; i < bananasSold.Length - 4; i++)
        {
            var sequence = (differences[i], differences[i + 1], differences[i + 2], differences[i + 3]);
            if (!buyingOptions.ContainsKey(sequence))
            {
                buyingOptions[sequence] = bananasSold[i + 4];
            }
        }

        return buyingOptions;
    }

    int[] GetBananasPerStep(int seed) => GenerateSecretNumbers(seed).Select(n => n % 10).ToArray();

    int[] Diff(IEnumerable<int> x) => x.Zip(x.Skip(1)).Select(p => p.Second - p.First).ToArray();

    IEnumerable<int> GenerateSecretNumbers(int seed)
    {
        var mixAndPrune = (int a, int b) => (a ^ b) & 0xffffff;

        yield return seed;
        for (var i = 0; i < 2000; i++)
        {
            seed = mixAndPrune(seed, seed << 6);
            seed = mixAndPrune(seed, seed >> 5);
            seed = mixAndPrune(seed, seed << 11);
            yield return seed;
        }
    }

    IEnumerable<int> ParseSeeds(IEnumerable<string> lines) => lines.Select(int.Parse);
}