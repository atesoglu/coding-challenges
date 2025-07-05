namespace LeetCode.Tests.Problems;

public abstract class FindThePrefixCommonArrayOfTwoArrays
{
    public static int[] Solve(int[] inputA, int[] inputB)
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