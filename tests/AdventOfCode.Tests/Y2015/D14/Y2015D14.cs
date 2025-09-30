using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D14;

[ChallengeName("Reindeer Olympics")]
public partial class Y2015D14
{
    private readonly IEnumerable<int>[] _reindeerDistanceSequences = File.ReadAllLines(@"Y2015\D14\Y2015D14-input.txt", Encoding.UTF8).Select(GenerateReindeerDistance).ToArray();

    [Fact]
    public void PartOne()
    {
        var output = SimulateRaceDistances(_reindeerDistanceSequences).Skip(2502).First().Max();

        output.Should().Be(2660);
    }

    [Fact]
    public void PartTwo()
    {
        var output = SimulateRacePoints(_reindeerDistanceSequences).Skip(2502).First().Max();

        output.Should().Be(1256);
    }

    private static IEnumerable<int[]> SimulateRaceDistances(IEnumerable<int>[] reindeers)
    {
        var reindeerEnumerators = reindeers.Select(r => r.GetEnumerator()).ToArray();

        while (true)
        {
            var currentDistances = new int[reindeers.Length];
            for (var i = 0; i < reindeerEnumerators.Length; i++)
            {
                reindeerEnumerators[i].MoveNext();
                currentDistances[i] = reindeerEnumerators[i].Current;
            }

            yield return currentDistances;
        }
    }

    private static IEnumerable<int[]> SimulateRacePoints(IEnumerable<int>[] reindeers)
    {
        var scores = new int[reindeers.Length];

        foreach (var distancesAtSecond in SimulateRaceDistances(reindeers))
        {
            var maxDistance = distancesAtSecond.Max();
            for (var i = 0; i < distancesAtSecond.Length; i++)
            {
                if (distancesAtSecond[i] == maxDistance)
                    scores[i]++;
            }

            yield return scores;
        }
    }

    private static IEnumerable<int> GenerateReindeerDistance(string line)
    {
        var match = DistanceRegex().Match(line);
        var speed = int.Parse(match.Groups[2].Value);
        var flyDuration = int.Parse(match.Groups[3].Value);
        var restDuration = int.Parse(match.Groups[4].Value);

        var currentPhaseTime = 0;
        var distance = 0;
        var isFlying = true;

        while (true)
        {
            if (isFlying)
                distance += speed;

            currentPhaseTime++;

            if ((isFlying && currentPhaseTime == flyDuration) ||
                (!isFlying && currentPhaseTime == restDuration))
            {
                isFlying = !isFlying;
                currentPhaseTime = 0;
            }

            yield return distance;
        }
    }

    [GeneratedRegex(@"(.*) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.")]
    private static partial Regex DistanceRegex();
}