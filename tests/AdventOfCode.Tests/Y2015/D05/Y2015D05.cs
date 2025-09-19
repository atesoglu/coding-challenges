namespace AdventOfCode.Tests.Y2015.D05;

[ChallengeName("Doesn't He Have Intern-Elves For This?")]
public class Y2015D05
{
    private static readonly HashSet<char> Vowels = ['a', 'e', 'i', 'o', 'u'];
    private static readonly string[] Forbidden = ["ab", "cd", "pq", "xy"];

    public bool PartOne(string input)
    {
        var vowelCount = 0;
        var hasDouble = false;

        for (var i = 0; i < input.Length; i++)
        {
            if (Vowels.Contains(input[i]))
            {
                vowelCount++;
            }

            if (i > 0 && input[i] == input[i - 1])
            {
                hasDouble = true;
            }
        }

        var hasForbidden = Forbidden.Any(input.Contains);

        return vowelCount >= 3 && hasDouble && !hasForbidden;
    }

    public bool PartTwo(string input)
    {
        var pairs = new Dictionary<string, int>();
        var hasPair = false;
        var repeats = false;

        for (var i = 0; i < input.Length - 1; i++)
        {
            var pair = input.Substring(i, 2);
            if (pairs.TryGetValue(pair, out var lastIndex))
            {
                hasPair = i - lastIndex >= 2 || hasPair;
            }
            else
            {
                pairs[pair] = i;
            }

            repeats = i < input.Length - 2 && input[i] == input[i + 2] || repeats;
        }

        return hasPair && repeats;
    }
}