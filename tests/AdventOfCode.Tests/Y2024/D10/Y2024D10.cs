using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Tests.Y2024.D10;

[ChallengeName("Hoof It")]
public class Y2024D10
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D10\Y2024D10-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetAllTrails(_lines).Sum(t => t.Value.Distinct().Count());

        output.Should().Be(550);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetAllTrails(_lines).Sum(t => t.Value.Count());

        output.Should().Be(1255);
    }


    private Complex Up = Complex.ImaginaryOne;
    private Complex Down = -Complex.ImaginaryOne;
    private Complex Left = -1;
    private Complex Right = 1;

    private Dictionary<Complex, List<Complex>> GetAllTrails(IEnumerable<string> lines)
    {
        var map = BuildMap(lines);
        return GetTrailHeads(map).ToDictionary(t => t, t => GetTrailsFrom(map, t));
    }

    private IEnumerable<Complex> GetTrailHeads(Map map) => map.Keys.Where(pos => map[pos] == '0');

    private List<Complex> GetTrailsFrom(Map map, Complex trailHead)
    {
        // standard floodfill algorithm using a queue
        var positions = new Queue<Complex>();
        positions.Enqueue(trailHead);
        var trails = new List<Complex>();
        while (positions.Any())
        {
            var point = positions.Dequeue();
            if (map[point] == '9')
            {
                trails.Add(point);
            }
            else
            {
                foreach (var dir in new[] { Up, Down, Left, Right })
                {
                    if (map.GetValueOrDefault(point + dir) == map[point] + 1)
                    {
                        positions.Enqueue(point + dir);
                    }
                }
            }
        }

        return trails;
    }

    private Map BuildMap(IEnumerable<string> lines)
    {
        var rowArray = lines.ToArray();
        return (
            from y in Enumerable.Range(0, rowArray.Length)
            from x in Enumerable.Range(0, rowArray[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, rowArray[y][x])
        ).ToImmutableDictionary();
    }
}