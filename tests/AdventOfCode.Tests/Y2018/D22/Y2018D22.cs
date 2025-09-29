using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D22;

[ChallengeName("Mode Maze")]
public class Y2018D22
{
    private readonly string _input = File.ReadAllText(@"Y2018\D22\Y2018D22-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (targetX, targetY, regionType) = Parse(_input);
        var riskLevel = 0;
        for (var y = 0; y <= targetY; y++)
        {
            for (var x = 0; x <= targetX; x++)
            {
                riskLevel += (int)regionType(x, y);
            }
        }

        var output = riskLevel;

        output.Should().Be(7743);
    }

    [Fact]
    public void PartTwo()
    {
        var output = 0;


        var (targetX, targetY, regionType) = Parse(_input);
        var q = new PQueue<((int x, int y) pos, Tool tool, int t)>();
        var seen = new HashSet<((int x, int y), Tool tool)>();

        IEnumerable<((int x, int y) pos, Tool tool, int dt)> Neighbours((int x, int y) pos, Tool tool)
        {
            yield return regionType(pos.x, pos.y) switch
            {
                RegionType.Rocky => (pos, tool == Tool.ClimbingGear ? Tool.Torch : Tool.ClimbingGear, 7),
                RegionType.Narrow => (pos, tool == Tool.Torch ? Tool.Nothing : Tool.Torch, 7),
                RegionType.Wet => (pos, tool == Tool.ClimbingGear ? Tool.Nothing : Tool.ClimbingGear, 7),
                _ => throw new ArgumentException()
            };

            foreach (var dx in new[] { -1, 0, 1 })
            {
                foreach (var dy in new[] { -1, 0, 1 })
                {
                    if (Math.Abs(dx) + Math.Abs(dy) != 1)
                    {
                        continue;
                    }

                    var posNew = (x: pos.x + dx, y: pos.y + dy);
                    if (posNew.x < 0 || posNew.y < 0)
                    {
                        continue;
                    }

                    switch (regionType(posNew.x, posNew.y))
                    {
                        case RegionType.Rocky when tool == Tool.ClimbingGear || tool == Tool.Torch:
                        case RegionType.Narrow when tool == Tool.Torch || tool == Tool.Nothing:
                        case RegionType.Wet when tool == Tool.ClimbingGear || tool == Tool.Nothing:
                            yield return (posNew, tool, 1);
                            break;
                    }
                }
            }
        }

        q.Enqueue(0, ((0, 0), Tool.Torch, 0));

        while (q.Any())
        {
            var state = q.Dequeue();
            var (pos, tool, t) = state;

            if (pos.x == targetX && pos.y == targetY && tool == Tool.Torch)
            {
                output = t;
                break;
            }

            var hash = (pos, tool);
            if (seen.Contains(hash))
            {
                continue;
            }

            seen.Add(hash);

            foreach (var (newPos, newTool, dt) in Neighbours(pos, tool))
            {
                q.Enqueue(
                    t + dt + Math.Abs(newPos.x - targetX) + Math.Abs(newPos.y - targetY),
                    (newPos, newTool, t + dt)
                );
            }
        }

        output.Should().Be(1029);
    }

    (int targetX, int targetY, Func<int, int, RegionType> regionType) Parse(string input)
    {
        var lines = input.Split("\n");
        var depth = Regex.Matches(lines[0], @"\d+").Select(x => int.Parse(x.Value)).Single();
        var target = Regex.Matches(lines[1], @"\d+").Select(x => int.Parse(x.Value)).ToArray();
        var (targetX, targetY) = (target[0], target[1]);

        var m = 20183;

        var erosionLevelCache = new Dictionary<(int, int), int>();

        int erosionLevel(int x, int y)
        {
            var key = (x, y);
            if (!erosionLevelCache.ContainsKey(key))
            {
                if (x == targetX && y == targetY)
                {
                    erosionLevelCache[key] = depth;
                }
                else if (x == 0 && y == 0)
                {
                    erosionLevelCache[key] = depth;
                }
                else if (x == 0)
                {
                    erosionLevelCache[key] = ((y * 48271) + depth) % m;
                }
                else if (y == 0)
                {
                    erosionLevelCache[key] = ((x * 16807) + depth) % m;
                }
                else
                {
                    erosionLevelCache[key] = ((erosionLevel(x, y - 1) * erosionLevel(x - 1, y)) + depth) % m;
                }
            }

            return erosionLevelCache[key];
        }

        RegionType regionType(int x, int y)
        {
            return (RegionType)(erosionLevel(x, y) % 3);
        }

        return (targetX, targetY, regionType);
    }
}

enum RegionType
{
    Rocky = 0,
    Wet = 1,
    Narrow = 2
}

enum Tool
{
    Nothing,
    Torch,
    ClimbingGear
}

class PQueue<T>
{
    SortedDictionary<int, Queue<T>> d = new SortedDictionary<int, Queue<T>>();

    public bool Any()
    {
        return d.Any();
    }

    public void Enqueue(int p, T t)
    {
        if (!d.ContainsKey(p))
        {
            d[p] = new Queue<T>();
        }

        d[p].Enqueue(t);
    }

    public T Dequeue()
    {
        var p = d.Keys.First();
        var items = d[p];
        var t = items.Dequeue();
        if (!items.Any())
        {
            d.Remove(p);
        }

        return t;
    }
}