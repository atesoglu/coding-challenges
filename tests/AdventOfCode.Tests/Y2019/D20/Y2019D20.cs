using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D20;

[ChallengeName("Donut Maze")]
public class Y2019D20
{
    private readonly string _input = File.ReadAllText(@"Y2019\D20\Y2019D20-input.txt");

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, false);
        output.Should().Be(668);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, true);
        output.Should().Be(7778);
    }

    private int Solve(string input, bool part2)
    {
        var mx = input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray())
            .ToArray();

        var (portals, start, end) = Explore(mx);

        var q = new Queue<(Pos3 pos, int dist)>();
        var seen = new HashSet<Pos3>();
        q.Enqueue((start, 0));
        seen.Add(start);

        IEnumerable<Pos3> Neighbours(Pos3 pos)
        {
            foreach (var (drow, dcol) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) })
            {
                var nr = pos.irow + drow;
                var nc = pos.icol + dcol;
                if (nr >= 0 && nr < mx.Length && nc >= 0 && nc < mx[nr].Length)
                    yield return new Pos3(nr, nc, pos.level);
            }

            if (portals.TryGetValue(new Pos2(pos.irow, pos.icol), out var portal))
            {
                var dlevel = part2 ? portal.dlevel : 0;
                var newLevel = pos.level + dlevel;
                if (newLevel >= 0)
                    yield return new Pos3(portal.irow, portal.icol, newLevel);
            }
        }

        while (q.Count > 0)
        {
            var (pos, dist) = q.Dequeue();
            if (pos == end)
                return dist;

            foreach (var nxt in Neighbours(pos))
            {
                if (!seen.Contains(nxt) && mx[nxt.irow][nxt.icol] == '.')
                {
                    seen.Add(nxt);
                    q.Enqueue((nxt, dist + 1));
                }
            }
        }

        throw new Exception("No path found");
    }

    private static (Dictionary<Pos2, PosD> portals, Pos3 start, Pos3 goal) Explore(char[][] mx)
    {
        var portals = new Dictionary<Pos2, PosD>();
        var tmp = new Dictionary<string, Pos2>();
        var rows = mx.Length;
        var cols = mx.Max(r => r.Length);

        for (var irow = 0; irow < rows; irow++)
        {
            for (var icol = 0; icol < mx[irow].Length; icol++)
            {
                foreach (var (dr, dc) in new[] { (0, 1), (1, 0) })
                {
                    var nr = irow + dr;
                    var nc = icol + dc;
                    if (nr >= rows || nc >= mx[nr].Length) continue;

                    var c1 = mx[irow][icol];
                    var c2 = mx[nr][nc];
                    if (!char.IsLetter(c1) || !char.IsLetter(c2)) continue;

                    Pos2 portal = default;
                    if (irow - dr >= 0 && irow - dr < rows && icol - dc >= 0 && icol - dc < mx[irow - dr].Length && mx[irow - dr][icol - dc] == '.')
                        portal = new Pos2(irow - dr, icol - dc);
                    else if (nr + dr < rows && nc + dc < mx[nr + dr].Length && mx[nr + dr][nc + dc] == '.')
                        portal = new Pos2(nr + dr, nc + dc);
                    else
                        continue;

                    var label = $"{c1}{c2}";
                    if (tmp.ContainsKey(label))
                    {
                        // outer/inner
                        var outer = portal.irow <= 2 || portal.irow >= rows - 3 || portal.icol <= 2 || portal.icol >= cols - 3;
                        var dlevel = outer ? -1 : 1;

                        portals[portal] = new PosD(tmp[label].irow, tmp[label].icol, dlevel);
                        portals[tmp[label]] = new PosD(portal.irow, portal.icol, -dlevel);
                    }
                    else
                    {
                        tmp[label] = portal;
                    }

                    mx[irow][icol] = ' ';
                    mx[nr][nc] = ' ';
                }
            }
        }

        return (portals,
            new Pos3(tmp["AA"].irow, tmp["AA"].icol, 0),
            new Pos3(tmp["ZZ"].irow, tmp["ZZ"].icol, 0));
    }
}

internal record Pos2(int irow, int icol);

internal record Pos3(int irow, int icol, int level);

internal record PosD(int irow, int icol, int dlevel);