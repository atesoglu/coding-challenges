using System.Collections.Immutable;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2021.D09;

[ChallengeName("Smoke Basin")]
public class Y2021D09
{
    private readonly string _input = File.ReadAllText(@"Y2021\D09\Y2021D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var map = GetMap(_input);

        // find the 'low points' and return a hash computed from their heights:
        var output = GetLowPoints(map).Select(point => 1 + map[point]).Sum();

        output.Should().Be(462);
    }

    [Fact]
    public void PartTwo()
    {
        var map = GetMap(_input);

        // find the 3 biggest basins and return a hash computed from their size:
        var output = GetLowPoints(map)
            .Select(p => BasinSize(map, p))
            .OrderByDescending(basinSize => basinSize)
            .Take(3)
            .Aggregate(1, (m, basinSize) => m * basinSize);

        output.Should().Be(1397760);
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    private static ImmutableDictionary<Point, int> GetMap(string input)
    {
        // Normalize line endings to just "\n"
        input = input.Replace("\r\n", "\n").TrimEnd();

        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Point, int>(new Point(x, y), map[y][x] - '0')
        ).ToImmutableDictionary();
    }

    private static IEnumerable<Point> Neighbours(Point point) =>
        new[]
        {
            point with { y = point.y + 1 },
            point with { y = point.y - 1 },
            point with { x = point.x + 1 },
            point with { x = point.x - 1 },
        };

    private IEnumerable<Point> GetLowPoints(ImmutableDictionary<Point, int> map) =>
        from point in map.Keys
        // point is low if each of its neighbours is higher:
        where Neighbours(point).All(neighbour => map[point] < map.GetValueOrDefault(neighbour, 9))
        select point;

    private int BasinSize(ImmutableDictionary<Point, int> map, Point point)
    {
        // flood fill algorithm
        var filled = new HashSet<Point> { point };
        var queue = new Queue<Point>(filled);

        while (queue.Any())
        {
            foreach (var neighbour in Neighbours(queue.Dequeue()).Except(filled))
            {
                if (map.GetValueOrDefault(neighbour, 9) != 9)
                {
                    queue.Enqueue(neighbour);
                    filled.Add(neighbour);
                }
            }
        }

        return filled.Count;
    }
}

internal record Point(int x, int y);