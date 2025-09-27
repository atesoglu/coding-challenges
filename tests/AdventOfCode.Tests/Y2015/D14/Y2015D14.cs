using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D14;

[ChallengeName("Reindeer Olympics")]
public class Y2015D14
{
    private readonly IEnumerable<int>[] _reindeerDistances = File.ReadAllLines(@"Y2015\D14\Y2015D14-input.txt", Encoding.UTF8).Select(GenerateDistanceSequence).ToArray();

    [Fact]
    public void PartOne()
    {
        var output = DistanceSteps(_reindeerDistances).Skip(2502).First().Max();
        output.Should().Be(2660);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PointsSteps(_reindeerDistances).Skip(2502).First().Max();
        output.Should().Be(1256);
    }

    private static IEnumerable<int[]> DistanceSteps(IEnumerable<int>[] reindeers)
    {
        var enumerators = reindeers.Select(r => r.GetEnumerator()).ToArray();
        while (true)
        {
            var distances = new int[reindeers.Length];
            for (var i = 0; i < enumerators.Length; i++)
            {
                enumerators[i].MoveNext();
                distances[i] = enumerators[i].Current;
            }

            yield return distances;
        }
    }

    private IEnumerable<int[]> PointsSteps(IEnumerable<int>[] reindeers)
    {
        var points = new int[reindeers.Length];
        foreach (var step in DistanceSteps(reindeers))
        {
            var maxDistance = step.Max();
            for (var i = 0; i < step.Length; i++)
            {
                if (step[i] == maxDistance)
                    points[i]++;
            }

            yield return points;
        }
    }

    private static IEnumerable<int> GenerateDistanceSequence(string line)
    {
        var m = Regex.Match(line, @"(.*) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.");
        var speed = int.Parse(m.Groups[2].Value);
        var flyTime = int.Parse(m.Groups[3].Value);
        var restTime = int.Parse(m.Groups[4].Value);

        var elapsedTime = 0;
        var distance = 0;
        var isFlying = true;

        while (true)
        {
            if (isFlying)
                distance += speed;

            elapsedTime++;

            if ((isFlying && elapsedTime == flyTime) || (!isFlying && elapsedTime == restTime))
            {
                isFlying = !isFlying;
                elapsedTime = 0;
            }

            yield return distance;
        }
    }
}