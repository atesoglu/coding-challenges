using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D24;

[ChallengeName("Blizzard Basin")]
public class Y2022D24
{
    private readonly string _input = File.ReadAllText(@"Y2022\D24\Y2022D24-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (entry, exit, maps) = Parse(_input);
        var output = WalkTo(entry, exit, maps).time;

        output.Should().Be(274);
    }

    [Fact]
    public void PartTwo()
    {
        var (entry, exit, maps) = Parse(_input);
        var pos = WalkTo(entry, exit, maps);
        pos = WalkTo(pos, entry, maps);
        pos = WalkTo(pos, exit, maps);

        var output = pos.time;

        output.Should().Be(839);
    }


    // We do a standard A* algorithm, the only trick is that
    // the 'map' always changes as blizzards move, so our position
    // is now a space time coordinate. 
    // I used an efficent Maps class that can be queried with these.

    private record Pos(int time, int irow, int icol);

    // Standard A* algorithm
    private Pos WalkTo(Pos start, Pos goal, Maps maps)
    {
        var q = new PriorityQueue<Pos, int>();

        int f(Pos pos)
        {
            // estimate the remaining step count with Manhattan distance
            var dist =
                Math.Abs(goal.irow - pos.irow) +
                Math.Abs(goal.icol - pos.icol);
            return pos.time + dist;
        }

        q.Enqueue(start, f(start));
        var seen = new HashSet<Pos>();

        while (q.Count > 0)
        {
            var pos = q.Dequeue();
            if (pos.irow == goal.irow && pos.icol == goal.icol)
            {
                return pos;
            }

            foreach (var nextPos in NextPositions(pos, maps))
            {
                if (!seen.Contains(nextPos))
                {
                    seen.Add(nextPos);
                    q.Enqueue(nextPos, f(nextPos));
                }
            }
        }

        throw new Exception();
    }

    // Increase time, look for free neighbours
    private static IEnumerable<Pos> NextPositions(Pos pos, Maps maps) {
        pos = pos with {time = pos.time + 1};
        foreach (var nextPos in new Pos[]{
            pos,
            pos with {irow=pos.irow -1},
            pos with {irow=pos.irow +1},
            pos with {icol=pos.icol -1},
            pos with {icol=pos.icol +1},
        }) {
            if (maps.Get(nextPos) == '.') {
                yield return nextPos;
            }
        }
    }

    private static (Pos entry, Pos exit, Maps maps) Parse(string input) {
        var maps = new Maps(input);
        var entry = new Pos(0, 0, 1);
        var exit = new Pos(int.MaxValue, maps.crow - 1, maps.ccol - 2);
        return (entry, exit, maps);
    }

    // Space-time indexable map
    private class Maps {
        private string[] map;
        public readonly int crow;
        public readonly int ccol;

        public Maps(string input) {
            map = input.Split("\n");
            crow = map.Length;
            ccol = map[0].Length;
        }

        public char Get(Pos pos) {
            if (pos.irow == 0 && pos.icol == 1) {
                return '.';
            }
            if (pos.irow == crow - 1 && pos.icol == ccol - 2) {
                return '.';
            }

            if (pos.irow <= 0 || pos.irow >= crow - 1 ||
                pos.icol <= 0 || pos.icol >= ccol - 1
            ) {
                return '#';
            }

            // blizzards have a horizontal and a vertical loop
            // it's easy to check the original postions with going back in time
            // using modular arithmetic
            var hmod = ccol - 2;
            var vmod = crow - 2;

            var icolW = (pos.icol - 1 + hmod - (pos.time % hmod)) % hmod + 1;
            var icolE = (pos.icol - 1 + hmod + (pos.time % hmod)) % hmod + 1;
            var icolN = (pos.irow - 1 + vmod - (pos.time % vmod)) % vmod + 1;
            var icolS = (pos.irow - 1 + vmod + (pos.time % vmod)) % vmod + 1;

            return
                map[pos.irow][icolW] == '>' ? '>':
                map[pos.irow][icolE] == '<' ? '<':
                map[icolN][pos.icol] == 'v' ? 'v':
                map[icolS][pos.icol] == '^' ? '^':
                                              '.';
        }
    }
}