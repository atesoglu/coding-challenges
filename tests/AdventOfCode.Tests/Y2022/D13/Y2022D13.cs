using System.Text;
using System.Text.Json.Nodes;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D13;

[ChallengeName("Distress Signal")]
public class Y2022D13
{
    private readonly string _input = File.ReadAllText(@"Y2022\D13\Y2022D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetPackets(_input)
            .Chunk(2)
            .Select((pair, index) => Compare(pair[0], pair[1]) < 0 ? index + 1 : 0)
            .Sum();

        output.Should().Be(5013);
    }

    [Fact]
    public void PartTwo()
    {
        var divider = GetPackets("[[2]]\n[[6]]").ToList();
        var packets = GetPackets(_input).Concat(divider).ToList();
        packets.Sort(Compare);
        var output = (packets.IndexOf(divider[0]) + 1) * (packets.IndexOf(divider[1]) + 1);

        output.Should().Be(25038);
    }

    private static IEnumerable<JsonNode> GetPackets(string input)
    {
        // Normalize line endings to just "\n"
        input = input.Replace("\r\n", "\n").TrimEnd();

        return from line in input.Split("\n")
            where !string.IsNullOrEmpty(line)
            select JsonNode.Parse(line);
    }

    private int Compare(JsonNode nodeA, JsonNode nodeB)
    {
        if (nodeA is JsonValue && nodeB is JsonValue)
        {
            return (int)nodeA - (int)nodeB;
        }
        else
        {
            // It's AoC time, let's exploit FirstOrDefault! 
            // ðŸ˜ˆ if all items are equal, compare the length of the arrays 
            var arrayA = nodeA as JsonArray ?? new JsonArray((int)nodeA);
            var arrayB = nodeB as JsonArray ?? new JsonArray((int)nodeB);
            return Enumerable.Zip(arrayA, arrayB)
                .Select(p => Compare(p.First, p.Second))
                .FirstOrDefault(c => c != 0, arrayA.Count - arrayB.Count);
        }
    }
}