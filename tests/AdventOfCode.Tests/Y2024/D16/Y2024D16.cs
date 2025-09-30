using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using State = (System.Numerics.Complex pos, System.Numerics.Complex dir);

namespace AdventOfCode.Tests.Y2024.D16;

[ChallengeName("Reindeer Maze")]
public class Y2024D16
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D16\Y2024D16-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = FindBestScore(BuildMap(_lines));

        output.Should().Be(72400);
    }

    [Fact]
    public void PartTwo()
    {
        var output = FindBestSpots(BuildMap(_lines));

        output.Should().Be(435);
    }

    private static readonly Complex North = -Complex.ImaginaryOne;
    private static readonly Complex South = Complex.ImaginaryOne;
    private static readonly Complex West = -1;
    private static readonly Complex East = 1;

    private int FindBestScore(Map map) => CalculateShortestPaths(map, FindGoal(map))[FindStart(map)];

    private int FindBestSpots(Map map)
    {
        var distances = CalculateShortestPaths(map, FindGoal(map));
        var startState = FindStart(map);

        var queue = new PriorityQueue<State, int>();
        queue.Enqueue(startState, distances[startState]);

        var bestSpots = new HashSet<State> { startState };
        while (queue.TryDequeue(out var currentState, out var remainingScore))
        {
            foreach (var (nextState, cost) in GetPossibleMoves(map, currentState, forward: true))
            {
                var nextRemainingScore = remainingScore - cost;
                if (!bestSpots.Contains(nextState) && distances[nextState] == nextRemainingScore)
                {
                    bestSpots.Add(nextState);
                    queue.Enqueue(nextState, nextRemainingScore);
                }
            }
        }

        return bestSpots.DistinctBy(state => state.pos).Count();
    }

    private Dictionary<State, int> CalculateShortestPaths(Map map, Complex goal)
    {
        var distances = new Dictionary<State, int>();
        var queue = new PriorityQueue<State, int>();

        foreach (var direction in new[] { North, East, West, South })
        {
            queue.Enqueue((goal, direction), 0);
            distances[(goal, direction)] = 0;
        }

        while (queue.TryDequeue(out var currentState, out var totalDistance))
        {
            foreach (var (nextState, cost) in GetPossibleMoves(map, currentState, forward: false))
            {
                var nextCost = totalDistance + cost;

                if (!distances.TryGetValue(nextState, out var oldCost))
                {
                    oldCost = int.MaxValue;
                }

                if (nextCost < oldCost)
                {
                    distances[nextState] = nextCost;
                    queue.Enqueue(nextState, nextCost);
                }
            }
        }

        return distances;
    }

    private static IEnumerable<(State, int cost)> GetPossibleMoves(Map map, State state, bool forward)
    {
        foreach (var direction in new[] { North, East, West, South })
        {
            if (direction == state.dir)
            {
                var position = forward ? state.pos + direction : state.pos - direction;

                if (!map.TryGetValue(position, out var cell) || cell != '#')
                {
                    yield return ((position, direction), 1);
                }
            }
            else if (direction != -state.dir)
            {
                yield return ((state.pos, direction), 1000);
            }
        }
    }

    private static Map BuildMap(IEnumerable<string> lines)
    {
        var rowArray = lines.ToArray();
        return (
            from y in Enumerable.Range(0, rowArray.Length)
            from x in Enumerable.Range(0, rowArray[0].Length)
            select new KeyValuePair<Complex, char>(x + y * South, rowArray[y][x])
        ).ToDictionary();
    }

    private static Complex FindGoal(Map map) => map.Keys.Single(key => map[key] == 'E');
    private static State FindStart(Map map) => (map.Keys.Single(key => map[key] == 'S'), East);
}
