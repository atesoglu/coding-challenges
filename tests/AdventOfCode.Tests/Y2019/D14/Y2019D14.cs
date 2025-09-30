using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D14;

[ChallengeName("Space Stoichiometry")]
public class Y2019D14
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2019\D14\Y2019D14-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Parse()(1);

        output.Should().Be(261960);
    }

    [Fact]
    public void PartTwo()
    {
        var output = 0;
        var oreForFuel = Parse();

        var ore = 1000000000000L;

        var fuel = 1L;
        while (true)
        {
            // newFuel <= the amount we can produce with the given ore
            // since (double)ore / oreForFuel(fuel) >= 1, fuel becomes
            // a better estimation in each iteration until it reaches
            // the maximum

            var newFuel = (int)((double)ore / oreForFuel(fuel) * fuel);

            if (newFuel == fuel)
            {
                output = newFuel;
                break;
            }

            fuel = newFuel;
        }

        output.Should().Be(4366186);
    }

    private Func<long, long> Parse()
    {
        (string chemical, long amount) ParseReagent(string st)
        {
            var parts = st.Split(" ");
            return (parts[1], long.Parse(parts[0]));
        }

        var reactions = (
            from rule in _lines
            let inout = rule.Split(" => ")
            let input = inout[0].Split(", ").Select(ParseReagent).ToArray()
            let output = ParseReagent(inout[1])
            select (output, input)
        ).ToDictionary(inout => inout.output.chemical, inout => inout);

        return (fuel) =>
        {
            var ore = 0L;
            var inventory = reactions.Keys.ToDictionary(chemical => chemical, _ => 0L);
            var productionList = new Queue<(string chemical, long amount)>();
            productionList.Enqueue(("FUEL", fuel));

            while (productionList.Any())
            {
                var (chemical, amount) = productionList.Dequeue();
                if (chemical == "ORE")
                {
                    ore += amount;
                }
                else
                {
                    var reaction = reactions[chemical];

                    var useFromInventory = Math.Min(amount, inventory[chemical]);
                    amount -= useFromInventory;
                    inventory[chemical] -= useFromInventory;

                    if (amount > 0)
                    {
                        var multiplier = (long)Math.Ceiling((decimal)amount / reaction.output.amount);
                        inventory[chemical] = Math.Max(0, multiplier * reaction.output.amount - amount);

                        foreach (var reagent in reaction.input)
                        {
                            productionList.Enqueue((reagent.chemical, reagent.amount * multiplier));
                        }
                    }
                }
            }

            return ore;
        };
    }
}