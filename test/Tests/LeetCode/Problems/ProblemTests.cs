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

    [Theory]
    [InlineData(new[] { 1, 3, 2, 4 }, new[] { 3, 1, 2, 4 }, new[] { 0, 2, 3, 4 })]
    [InlineData(new[] { 2, 3, 1 }, new[] { 3, 1, 2 }, new[] { 0, 1, 3 })]
    public void FindThePrefixCommonArrayOfTwoArraysTest(int[] inputA, int[] inputB, int[] expected)
    {
        var output = FindThePrefixCommonArrayOfTwoArrays.Solve(inputA, inputB);

        output.Should().BeEquivalentTo(expected);
    }
}