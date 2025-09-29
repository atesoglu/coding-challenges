using System.Text;
using AdventOfCode.Tests.Y2019.D02;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D13;

[ChallengeName("Care Package")]
public class Y2019D13
{
    private readonly string _input = File.ReadAllText(@"Y2019\D13\Y2019D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var icm = new IntCodeMachine(_input);
        var chunks = Chunk(icm.Run(), 3);
        var output = chunks.Count(x => x[2] == 2);

        output.Should().Be(318);
    }

    [Fact]
    public void PartTwo()
    {
        var icm = new IntCodeMachine(_input);
        icm.memory[0] = 2;
        var score = 0;
        var icolBall = -1;
        var icolPaddle = -1;
        var dir = 0;
        while (true)
        {
            var chunks = Chunk(icm.Run(dir), 3);
            foreach (var chunk in chunks)
            {
                var (icol, irow, block) = (chunk[0], chunk[1], chunk[2]);
                if ((icol, irow) == (-1, 0))
                {
                    score = (int)block;
                }

                if (block == 3)
                {
                    icolPaddle = (int)icol;
                }
                else if (block == 4)
                {
                    icolBall = (int)icol;
                }
            }

            if (icm.Halted())
            {
                break;
            }

            dir =
                icolBall < icolPaddle ? -1 :
                icolBall > icolPaddle ? 1 :
                0;
        }

        var output = score;

        output.Should().Be(16309);
    }

    private T[][] Chunk<T>(IEnumerable<T> source, int chunksize)
    {
        var res = new List<T[]>();
        while (source.Any())
        {
            res.Add(source.Take(chunksize).ToArray());
            source = source.Skip(chunksize);
        }

        return res.ToArray();
    }
}