using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D08;

[ChallengeName("Playground")]
public class Y2025D08
{
    private readonly string _input = File.ReadAllText(@"Y2025\D08\Y2025D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var points = Parse(_input);
        var setOf = points.ToDictionary(p => p, p => new HashSet<Point>([p]));
        foreach (var (a, b) in GetOrderedPairs(points).Take(1000))
        {
            if (setOf[a] != setOf[b])
            {
                Connect(a, b, setOf);
            }
        }

        var output = setOf.Values.Distinct()
            .OrderByDescending(set => set.Count)
            .Take(3)
            .Aggregate(1, (a, b) => a * b.Count);

        output.Should().Be(352584);
    }

    [Fact]
    public void PartTwo()
    {
        var points = Parse(_input);
        var componentCount = points.Length;
        var setOf = points.ToDictionary(p => p, p => new HashSet<Point>([p]));
        var res = 0m;
        foreach (var (a, b) in GetOrderedPairs(points).TakeWhile(_ => componentCount > 1))
        {
            if (setOf[a] != setOf[b])
            {
                Connect(a, b, setOf);
                res = a.x * b.x;
                componentCount--;
            }
        }

        var output = res;

        output.Should().Be(9617397716);
    }

    void Connect(Point a, Point b, Dictionary<Point, HashSet<Point>> setOf)
    {
        setOf[a].UnionWith(setOf[b]);
        foreach (var p in setOf[b])
        {
            setOf[p] = setOf[a];
        }
    }

    IEnumerable<(Point a, Point b)> GetOrderedPairs(Point[] points) =>
        from a in points
        from b in points
        where (a.x, a.y, a.z).CompareTo((b.x, b.y, b.z)) < 0
        orderby Metric(a, b)
        select (a, b);

    decimal Metric(Point a, Point b) =>
        (a.x - b.x) * (a.x - b.x) +
        (a.y - b.y) * (a.y - b.y) +
        (a.z - b.z) * (a.z - b.z);

    Point[] Parse(string input) => (
        from line in input.Split("\n")
        let parts = line.Split(",").Select(int.Parse).ToArray()
        select new Point(parts[0], parts[1], parts[2])
    ).ToArray();


    record Point(decimal x, decimal y, decimal z);
}