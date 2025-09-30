using System.Text;
using FluentAssertions;
using System.Linq;
using Fs = System.Collections.Generic.LinkedList<AdventOfCode.Tests.Y2024.D09.Block>;
using Node = System.Collections.Generic.LinkedListNode<AdventOfCode.Tests.Y2024.D09.Block>;

namespace AdventOfCode.Tests.Y2024.D09;

internal record struct Block(int fileId, int length) { }

[ChallengeName("Disk Fragmenter")]
public class Y2024D09
{
    private readonly string _input = File.ReadAllText(@"Y2024\D09\Y2024D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Checksum(CompactFs(Parse(_input), fragmentsEnabled: true));

        output.Should().Be(6242766523059);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Checksum(CompactFs(Parse(_input), fragmentsEnabled: false));

        output.Should().Be(6272188244509);
    }


    private Fs CompactFs(Fs fs, bool fragmentsEnabled)
    {
        var (i, j) = (fs.First, fs.Last);
        while (i != j)
        {
            if (i.Value.fileId != -1)
            {
                i = i.Next;
            }
            else if (j.Value.fileId == -1)
            {
                j = j.Previous;
            }
            else
            {
                RelocateBlock(fs, i, j, fragmentsEnabled);
                j = j.Previous;
            }
        }

        return fs;
    }

    private void RelocateBlock(Fs fs, Node start, Node j, bool fragmentsEnabled)
    {
        for (var i = start; i != j; i = i.Next)
        {
            if (i.Value.fileId != -1)
            {
                // noop
            }
            else if (i.Value.length == j.Value.length)
            {
                (i.Value, j.Value) = (j.Value, i.Value);
                return;
            }
            else if (i.Value.length > j.Value.length)
            {
                var d = i.Value.length - j.Value.length;
                i.Value = j.Value;
                j.Value = j.Value with { fileId = -1 };
                fs.AddAfter(i, new Block(-1, d));
                return;
            }
            else if (i.Value.length < j.Value.length && fragmentsEnabled)
            {
                var d = j.Value.length - i.Value.length;
                i.Value = i.Value with { fileId = j.Value.fileId };
                j.Value = j.Value with { length = d };
                fs.AddAfter(j, new Block(-1, i.Value.length));
            }
        }
    }

    private long Checksum(Fs fs)
    {
        var res = 0L;
        var l = 0;
        for (var i = fs.First; i != null; i = i.Next)
        {
            for (var k = 0; k < i.Value.length; k++)
            {
                if (i.Value.fileId != -1)
                {
                    res += l * i.Value.fileId;
                }

                l++;
            }
        }

        return res;
    }

    private Fs Parse(string input)
    {
        return new Fs(input.Select((ch, i) => new Block(i % 2 == 1 ? -1 : i / 2, ch - '0')));
    }
}