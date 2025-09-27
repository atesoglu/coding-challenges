using FluentAssertions;

namespace LeetCode.Tests.Problems;

public class FindThePrefixCommonArrayOfTwoArrays
{
    [Theory]
    [InlineData(new[] { 1, 3, 2, 4 }, new[] { 3, 1, 2, 4 }, new[] { 0, 2, 3, 4 })]
    [InlineData(new[] { 2, 3, 1 }, new[] { 3, 1, 2 }, new[] { 0, 1, 3 })]
    public void FindThePrefixCommonArrayOfTwoArraysTest(int[] inputA, int[] inputB, int[] expected)
    {
        var output = Solve(inputA, inputB);

        output.Should().BeEquivalentTo(expected);
    }

    private static int[] Solve(int[] inputA, int[] inputB)
    {
        var result = new int[inputA.Length];
        var seenA = new HashSet<int>();
        var seenB = new HashSet<int>();
        var commonCount = 0;

        for (var index = 0; index < inputA.Length; index++)
        {
            var currentA = inputA[index];
            var currentB = inputB[index];

            // Update seen sets and the common count
            if (seenB.Contains(currentA))
                commonCount++;
            seenA.Add(currentA);

            if (seenA.Contains(currentB))
                commonCount++;
            seenB.Add(currentB);

            // Record the current common count
            result[index] = commonCount;
        }

        return result;
    }
}