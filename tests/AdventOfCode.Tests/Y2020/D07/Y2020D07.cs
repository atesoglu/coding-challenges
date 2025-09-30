using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D07;

[ChallengeName("Handy Haversacks")]
public class Y2020D07
{
    private readonly string _input = File.ReadAllText(@"Y2020\D07\Y2020D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var parentsOf = new Dictionary<string, HashSet<string>>();
        foreach (var line in _input.Split("\n"))
        {
            var descr = ParseLine(line);

            foreach (var (_, bag) in descr.children)
            {
                if (!parentsOf.ContainsKey(bag))
                {
                    parentsOf[bag] = new HashSet<string>();
                }

                parentsOf[bag].Add(descr.bag);
            }
        }

        IEnumerable<string> PathsToRoot(string bag)
        {
            yield return bag;

            if (parentsOf.ContainsKey(bag))
            {
                foreach (var container in parentsOf[bag])
                {
                    foreach (var bagT in PathsToRoot(container))
                    {
                        yield return bagT;
                    }
                }
            }
        }

        var output = PathsToRoot("shiny gold bag").ToHashSet().Count - 1;

        output.Should().Be(185);
    }

    [Fact]
    public void PartTwo()
    {
        var childrenOf = new Dictionary<string, List<(int count, string bag)>>();
        foreach (var line in _input.Split("\n"))
        {
            var descr = ParseLine(line);
            childrenOf[descr.bag] = descr.children;
        }

        long CountWithChildren(string bag) =>
            1 + (from child in childrenOf[bag] select child.count * CountWithChildren(child.bag)).Sum();

        var output = CountWithChildren("shiny gold bag") - 1;

        output.Should().Be(89084);
    }

    private static (string bag, List<(int count, string bag)> children) ParseLine(string line)
    {
        var bag = Regex.Match(line, "^[a-z]+ [a-z]+ bag").Value;

        var children =
            Regex
                .Matches(line, "(\\d+) ([a-z]+ [a-z]+ bag)")
                .Select(x => (count: int.Parse(x.Groups[1].Value), bag: x.Groups[2].Value))
                .ToList();

        return (bag, children);
    }
}