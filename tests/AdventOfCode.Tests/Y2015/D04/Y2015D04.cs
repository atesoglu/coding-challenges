using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D04;

[ChallengeName("The Ideal Stocking Stuffer")]
public class Y2015D04
{
    private readonly string _input = File.ReadAllText(@"Y2015\D04\Y2015D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = FindFirst(_input, "00000");

        output.Should().Be(346386);
    }

    [Fact]
    public void PartTwo()
    {
        var output = FindFirst(_input, "000000");

        output.Should().Be(9958218);
    }

    private static int FindFirst(string input, string prefix)
    {
        var q = new ConcurrentQueue<int>();
        Parallel.ForEach(Numbers(), MD5.Create, (i, state, md5) =>
            {
                var hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));
                var hash = string.Join("", hashBytes.Select(b => b.ToString("x2")));
                if (hash.StartsWith(prefix))
                {
                    q.Enqueue(i);
                    state.Stop();
                }

                return md5;
            },
            md5 => { md5.Dispose(); });

        return q.Min();
    }

    private static IEnumerable<int> Numbers()
    {
        for (var i = 0;; i++)
        {
            yield return i;
        }
    }
}