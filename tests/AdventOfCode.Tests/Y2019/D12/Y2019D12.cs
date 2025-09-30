using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D12;

[ChallengeName("The N-Body Problem")]
public class Y2019D12
{
    private readonly string _input = File.ReadAllText(@"Y2019\D12\Y2019D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = (
            from planet in Simulate(_input).ElementAt(999)
            let pot = planet.pos.Select(Math.Abs).Sum()
            let kin = planet.vel.Select(Math.Abs).Sum()
            select pot * kin
        ).Sum();

        output.Should().Be(6490);
    }

    [Fact]
    public void PartTwo()
    {
        var statesByDim = new long[3];
        for (var dim = 0; dim < 3; dim++)
        {
            var states = new HashSet<(int, int, int, int, int, int, int, int)>();
            foreach (var planets in Simulate(_input))
            {
                var state = (planets[0].pos[dim], planets[1].pos[dim], planets[2].pos[dim], planets[3].pos[dim], planets[0].vel[dim], planets[1].vel[dim], planets[2].vel[dim], planets[3].vel[dim]);
                if (states.Contains(state))
                {
                    break;
                }

                states.Add(state);
            }

            statesByDim[dim] = states.Count;
        }

        var output = Lcm(statesByDim[0], Lcm(statesByDim[1], statesByDim[2]));

        output.Should().Be(277068010964808);
    }

    private long Lcm(long a, long b) => a * b / Gcd(a, b);
    private static long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

    private static IEnumerable<(int[] pos, int[] vel)[]> Simulate(string input)
    {
        var planets = (
            from line in input.Split("\n")
            let m = Regex.Matches(line, @"-?\d+")
            let pos = (from v in m select int.Parse(v.Value)).ToArray()
            let vel = new int[3]
            select (pos, vel)
        ).ToArray();

        while (true)
        {
            foreach (var planetA in planets)
            {
                foreach (var planetB in planets)
                {
                    for (var dim = 0; dim < 3; dim++)
                    {
                        planetA.vel[dim] += Math.Sign(planetB.pos[dim] - planetA.pos[dim]);
                    }
                }
            }

            foreach (var planet in planets)
            {
                for (var dim = 0; dim < 3; dim++)
                {
                    planet.pos[dim] += planet.vel[dim];
                }
            }

            yield return planets;
        }
    }
}