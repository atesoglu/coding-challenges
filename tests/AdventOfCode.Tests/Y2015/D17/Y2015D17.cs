using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D17;

[ChallengeName("No Such Thing as Too Much")]
public class Y2015D17
{
    private readonly int[] _containers = File.ReadAllLines(@"Y2015\D17\Y2015D17-input.txt", Encoding.UTF8)
        .Select(int.Parse)
        .ToArray();

    [Fact]
    public void PartOne()
    {
        var combinations = GetCombinations(_containers, 150).ToList();
        combinations.Count.Should().Be(1304);
    }

    [Fact]
    public void PartTwo()
    {
        var combinations = GetCombinations(_containers, 150).ToList();
        var minCount = combinations.Min(c => c.Count);
        var output = combinations.Count(c => c.Count == minCount);

        output.Should().Be(18);
    }

    private static IEnumerable<List<int>> GetCombinations(int[] containers, int target) => GetCombinationsRecursive(containers, 0, target, new List<int>());

    private static IEnumerable<List<int>> GetCombinationsRecursive(int[] containers, int index, int remaining, List<int> current)
    {
        if (remaining == 0)
        {
            yield return new List<int>(current);
            yield break;
        }

        if (index >= containers.Length || remaining < 0)
            yield break;

        // Include current container
        current.Add(containers[index]);
        foreach (var combination in GetCombinationsRecursive(containers, index + 1, remaining - containers[index], current))
            yield return combination;
        current.RemoveAt(current.Count - 1);

        // Exclude current container
        foreach (var combination in GetCombinationsRecursive(containers, index + 1, remaining, current))
            yield return combination;
    }
}