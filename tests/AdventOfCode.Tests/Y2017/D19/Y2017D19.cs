using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D19
{
    [ChallengeName("A Series of Tubes")]
    public class Y2017D19
    {
        private readonly string _input = File.ReadAllText(@"Y2017\D19\Y2017D19-input.txt", Encoding.UTF8);

        [Fact]
        public void PartOne()
        {
            var output = FollowPath(_input).msg;
            output.Should().Be("ITSZCJNMUO");
        }

        [Fact]
        public void PartTwo()
        {
            var output = FollowPath(_input).steps;
            output.Should().Be(17420);
        }

        (string msg, int steps) FollowPath(string input)
        {
            var map = input.Split('\n');
            var rows = map.Length;
            var cols = 0;

            foreach (var line in map)
                if (line.Length > cols)
                    cols = line.Length;

            // Pad all lines so that indexing is safe
            for (var i = 0; i < map.Length; i++)
                map[i] = map[i].PadRight(cols);

            int irow = 0, icol = map[0].IndexOf('|');
            int drow = 1, dcol = 0; // initial direction: down

            var msg = "";
            var steps = 0;

            while (true)
            {
                // Move
                irow += drow;
                icol += dcol;
                steps++;

                // Check boundaries
                if (irow < 0 || irow >= rows || icol < 0 || icol >= cols)
                    break;

                var current = map[irow][icol];

                if (current == ' ')
                    break;

                if (current == '+')
                {
                    // Try two perpendicular directions
                    if (drow != 0)
                    {
                        // Coming vertically, try left and right
                        if (icol + 1 < cols && map[irow][icol + 1] != ' ')
                        {
                            dcol = 1;
                            drow = 0;
                        }
                        else if (icol - 1 >= 0 && map[irow][icol - 1] != ' ')
                        {
                            dcol = -1;
                            drow = 0;
                        }
                        else
                        {
                            throw new InvalidOperationException($"No valid turn at {irow},{icol}");
                        }
                    }
                    else
                    {
                        // Coming horizontally, try up and down
                        if (irow + 1 < rows && map[irow + 1][icol] != ' ')
                        {
                            drow = 1;
                            dcol = 0;
                        }
                        else if (irow - 1 >= 0 && map[irow - 1][icol] != ' ')
                        {
                            drow = -1;
                            dcol = 0;
                        }
                        else
                        {
                            throw new InvalidOperationException($"No valid turn at {irow},{icol}");
                        }
                    }
                }
                else if (char.IsLetter(current))
                {
                    msg += current;
                }
            }

            return (msg, steps);
        }
    }
}