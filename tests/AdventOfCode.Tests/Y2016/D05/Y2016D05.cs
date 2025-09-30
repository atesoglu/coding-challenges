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

        output.Should().Be("f97c354d");
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

        output.Should().Be("863dde27");
    }

    private static IEnumerable<string> Hashes(string input)
    {
        for (var i = 0; i < int.MaxValue; i++)
        {
            var q = new ConcurrentQueue<(int i, byte[] hash)>();

            Parallel.ForEach(NumbersFrom(i), MD5.Create, (num, state, md5) =>
                {
                    var bytes = Encoding.ASCII.GetBytes(input + num);
                    var hash = md5.ComputeHash(bytes);

                    if (HasPrefix(hash))
                    {
                        q.Enqueue((num, hash));
                        state.Stop();
                    }

                    return md5;
                },
                md5 => md5.Dispose() // clean up
            );

            if (q.IsEmpty)
            {
                continue;
            }

            var item = q.OrderBy(x => x.i).First();
            i = item.i;
            yield return HashToHex(item.hash);
        }
    }

    // Checks if the first 5 hex digits are zeros (20 bits)
    private static bool HasPrefix(byte[] hash)
    {
        return hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0;
    }

    // Convert a byte array to hex string only when needed
    private static string HashToHex(byte[] hash)
    {
        var sb = new StringBuilder(hash.Length * 2);
        foreach (var b in hash)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    // Infinite sequence generator
    private static IEnumerable<int> NumbersFrom(int start)
    {
        for (var i = start;; i++)
            yield return i;
    }
}