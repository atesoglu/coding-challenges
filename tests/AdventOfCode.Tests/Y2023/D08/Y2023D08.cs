using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using Map = System.Collections.Generic.Dictionary<string, (string Left, string Right)>;

namespace AdventOfCode.Tests.Y2023.D08;

[ChallengeName("Haunted Wasteland")]
public class Y2023D08
{
    private readonly string _input = File.ReadAllText(@"Y2023\D08\Y2023D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, "AAA", "ZZZ");

        output.Should().Be(12599);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, "A", "Z");

        output.Should().Be(8245452805243);
    }

    private long Solve(string input, string aMarker, string zMarker)
    {
        var blocks = input.Split("\n\n");
        var dirs = blocks[0];
        var map = ParseMap(blocks[1]);

        // From each start node calculate the steps to the first Z node, then 
        // suppose that if we continue wandering around in the desert the 
        /// distance between the Z nodes is always the same.
        // The input was set up this way, which justifies the use of LCM in 
        // computing the final result.
        return map.Keys
            .Where(w => w.EndsWith(aMarker))
            .Select(w => StepsToZ(w, zMarker, dirs, map))
            .Aggregate(1L, Lcm);
    }

    private long Lcm(long a, long b) => a * b / Gcd(a, b);
    private long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

    private long StepsToZ(string current, string zMarker, string dirs, Map map)
    {
        var i = 0;
        while (!current.EndsWith(zMarker))
        {
            var dir = dirs[i % dirs.Length];
            current = dir == 'L' ? map[current].Left : map[current].Right;
            i++;
        }

        return i;
    }

    private Map ParseMap(string input) =>
        input.Split("\n")
            .Select(line => Regex.Matches(line, "[A-Z]+"))
            .ToDictionary(m => m[0].Value, m => (m[1].Value, m[2].Value));
}