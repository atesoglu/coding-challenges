using System.Numerics;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D20;

[ChallengeName("Race Condition")]
public class Y2024D20
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D20\Y2024D20-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateCheatScore(_lines, 2);

        output.Should().Be(1293);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CalculateCheatScore(_lines, 20);

        output.Should().Be(977747);
    }

    private int CalculateCheatScore(IEnumerable<string> lines, int maxDistance)
    {
        var path = BuildPathFromGoalToStart(lines);
        var indices = Enumerable.Range(0, path.Length).ToArray();

        var calculateCheatsFromIndex = (int index) => (
            from previousIndex in indices[0..index]
            let distance = CalculateManhattanDistance(path[index], path[previousIndex])
            let timeSaving = index - (previousIndex + distance)
            where distance <= maxDistance && timeSaving >= 100
            select 1
        ).Sum();

        return indices.AsParallel().Select(calculateCheatsFromIndex).Sum();
    }

    private static int CalculateManhattanDistance(Complex positionA, Complex positionB) =>
        (int)(Math.Abs(positionA.Imaginary - positionB.Imaginary) + Math.Abs(positionA.Real - positionB.Real));

    private static Complex[] BuildPathFromGoalToStart(IEnumerable<string> lines)
    {
        var rowArray = lines.ToArray();
        var map = (
            from y in Enumerable.Range(0, rowArray.Length)
            from x in Enumerable.Range(0, rowArray[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Complex.ImaginaryOne, rowArray[y][x])
        ).ToDictionary();

        Complex[] directions = [-1, 1, Complex.ImaginaryOne, -Complex.ImaginaryOne];

        var startPosition = map.Keys.Single(key => map[key] == 'S');
        var goalPosition = map.Keys.Single(key => map[key] == 'E');

        var (previousPosition, currentPosition) = ((Complex?)null, goalPosition);
        var path = new List<Complex> { currentPosition };

        while (currentPosition != startPosition)
        {
            var direction = directions.Single(dir => map[currentPosition + dir] != '#' && currentPosition + dir != previousPosition);
            (previousPosition, currentPosition) = (currentPosition, currentPosition + direction);
            path.Add(currentPosition);
        }

        return path.ToArray();
    }
}