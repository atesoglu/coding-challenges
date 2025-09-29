using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using Region = System.Collections.Generic.HashSet<System.Numerics.Complex>;

namespace AdventOfCode.Tests.Y2024.D12;

[ChallengeName("Garden Groups")]
public class Y2024D12
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D12\Y2024D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateFencePrice(_lines, CountEdges);

        output.Should().Be(1352976);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CalculateFencePrice(_lines, CountCorners);

        output.Should().Be(808796);
    }


    Complex Up = Complex.ImaginaryOne;
    Complex Down = -Complex.ImaginaryOne;
    Complex Left = -1;
    Complex Right = 1;

    int CalculateFencePrice(IEnumerable<string> lines, MeasurePerimeter measure)
    {
        var regions = BuildRegions(lines);
        var totalCost = 0;
        foreach (var region in regions.Values.Distinct())
        {
            var perimeter = 0;
            foreach (var point in region)
            {
                perimeter += measure(regions, point);
            }

            totalCost += region.Count() * perimeter;
        }

        return totalCost;
    }

    delegate int MeasurePerimeter(Dictionary<Complex, Region> map, Complex pt);

    int CountEdges(Dictionary<Complex, Region> map, Complex pt)
    {
        var edgeCount = 0;
        var region = map[pt];
        foreach (var direction in new[] { Right, Down, Left, Up })
        {
            if (map.GetValueOrDefault(pt + direction) != region)
            {
                edgeCount++;
            }
        }

        return edgeCount;
    }

    int CountCorners(Dictionary<Complex, Region> map, Complex pt)
    {
        var cornerCount = 0;
        var region = map[pt];

        foreach (var (direction1, direction2) in new[] { (Up, Right), (Right, Down), (Down, Left), (Left, Up) })
        {
            if (map.GetValueOrDefault(pt + direction1) != region &&
                map.GetValueOrDefault(pt + direction2) != region)
            {
                cornerCount++;
            }

            if (map.GetValueOrDefault(pt + direction1) == region &&
                map.GetValueOrDefault(pt + direction2) == region &&
                map.GetValueOrDefault(pt + direction1 + direction2) != region)
            {
                cornerCount++;
            }
        }

        return cornerCount;
    }

    Dictionary<Complex, Region> BuildRegions(IEnumerable<string> lines)
    {
        var rowArray = lines.ToArray();
        var garden = (
            from y in Enumerable.Range(0, rowArray.Length)
            from x in Enumerable.Range(0, rowArray[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, rowArray[y][x])
        ).ToDictionary();

        var regions = new Dictionary<Complex, Region>();
        var unprocessedPositions = garden.Keys.ToHashSet();
        while (unprocessedPositions.Any())
        {
            var startPoint = unprocessedPositions.First();
            var region = new Region { startPoint };

            var queue = new Queue<Complex>();
            queue.Enqueue(startPoint);

            var plantType = garden[startPoint];

            while (queue.Any())
            {
                var point = queue.Dequeue();
                regions[point] = region;
                unprocessedPositions.Remove(point);
                foreach (var direction in new[] { Up, Down, Left, Right })
                {
                    if (!region.Contains(point + direction) && garden.GetValueOrDefault(point + direction) == plantType)
                    {
                        region.Add(point + direction);
                        queue.Enqueue(point + direction);
                    }
                }
            }
        }

        return regions;
    }
}