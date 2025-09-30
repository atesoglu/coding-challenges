using System.Collections.Immutable;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using Rules = System.Collections.Generic.Dictionary<string, string>;
using Cube = System.Collections.Immutable.ImmutableArray<AdventOfCode.Tests.Y2023.D19.Range>;

namespace AdventOfCode.Tests.Y2023.D19;

[ChallengeName("Aplenty")]
public class Y2023D19
{
    private readonly string _input = File.ReadAllText(@"Y2023\D19\Y2023D19-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        // Part 1 can be understood in the context of Part 2. Part 2 asks to compute
        // the accepted volume of a four dimensional hypercube. It has some elaborate
        // way to slice up the cube parallel to its edges to smaller and smaller pieces
        // and decide if the final sub-sub cubes are accepted or not. Our Part 2
        // algorithm follows these rules and returns the 'accepted'volume we are
        // looking for.

        // We can use this algorithm to solve Part 1 starting from unit sized cubes
        // and checking if they are fully accepted or not.

        var parts = _input.Split("\n\n");
        var rules = ParseRules(parts[0]);


        var output = (
            from cube in ParseUnitCube(parts[1])
            where AcceptedVolume(rules, cube) == 1
            select cube.Select(r => r.begin).Sum()
        ).Sum();

        output.Should().Be(399284);
    }

    [Fact]
    public void PartTwo()
    {
        var parts = _input.Split("\n\n");
        var rules = ParseRules(parts[0]);
        var cube = Enumerable.Repeat(new Range(1, 4000), 4).ToImmutableArray();

        var output = AcceptedVolume(rules, cube);

        output.Should().Be(121964982771486);
    }

    private BigInteger AcceptedVolume(Rules rules, Cube cube)
    {
        var q = new Queue<(Cube cube, string state)>();
        q.Enqueue((cube, "in"));

        BigInteger res = 0;
        while (q.Any())
        {
            (cube, var state) = q.Dequeue();
            if (cube.Any(coord => coord.end < coord.begin))
            {
                continue; // cube is empty
            }
            else if (state == "R")
            {
                continue; // cube is rejected
            }
            else if (state == "A")
            {
                res += Volume(cube); // cube is accepted
            }
            else
            {
                foreach (var stm in rules[state].Split(","))
                {
                    var cond = TryParseCond(stm);
                    if (cond == null)
                    {
                        q.Enqueue((cube, stm));
                    }
                    else if (cond.op == '<')
                    {
                        var (cube1, cube2) = CutCube(cube, cond.dim, cond.num - 1);
                        q.Enqueue((cube1, cond.state));
                        cube = cube2;
                    }
                    else if (cond?.op == '>')
                    {
                        var (cube1, cube2) = CutCube(cube, cond.dim, cond.num);
                        cube = cube1;
                        q.Enqueue((cube2, cond.state));
                    }
                }
            }
        }

        return res;
    }

    private BigInteger Volume(Cube cube) =>
        cube.Aggregate(BigInteger.One, (m, r) => m * (r.end - r.begin + 1));

    // Cuts a cube along the specified dimension, other dimensions are unaffected.
    private (Cube lo, Cube hi) CutCube(Cube cube, int dim, int num)
    {
        var r = cube[dim];
        return (
            cube.SetItem(dim, r with { end = Math.Min(num, r.end) }),
            cube.SetItem(dim, r with { begin = Math.Max(r.begin, num + 1) })
        );
    }

    private Cond TryParseCond(string st) =>
        st.Split('<', '>', ':') switch
        {
            ["x", var num, var state] => new Cond(0, st[1], int.Parse(num), state),
            ["m", var num, var state] => new Cond(1, st[1], int.Parse(num), state),
            ["a", var num, var state] => new Cond(2, st[1], int.Parse(num), state),
            ["s", var num, var state] => new Cond(3, st[1], int.Parse(num), state),
            _ => null
        };

    private Rules ParseRules(string input) => (
        from line in input.Split('\n')
        let parts = line.Split('{', '}')
        select new KeyValuePair<string, string>(parts[0], parts[1])
    ).ToDictionary();

    private IEnumerable<Cube> ParseUnitCube(string input) =>
        from line in input.Split('\n')
        let nums = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value))
        select nums.Select(n => new Range(n, n)).ToImmutableArray();
}

internal record Range(int begin, int end);

internal record Cond(int dim, char op, int num, string state);