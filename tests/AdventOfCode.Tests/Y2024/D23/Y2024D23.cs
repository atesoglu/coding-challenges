using System.Text;
using FluentAssertions;
using Graph = System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>;
using Component = string;

namespace AdventOfCode.Tests.Y2024.D23;

[ChallengeName("LAN Party")]
public class Y2024D23
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D23\Y2024D23-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var g = BuildGraph(_lines);
        var components = g.Keys.ToHashSet();
        components = Grow(g, components);
        components = Grow(g, components);

        var output = components.Count(c => Members(c).Any(m => m.StartsWith("t")));

        output.Should().Be(1302);
    }

    [Fact]
    public void PartTwo()
    {
        var g = BuildGraph(_lines);
        var components = g.Keys.ToHashSet();
        while (components.Count > 1)
        {
            components = Grow(g, components);
        }


        var output = components.Single();

        output.Should().Be("cb,df,fo,ho,kk,nw,ox,pq,rt,sf,tq,wi,xz");
    }

    private HashSet<Component> Grow(Graph g, HashSet<Component> components) => (
        from c in components.AsParallel()
        let members = Members(c)
        from neighbour in members.SelectMany(m => g[m]).Distinct()
        where !members.Contains(neighbour)
        where members.All(m => g[neighbour].Contains(m))
        select Extend(c, neighbour)
    ).ToHashSet();

    private static IEnumerable<string> Members(Component c) =>
        c.Split(",");

    private Component Extend(Component c, string item) => string.Join(",", Members(c).Append(item).OrderBy(x => x));

    private static Graph BuildGraph(IEnumerable<string> lines)
    {
        var edges =
            from line in lines
            let nodes = line.Split("-")
            from edge in new[] { (nodes[0], nodes[1]), (nodes[1], nodes[0]) }
            select (From: edge.Item1, To: edge.Item2);

        return (
            from e in edges
            group e by e.From
            into g
            select (g.Key, g.Select(e => e.To).ToHashSet())
        ).ToDictionary();
    }
}