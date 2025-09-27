using FluentAssertions;

namespace LeetCode.Tests.Problems;

public class TwoSum
{
    [Theory]
    [InlineData(new[] { 2, 7, 11, 15 }, 9, new[] { 0, 1 })]
    [InlineData(new[] { 3, 2, 3 }, 6, new[] { 0, 2 })]
    public void TwoSumTest(int[] input, int target, int[] expected)
    {
        var output = Solve(input, target);

        output.Should().BeEquivalentTo(expected);
    }

    private static int[] Solve(int[] nums, int target)
    {
        var seenNumbers = new Dictionary<int, int>();

        for (var index = 0; index < nums.Length; index++)
        {
            var complement = target - nums[index];

            if (seenNumbers.TryGetValue(complement, out var number))
            {
                return [number, index];
            }

            if (!seenNumbers.ContainsKey(nums[index]))
            {
                seenNumbers[nums[index]] = index;
            }
        }

        return [];
    }
}