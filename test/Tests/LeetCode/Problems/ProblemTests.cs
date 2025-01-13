using FluentAssertions;
using Solutions.LeetCode.Problems;

namespace Tests.LeetCode.Problems;

public class ProblemTests
{
    [Theory]
    [InlineData(new[] { 2, 7, 11, 15 }, 9, new[] { 0, 1 })]
    [InlineData(new[] { 3, 2, 3 }, 6, new[] { 0, 2 })]
    public void TwoSumTest(int[] input, int target, int[] expected)
    {
        var output = TwoSum.Solve(input, target);

        output.Should().BeEquivalentTo(expected);
    }
}