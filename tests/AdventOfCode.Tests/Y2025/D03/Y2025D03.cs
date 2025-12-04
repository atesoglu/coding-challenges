using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D03;

[ChallengeName("Lobby")]
public class Y2025D03
{
    private readonly string _input = File.ReadAllText(@"Y2025\D03\Y2025D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = MaxJoltSum(_input, 2);

        output.Should().Be(17408);
    }

    [Fact]
    public void PartTwo()
    {
        var output = MaxJoltSum(_input, 12);

        output.Should().Be(172740584266849);
    }

    private static long MaxJoltSum(string input, int batteryCount)
    {
        long sum = 0;

        foreach (var bank in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            sum += MaxJolt(bank, batteryCount);

        return sum;
    }

    /// <summary>
    /// Selects the largest possible number by picking digits from left to right,
    /// ensuring that enough digits remain to fill all remaining battery slots.
    /// </summary>
    private static long MaxJolt(string bank, int batteryCount)
    {
        long result = 0;

        for (var remaining = batteryCount; remaining > 0; remaining--)
        {
            var takeUntil = bank.Length - (remaining - 1); // how far we are allowed to look
            var best = '0';
            var bestIndex = 0;

            // Find the highest digit within the allowed prefix
            for (var i = 0; i < takeUntil; i++)
            {
                if (bank[i] > best)
                {
                    best = bank[i];
                    bestIndex = i;
                }
            }

            result = result * 10 + (best - '0');

            // Move bank forward past the chosen digit
            bank = bank.Substring(bestIndex + 1);
        }

        return result;
    }
}