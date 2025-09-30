using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D24;

[ChallengeName("Electromagnetic Moat")]
public class Y2017D24
{
    private readonly string _input = File.ReadAllText(@"Y2017\D24\Y2017D24-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = StrongestBridge(_input, (a, b) => a.strength - b.strength);

        output.Should().Be(1868);
    }

    [Fact]
    public void PartTwo()
    {
        var output = StrongestBridge(_input, (a, b) => a.CompareTo(b));

        output.Should().Be(1841);
    }


    private int StrongestBridge(string input, Func<(int length, int strength), (int length, int strength), int> compare)
    {
        (int length, int strength) fold(int pinIn, HashSet<Component> components)
        {
            var strongest = (0, 0);
            foreach (var component in components.ToList())
            {
                var pinOut =
                    pinIn == component.pinA ? component.pinB :
                    pinIn == component.pinB ? component.pinA :
                    -1;

                if (pinOut != -1)
                {
                    components.Remove(component);
                    var curr = fold(pinOut, components);
                    (curr.length, curr.strength) = (curr.length + 1, curr.strength + component.pinA + component.pinB);
                    strongest = compare(curr, strongest) > 0 ? curr : strongest;
                    components.Add(component);
                }
            }

            return strongest;
        }

        return fold(0, Parse(input)).strength;
    }

    private HashSet<Component> Parse(string input)
    {
        var components = new HashSet<Component>();
        foreach (var line in input.Split('\n'))
        {
            var parts = line.Split('/');
            components.Add(new Component { pinA = int.Parse(parts[0]), pinB = int.Parse(parts[1]) });
        }

        return components;
    }

    private class Component
    {
        public int pinA;
        public int pinB;
    }
}