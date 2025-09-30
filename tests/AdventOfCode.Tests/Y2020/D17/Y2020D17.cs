using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D17;

[ChallengeName("Conway Cubes")]
public class Y2020D17
{
    private readonly string _input = File.ReadAllText(@"Y2020\D17\Y2020D17-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var ds = (from dx in new[] { -1, 0, 1 }
            from dy in new[] { -1, 0, 1 }
            from dz in new[] { -1, 0, 1 }
            where dx != 0 || dy != 0 || dz != 0
            select (dx, dy, dz)).ToArray();

        var output = Solve(
            _input,
            (x, y) => (x: x, y: y, z: 0),
            (p) => ds.Select(d => (p.x + d.dx, p.y + d.dy, p.z + d.dz)));

        output.Should().Be(372);
    }

    [Fact]
    public void PartTwo()
    {
        var ds = (from dx in new[] { -1, 0, 1 }
            from dy in new[] { -1, 0, 1 }
            from dz in new[] { -1, 0, 1 }
            from dw in new[] { -1, 0, 1 }
            where dx != 0 || dy != 0 || dz != 0 || dw != 0
            select (dx, dy, dz, dw)).ToArray();

        var output = Solve(
            _input,
            (x, y) => (x: x, y: y, z: 0, w: 0),
            (p) => ds.Select(d => (p.x + d.dx, p.y + d.dy, p.z + d.dz, p.w + d.dw)));

        output.Should().Be(1896);
    }

    private static int Solve<T>(string input, Func<int, int, T> create, Func<T, IEnumerable<T>> neighbours)
    {
        var lines = input.Split("\n");
        var (width, height) = (lines[0].Length, lines.Length);
        var activePoints = new HashSet<T>(
            from x in Enumerable.Range(0, width)
            from y in Enumerable.Range(0, height)
            where lines[y][x] == '#'
            select create(x, y)
        );

        for (var i = 0; i < 6; i++)
        {
            var newActivePoints = new HashSet<T>();
            var inactivePoints = new Dictionary<T, int>();

            foreach (var point in activePoints)
            {
                var activeNeighbours = 0;
                foreach (var neighbour in neighbours(point))
                {
                    if (activePoints.Contains(neighbour))
                    {
                        activeNeighbours++;
                    }
                    else
                    {
                        inactivePoints[neighbour] = inactivePoints.GetValueOrDefault(neighbour) + 1;
                    }
                }

                if (activeNeighbours == 2 || activeNeighbours == 3)
                {
                    newActivePoints.Add(point);
                }
            }

            foreach (var (point, activeNeighbours) in inactivePoints)
            {
                if (activeNeighbours == 3)
                {
                    newActivePoints.Add(point);
                }
            }

            activePoints = newActivePoints;
        }

        return activePoints.Count();
    }
}