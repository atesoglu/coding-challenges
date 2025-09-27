using System.Text;
using System.Text.Json;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D12;

[ChallengeName("JSAbacusFramework.io")]
public class Y2015D12
{
    private readonly string _input = File.ReadAllText(@"Y2015\D12\Y2015D12-input.txt", Encoding.UTF8);
    private readonly JsonElement _root;

    public Y2015D12()
    {
        _root = JsonDocument.Parse(_input).RootElement;
    }

    [Fact]
    public void PartOne()
    {
        var output = Traverse(_root, false);

        output.Should().Be(191164);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Traverse(_root, true);

        output.Should().Be(87842);
    }

    private static int Traverse(JsonElement element, bool skipRedObjects)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Number => element.GetInt32(),

            JsonValueKind.Array => element.EnumerateArray().Sum(e => Traverse(e, skipRedObjects)),

            JsonValueKind.Object => (skipRedObjects && element.EnumerateObject()
                .Any(p => p.Value.ValueKind == JsonValueKind.String && p.Value.GetString() == "red"))
                ? 0
                : element.EnumerateObject().Sum(p => Traverse(p.Value, skipRedObjects)),

            _ => 0
        };
    }
}