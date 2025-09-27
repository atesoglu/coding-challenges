using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D05;

[ChallengeName("How About a Nice Game of Chess?")]
public class Y2016D05
{
    private readonly string _input = File.ReadAllText(@"Y2016\D05\Y2016D05-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = string.Join("", Hashes(_input).Select(hash => hash[5]).Take(8));

        output.Should().Be("0");
    }

    [Fact]
    public void PartTwo()
    {
        var res = new char[8];
        var found = 0;
        foreach (var hash in Hashes(_input))
        {
            var idx = hash[5] - '0';
            if (0 <= idx && idx < 8 && res[idx] == 0)
            {
                res[idx] = hash[6];
                found++;
                if (found == 8)
                {
                    break;
                }
            }
        }

        var output = string.Join("", res);

        output.Should().Be("0");
    }

    private IEnumerable<string> Hashes(string input)
    {
        for (var i = 0; i < int.MaxValue; i++)
        {
            var q = new ConcurrentQueue<(int i, string hash)>();

            Parallel.ForEach(
                NumbersFrom(i),
                () => MD5.Create(),
                (i, state, md5) =>
                {
                    var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));
                    var hashString = string.Join("", hash.Select(x => x.ToString("x2")));

                    if (hashString.StartsWith("00000"))
                    {
                        q.Enqueue((i, hashString));
                        state.Stop();
                    }

                    return md5;
                },
                (_) => { }
            );
            var item = q.OrderBy(x => x.i).First();
            i = item.i;
            yield return item.hash;
        }
    }

    IEnumerable<int> NumbersFrom(int i)
    {
        for (;;) yield return i++;
    }
}