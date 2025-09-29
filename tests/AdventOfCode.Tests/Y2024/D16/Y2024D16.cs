using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using State = (System.Numerics.Complex pos, System.Numerics.Complex dir);

namespace AdventOfCode.Tests.Y2024.D16;

[ChallengeName("Reindeer Maze")]
public class Y2024D16
{
    private readonly string _input = File.ReadAllText(@"Y2024\D16\Y2024D16-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = FindBestScore(GetMap(_input));

        output.Should().Be(72400);
    }

    [Fact]
    public void PartTwo()
    {
        var output = FindBestSpots(GetMap(_input));

        output.Should().Be(435);
    }

    static readonly Complex North = -Complex.ImaginaryOne;
    static readonly Complex South = Complex.ImaginaryOne;
    static readonly Complex West = -1;
    static readonly Complex East = 1;

    int FindBestScore(Map map) => Dijkstra(map, Goal(map))[Start(map)];

    int FindBestSpots(Map map)
    {
        var dist = Dijkstra(map, Goal(map));
        var start = Start(map);

        var q = new PriorityQueue<State, int>();
        q.Enqueue(start, dist[start]);

        var bestSpots = new HashSet<State> { start };
        while (q.TryDequeue(out var state, out var remainingScore))
        {
            foreach (var (next, score) in Steps(map, state, forward: true))
            {
                var nextRemainingScore = remainingScore - score;
                if (!bestSpots.Contains(next) && dist[next] == nextRemainingScore)
                {
                    bestSpots.Add(next);
                    q.Enqueue(next, nextRemainingScore);
                }
            }
        }

        return bestSpots.DistinctBy(state => state.pos).Count();
    }

    Dictionary<State, int> Dijkstra(Map map, Complex goal)
    {
        var dist = new Dictionary<State, int>();
        var q = new PriorityQueue<State, int>();

        foreach (var dir in new[] { North, East, West, South })
        {
            q.Enqueue((goal, dir), 0);
            dist[(goal, dir)] = 0;
        }

        while (q.TryDequeue(out var cur, out var totalDistance))
        {
            foreach (var (next, score) in Steps(map, cur, forward: false))
            {
                var nextCost = totalDistance + score;

                // ✅ replacement for dist.GetOrDefault(next, int.MaxValue)
                if (!dist.TryGetValue(next, out var oldCost))
                {
                    oldCost = int.MaxValue;
                }

                if (nextCost < oldCost)
                {
                    // no PriorityQueue.Remove in .NET, but it still works fine
                    dist[next] = nextCost;
                    q.Enqueue(next, nextCost);
                }
            }
        }

        return dist;
    }

    IEnumerable<(State, int cost)> Steps(Map map, State state, bool forward)
    {
        foreach (var dir in new[] { North, East, West, South })
        {
            if (dir == state.dir)
            {
                var pos = forward ? state.pos + dir : state.pos - dir;

                // ✅ replacement for map.GetValueOrDefault(pos) != '#'
                if (!map.TryGetValue(pos, out var cell) || cell != '#')
                {
                    yield return ((pos, dir), 1);
                }
            }
            else if (dir != -state.dir)
            {
                yield return ((state.pos, dir), 1000);
            }
        }
    }

    Map GetMap(string input)
    {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(x + y * South, map[y][x])
        ).ToDictionary();
    }

    Complex Goal(Map map) => map.Keys.Single(k => map[k] == 'E');
    State Start(Map map) => (map.Keys.Single(k => map[k] == 'S'), East);
}
