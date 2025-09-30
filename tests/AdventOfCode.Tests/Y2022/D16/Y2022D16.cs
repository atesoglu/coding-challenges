using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D16;

[ChallengeName("Proboscidea Volcanium")]
public class Y2022D16
{
    private readonly string _input = File.ReadAllText(@"Y2022\D16\Y2022D16-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, true, 30);

        output.Should().Be(2183);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, false, 26);

        output.Should().Be(2911);
    }


    private record Map(int[,] distances, Valve[] valves);

    private record Valve(int id, string name, int flowRate, string[] tunnels);

    private int Solve(string input, bool humanOnly, int time)
    {
        var map = Parse(input);
        var start = map.valves.Single(x => x.name == "AA");
        var valvesToOpen = map.valves.Where(valve => valve.flowRate > 0).ToArray();

        var cache = new Dictionary<string, int>();
        if (humanOnly)
        {
            return MaxFlow(cache, map, start, valvesToOpen.ToHashSet(), time);
        }
        else
        {
            return Pairings(valvesToOpen).Select(pairing =>
                MaxFlow(cache, map, start, pairing.human, time) +
                MaxFlow(cache, map, start, pairing.elephant, time)
            ).Max();
        }
    }

    // Divide the valves between human and elephant in all possible ways
    private IEnumerable<(HashSet<Valve> human, HashSet<Valve> elephant)> Pairings(Valve[] valves)
    {
        var maxMask = 1 << (valves.Length - 1);

        for (var mask = 0; mask < maxMask; mask++)
        {
            var elephant = new HashSet<Valve>();
            var human = new HashSet<Valve>();

            elephant.Add(valves[0]);

            for (var ivalve = 1; ivalve < valves.Length; ivalve++)
            {
                if ((mask & (1 << ivalve)) == 0)
                {
                    human.Add(valves[ivalve]);
                }
                else
                {
                    elephant.Add(valves[ivalve]);
                }
            }

            yield return (human, elephant);
        }
    }

    private int MaxFlow(
        Dictionary<string, int> cache,
        Map map,
        Valve currentValve,
        HashSet<Valve> valves,
        int remainingTime
    )
    {
        var key =
            remainingTime + "-" +
            currentValve.id + "-" +
            string.Join("-", valves.OrderBy(x => x.id).Select(x => x.id));

        if (!cache.ContainsKey(key))
        {
            // current valve gives us this much flow:
            var flowFromValve = currentValve.flowRate * remainingTime;

            // determine best use of the remaining time:
            var flowFromRest = 0;
            foreach (var valve in valves.ToArray())
            {
                var distance = map.distances[currentValve.id, valve.id];

                if (remainingTime >= distance + 1)
                {
                    valves.Remove(valve);
                    remainingTime -= distance + 1;

                    flowFromRest = Math.Max(
                        flowFromRest, MaxFlow(cache, map, valve, valves, remainingTime));

                    remainingTime += distance + 1;
                    valves.Add(valve);
                }
            }

            cache[key] = flowFromValve + flowFromRest;
        }

        return cache[key];
    }

    private Map Parse(string input)
    {
        // Valve BB has flow rate=0; tunnels lead to valve CC
        // Valve CC has flow rate=10; tunnels lead to valves DD, EE
        var valveList = new List<Valve>();
        foreach (var line in input.Split("\n"))
        {
            var name = Regex.Match(line, "Valve (.*) has").Groups[1].Value;
            var flow = int.Parse(Regex.Match(line, @"\d+").Groups[0].Value);
            var tunnels = Regex.Match(line, "to valves? (.*)").Groups[1].Value.Split(", ");
            valveList.Add(new Valve(0, name, flow, tunnels));
        }

        var valves = valveList
            .OrderByDescending(valve => valve.flowRate)
            .Select((v, i) => v with { id = i })
            .ToArray();

        return new Map(ComputeDistances(valves), valves);
    }

    private int[,] ComputeDistances(Valve[] valves)
    {
        // Floyd-Warshall style distance calculation for every pair of valves.
        // See https://en.wikipedia.org/wiki/Floyd%E2%80%93Warshall_algorithm
        // The only change is that moving from i to i is not enabled.

        // This is an O(n^3) algorithm, but we are dealing with a low n.
        var n = valves.Length;

        // Just "big enough" so that infinity + infinity still fits in an int.
        var infinity = int.MaxValue / 2;

        var dist = new int[valves.Length, valves.Length];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                var neighbours = valves[i].tunnels.Contains(valves[j].name);
                dist[i, j] = neighbours ? 1 : infinity;
            }
        }

        for (var k = 0; k < n; k++)
        {
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    dist[i, j] = Math.Min(dist[i, j], dist[i, k] + dist[k, j]);
                }
            }
        }

        return dist;
    }
}