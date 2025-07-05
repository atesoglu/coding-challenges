namespace LeetCode.Tests.Problems;

public abstract class TwoSum
{
    public static int[] Solve(int[] nums, int target)
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