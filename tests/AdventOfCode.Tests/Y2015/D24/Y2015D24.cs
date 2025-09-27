using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D24;

[ChallengeName("It Hangs in the Balance")]
public class Y2015D24
{
    private readonly int[] _weights = File.ReadAllLines(@"Y2015\D24\Y2015D24-input.txt", Encoding.UTF8)
        .Select(int.Parse)
        .ToArray();

    [Fact]
    public void PartOne()
    {
        var output = Solve(_weights, 3);

        output.Should().Be(10439961859);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_weights, 4);

        output.Should().Be(72050269);
    }

    private static long Solve(int[] weights, int groups)
    {
        var target = weights.Sum() / groups;

        // Try subsets in increasing size until we find valid ones
        for (var size = 1; size <= weights.Length; size++)
        {
            var validGroups = Subsets(weights, size, target).ToList();
            if (validGroups.Any())
            {
                return validGroups.Min(group => group.Aggregate(1L, (a, b) => a * b));
            }
        }

        throw new InvalidOperationException("No solution found.");
    }

    private static IEnumerable<List<int>> Subsets(int[] nums, int count, int target, int start = 0)
    {
        if (target == 0 && count == 0)
        {
            yield return new List<int>();
            yield break;
        }

        if (count == 0 || target < 0) yield break;

        for (int i = start; i < nums.Length; i++)
        {
            foreach (var subset in Subsets(nums, count - 1, target - nums[i], i + 1))
            {
                subset.Add(nums[i]);
                yield return subset;
            }
        }
    }
}