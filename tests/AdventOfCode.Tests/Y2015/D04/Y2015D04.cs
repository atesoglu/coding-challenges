using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Tests.Y2015.D04;

[ChallengeName("The Ideal Stocking Stuffer")]
public class Y2015D04
{
    public int PartOne(string input) => FindFirst(input, "00000");

    public int PartTwo(string input) => FindFirst(input, "000000");

    private int FindFirst(string input, string prefix)
    {
        var q = new ConcurrentQueue<int>();
        Parallel.ForEach(Numbers(), MD5.Create,
            (i, state, md5) =>
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

    private IEnumerable<int> Numbers()
    {
        for (var i = 0;; i++)
        {
            yield return i;
        }
    }
}