using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D12;

[ChallengeName("Digital Plumber")]
public partial class Y2017D12
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2017\D12\Y2017D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetPartitions().Single(x => x.Contains("0")).Count;

        output.Should().Be(239);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetPartitions().Count();

        output.Should().Be(215);
    }


    private IEnumerable<HashSet<string>> GetPartitions()
    {
        var nodes = Parse();
        var parent = new Dictionary<string, string>();

        string getRoot(string id)
        {
            var root = id;
            while (parent.ContainsKey(root))
            {
                root = parent[root];
            }

            return root;
        }

        foreach (var nodeA in nodes)
        {
            var rootA = getRoot(nodeA.Id);
            foreach (var nodeB in nodeA.Neighbours)
            {
                var rootB = getRoot(nodeB);
                if (rootB != rootA)
                {
                    parent[rootB] = rootA;
                }
            }
        }

        return
            from node in nodes
            let root = getRoot(node.Id)
            group node.Id by root
            into partitions
            select new HashSet<string>(partitions.ToArray());
    }

    private List<Node> Parse() => (from line in _lines let parts = NodeRegex().Split(line) select new Node(parts[0], new List<string>(NeighboursSplitterRegex().Split(parts[1])))).ToList();

    private record Node(string Id, List<string> Neighbours);

    [GeneratedRegex(", ")]
    private static partial Regex NeighboursSplitterRegex();

    [GeneratedRegex(" <-> ")]
    private static partial Regex NodeRegex();
}