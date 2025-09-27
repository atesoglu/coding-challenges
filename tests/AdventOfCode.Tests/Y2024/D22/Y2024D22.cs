using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D22;

[ChallengeName("Monkey Market")]
public class Y2024D22
{
    private readonly string _input = File.ReadAllText(@"Y2024\D22\Y2024D22-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetNums(_input).Select(x => (long)SecretNumbers(x).Last()).Sum();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        // create a dictionary of all buying options then select the one with the most banana:

        var buyingOptions = new Dictionary<(int, int, int, int), int>();

        foreach (var num in GetNums(_input))
        {
            var optionsBySeller = BuyingOptions(num);
            foreach (var seq in optionsBySeller.Keys)
            {
                buyingOptions[seq] = buyingOptions.GetValueOrDefault(seq) + optionsBySeller[seq];
            }
        }

        var output = buyingOptions.Values.Max();

        output.Should().Be(0);
    }

    Dictionary<(int, int, int, int), int> BuyingOptions(int seed)
    {
        var bananasSold = Bananas(seed).ToArray();
        var buyingOptions = new Dictionary<(int, int, int, int), int>();

        // a sliding window of 5 elements over the sold bananas defines the sequence the monkey 
        // will recognize. add the first occurrence of each sequence to the buyingOptions dictionary 
        // with the corresponding banana count
        var diff = Diff(bananasSold);
        for (var i = 0; i < bananasSold.Length - 4; i++)
        {
            var seq = (diff[i], diff[i + 1], diff[i + 2], diff[i + 3]);
            if (!buyingOptions.ContainsKey(seq))
            {
                buyingOptions[seq] = bananasSold[i + 4];
            }
        }

        return buyingOptions;
    }

    int[] Bananas(int seed) => SecretNumbers(seed).Select(n => n % 10).ToArray();

    int[] Diff(IEnumerable<int> x) => x.Zip(x.Skip(1)).Select(p => p.Second - p.First).ToArray();

    IEnumerable<int> SecretNumbers(int seed)
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

    IEnumerable<int> GetNums(string input) => input.Split("\n").Select(int.Parse);
}