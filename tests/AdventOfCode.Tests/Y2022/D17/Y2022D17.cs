using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D17;

[ChallengeName("Pyroclastic Flow")]
public class Y2022D17
{
    private readonly string _input = File.ReadAllText(@"Y2022\D17\Y2022D17-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = new Tunnel(_input, 100).AddRocks(2022).Height;

        output.Should().Be(3069);
    }

    [Fact]
    public void PartTwo()
    {
        var output = new Tunnel(_input, 100).AddRocks(1000000000000).Height;

        output.Should().Be(1523167155404);
    }

    private class Tunnel
    {
        private int linesToStore;

        private List<char[]> lines = new List<char[]>();
        private long linesNotStored;

        public long Height => lines.Count + linesNotStored;

        private string[][] rocks;
        private string jets;
        private ModCounter irock;
        private ModCounter ijet;

        // Simulation runs so that only the top N lines are kept in the tunnel.
        // This is a practical constant, there is NO THEORY BEHIND it.
        public Tunnel(string jets, int linesToStore)
        {
            this.linesToStore = linesToStore;
            rocks = new string[][]
            {
                new[] { "####" },
                new[] { " # ", "###", " # " },
                new[] { "  #", "  #", "###" },
                new[] { "#", "#", "#", "#" },
                new[] { "##", "##" }
            };
            this.irock = new ModCounter(0, rocks.Length);

            this.jets = jets;
            this.ijet = new ModCounter(0, jets.Length);
        }

        public Tunnel AddRocks(long rocksToAdd)
        {
            // We are adding rocks one by one until we find a recurring pattern.

            // Then we can jump forward full periods with just increasing the height
            // of the cave: the top of the cave should look the same after a full period
            // so no need to simulate he rocks anymore.

            // Then we just add the remaining rocks.

            var seen = new Dictionary<string, (long rocksToAdd, long height)>();
            while (rocksToAdd > 0)
            {
                var hash = string.Join("", lines.SelectMany(ch => ch));
                if (seen.TryGetValue(hash, out var cache))
                {
                    // we have seen this pattern, advance forwad as much as possible
                    var heightOfPeriod = this.Height - cache.height;
                    var periodLength = cache.rocksToAdd - rocksToAdd;
                    linesNotStored += (rocksToAdd / periodLength) * heightOfPeriod;
                    rocksToAdd = rocksToAdd % periodLength;
                    break;
                }
                else
                {
                    seen[hash] = (rocksToAdd, this.Height);
                    this.AddRock();
                    rocksToAdd--;
                }
            }

            while (rocksToAdd > 0)
            {
                this.AddRock();
                rocksToAdd--;
            }

            return this;
        }

        // Adds one rock to the cave
        public Tunnel AddRock()
        {
            var rock = rocks[(int)irock++];

            // make room of 3 lines + the height of the rock
            for (var i = 0; i < rock.Length + 3; i++)
            {
                lines.Insert(0, "|       |".ToArray());
            }

            // simulate falling
            var pos = new Pos(0, 3);
            while (true)
            {
                var jet = jets[(int)ijet++];
                if (jet == '>' && !Hit(rock, pos.Right))
                {
                    pos = pos.Right;
                }
                else if (jet == '<' && !Hit(rock, pos.Left))
                {
                    pos = pos.Left;
                }

                if (Hit(rock, pos.Below))
                {
                    break;
                }

                pos = pos.Below;
            }

            Draw(rock, pos);
            return this;
        }

        // tells if a rock can be placed in the given location or hits something
        private bool Hit(string[] rock, Pos pos) =>
            Area(rock).Any(pt =>
                Get(rock, pt) == '#' &&
                Get(lines, pt + pos) != ' '
            );

        private void Draw(string[] rock, Pos pos)
        {
            // draws a rock pattern into the cave at the given x,y coordinates,
            foreach (var pt in Area(rock))
            {
                if (Get(rock, pt) == '#')
                {
                    Set(lines, pt + pos, '#');
                }
            }

            // remove empty lines from the top
            while (!lines[0].Contains('#'))
            {
                lines.RemoveAt(0);
            }

            // keep the tail
            while (lines.Count > linesToStore)
            {
                lines.RemoveAt(lines.Count - 1);
                linesNotStored++;
            }
        }
    }

    private static IEnumerable<Pos> Area(string[] mat) =>
        from irow in Enumerable.Range(0, mat.Length)
        from icol in Enumerable.Range(0, mat[0].Length)
        select new Pos(irow, icol);

    private static char Get(IEnumerable<IEnumerable<char>> mat, Pos pos)
    {
        return (mat.ElementAtOrDefault(pos.irow) ?? "#########").ElementAt(pos.icol);
    }

    private static char Set(IList<char[]> mat, Pos pos, char ch)
    {
        return mat[pos.irow][pos.icol] = ch;
    }

    private record struct Pos(int irow, int icol)
    {
        public Pos Left => new Pos(irow, icol - 1);
        public Pos Right => new Pos(irow, icol + 1);
        public Pos Below => new Pos(irow + 1, icol);

        public static Pos operator +(Pos posA, Pos posB) =>
            new Pos(posA.irow + posB.irow, posA.icol + posB.icol);
    }

    private record struct ModCounter(int index, int mod)
    {
        public static explicit operator int(ModCounter c) => c.index;

        public static ModCounter operator ++(ModCounter c) =>
            c with { index = c.index == c.mod - 1 ? 0 : c.index + 1 };
    }
}