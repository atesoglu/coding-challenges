using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D13;

[ChallengeName("Knights of the Dinner Table")]
public partial class Y2015D13
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D13\Y2015D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateTotalHappinessScores(false).Max();

        output.Should().Be(664);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CalculateTotalHappinessScores(true).Max();

        output.Should().Be(640);
    }

    private IEnumerable<int> CalculateTotalHappinessScores(bool includeSelf)
    {
        var happinessMap = new Dictionary<(string, string), int>();

        foreach (var line in _lines)
        {
            var match = HappinessRegex().Match(line);
            var person = match.Groups[1].Value;
            var neighbor = match.Groups[4].Value;
            var happinessChange = int.Parse(match.Groups[3].Value) * (match.Groups[2].Value == "gain" ? 1 : -1);

            if (!happinessMap.ContainsKey((person, neighbor)))
            {
                happinessMap[(person, neighbor)] = 0;
                happinessMap[(neighbor, person)] = 0;
            }

            happinessMap[(person, neighbor)] += happinessChange;
            happinessMap[(neighbor, person)] += happinessChange;
        }

        var guests = happinessMap.Keys.Select(k => k.Item1).Distinct().ToList();
        if (includeSelf)
        {
            guests.Add("me");
        }

        return GeneratePermutations(
            guests.ToArray()).Select(seatingOrder => seatingOrder.Zip(seatingOrder.Skip(1).Append(seatingOrder[0]),
            (guest, neighbor) => happinessMap.GetValueOrDefault((guest, neighbor), 0)).Sum());
    }

    private static IEnumerable<IList<T>> GeneratePermutations<T>(IList<T> items)
    {
        if (items.Count == 0)
        {
            yield return Array.Empty<T>();
            yield break;
        }

        for (var i = 0; i < items.Count; i++)
        {
            var current = items[i];
            var remaining = items.Where((_, index) => index != i).ToList();

            foreach (var perm in GeneratePermutations(remaining))
            {
                yield return new[] { current }.Concat(perm).ToList();
            }
        }
    }


    [GeneratedRegex("(.*) would (.*) (.*) happiness units by sitting next to (.*).")]
    private static partial Regex HappinessRegex();
}