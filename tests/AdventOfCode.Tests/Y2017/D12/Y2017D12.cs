using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D12;

[ChallengeName("Digital Plumber")]
public class Y2017D12
{
    private readonly string _input = File.ReadAllText(@"Y2017\D12\Y2017D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetPartitions(_input).Single(x => x.Contains("0")).Count();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetPartitions(_input).Count();

        output.Should().Be(0);
    }


    IEnumerable<HashSet<string>> GetPartitions(string input)
    {
        var nodes = Parse(input);
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

    List<Node> Parse(string input)
    {
        return (
            from line in input.Split('\n')
            let parts = Regex.Split(line, " <-> ")
            select new Node()
            {
                Id = parts[0],
                Neighbours = new List<string>(Regex.Split(parts[1], ", "))
            }
        ).ToList();
    }

    class Node
    {
        public string Id;
        public List<string> Neighbours;
    }
}