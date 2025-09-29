using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D18;

[ChallengeName("RAM Run")]
public class Y2024D18
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D18\Y2024D18-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateShortestDistance(ParseBlockPositions(_lines).Take(1024));

        output.Should().Be(438);
    }

    [Fact]
    public void PartTwo()
    {
        var blockPositions = ParseBlockPositions(_lines);
        var (lowIndex, highIndex) = (0, blockPositions.Length);
        while (highIndex - lowIndex > 1)
        {
            var middleIndex = (lowIndex + highIndex) / 2;
            if (CalculateShortestDistance(blockPositions.Take(middleIndex)) == null)
            {
                highIndex = middleIndex;
            }
            else
            {
                lowIndex = middleIndex;
            }
        }

        var output = $"{blockPositions[lowIndex].Real},{blockPositions[lowIndex].Imaginary}";

        output.Should().Be("26,22");
    }

    int? CalculateShortestDistance(IEnumerable<Complex> blocks)
    {
        var gridSize = 70;
        var startPosition = Complex.Zero;
        var goalPosition = gridSize + gridSize * Complex.ImaginaryOne;
        var blockedPositions = blocks.Concat(new[] { startPosition }).ToHashSet();

        var queue = new PriorityQueue<Complex, int>();
        queue.Enqueue(startPosition, 0);
        while (queue.TryDequeue(out var currentPosition, out var distance))
        {
            if (currentPosition == goalPosition)
            {
                return distance;
            }

            foreach (var direction in new[] { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne })
            {
                var nextPosition = currentPosition + direction;
                if (!blockedPositions.Contains(nextPosition) &&
                    0 <= nextPosition.Imaginary && nextPosition.Imaginary <= gridSize &&
                    0 <= nextPosition.Real && nextPosition.Real <= gridSize)
                {
                    queue.Enqueue(nextPosition, distance + 1);
                    blockedPositions.Add(nextPosition);
                }
            }
        }

        return null;
    }

    Complex[] ParseBlockPositions(IEnumerable<string> lines) => (
        from line in lines
        let numbers = Regex.Matches(line, @"\d+").Select(match => int.Parse(match.Value)).ToArray()
        select numbers[0] + numbers[1] * Complex.ImaginaryOne
    ).ToArray();
}