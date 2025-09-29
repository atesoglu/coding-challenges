using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D24;

[ChallengeName("Immune System Simulator 20XX")]
public class Y2018D24
{
    private readonly string _input = File.ReadAllText(@"Y2018\D24\Y2018D24-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Fight(_input, 0).units;

        output.Should().Be(18346);
    }

    [Fact]
    public void PartTwo()
    {
        var l = 0;
        var h = int.MaxValue / 2;
        while (h - l > 1)
        {
            var m = (h + l) / 2;
            if (Fight(_input, m).immuneSystem)
            {
                h = m;
            }
            else
            {
                l = m;
            }
        }

        var output = Fight(_input, h).units;

        output.Should().Be(8698);
    }


    (bool immuneSystem, long units) Fight(string input, int boost)
    {
        var army = Parse(input);
        foreach (var g in army)
        {
            if (g.immuneSystem)
            {
                g.damage += boost;
            }
        }

        var attack = true;
        while (attack)
        {
            attack = false;
            var remainingTarget = new HashSet<Group>(army);
            var targets = new Dictionary<Group, Group>();
            foreach (var g in army.OrderByDescending(g => (g.effectivePower, g.initiative)))
            {
                var maxDamage = remainingTarget.Select(t => g.DamageDealtTo(t)).Max();
                if (maxDamage > 0)
                {
                    var possibleTargets = remainingTarget.Where(t => g.DamageDealtTo(t) == maxDamage);
                    targets[g] = possibleTargets.OrderByDescending(t => (t.effectivePower, t.initiative)).First();
                    remainingTarget.Remove(targets[g]);
                }
            }

            foreach (var g in targets.Keys.OrderByDescending(g => g.initiative))
            {
                if (g.units > 0)
                {
                    var target = targets[g];
                    var damage = g.DamageDealtTo(target);
                    if (damage > 0 && target.units > 0)
                    {
                        var dies = damage / target.hp;
                        target.units = Math.Max(0, target.units - dies);
                        if (dies > 0)
                        {
                            attack = true;
                        }
                    }
                }
            }

            army = army.Where(g => g.units > 0).ToList();
        }

        return (army.All(x => x.immuneSystem), army.Select(x => x.units).Sum());
    }

    List<Group> Parse(string input)
    {
        var lines = input.Split("\n");
        var immuneSystem = false;
        var res = new List<Group>();
        foreach (var line in lines)
            if (line == "Immune System:")
            {
                immuneSystem = true;
            }
            else if (line == "Infection:")
            {
                immuneSystem = false;
            }
            else if (line != "")
            {
                //643 units each with 9928 hit points (immune to fire; weak to slashing, bludgeoning) with an attack that does 149 fire damage at initiative 14
                var rx = @"(\d+) units each with (\d+) hit points(.*)with an attack that does (\d+)(.*)damage at initiative (\d+)";
                var m = Regex.Match(line, rx);
                if (m.Success)
                {
                    var g = new Group();
                    g.immuneSystem = immuneSystem;
                    g.units = int.Parse(m.Groups[1].Value);
                    g.hp = int.Parse(m.Groups[2].Value);
                    g.damage = int.Parse(m.Groups[4].Value);
                    g.attackType = m.Groups[5].Value.Trim();
                    g.initiative = int.Parse(m.Groups[6].Value);
                    var st = m.Groups[3].Value.Trim();
                    if (st != "")
                    {
                        st = st.Substring(1, st.Length - 2);
                        foreach (var part in st.Split(";"))
                        {
                            var k = part.Split(" to ");
                            var set = new HashSet<string>(k[1].Split(", "));
                            var w = k[0].Trim();
                            if (w == "immune")
                            {
                                g.immuneTo = set;
                            }
                            else if (w == "weak")
                            {
                                g.weakTo = set;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                    }

                    res.Add(g);
                }
                else
                {
                    throw new Exception();
                }
            }

        return res;
    }
}

class Group
{
    //4 units each with 9798 hit points (immune to bludgeoning) with an attack that does 1151 fire damage at initiative 9
    public bool immuneSystem;
    public long units;
    public int hp;
    public int damage;
    public int initiative;
    public string attackType;
    public HashSet<string> immuneTo = new HashSet<string>();
    public HashSet<string> weakTo = new HashSet<string>();

    public long effectivePower
    {
        get { return units * damage; }
    }

    public long DamageDealtTo(Group target)
    {
        if (target.immuneSystem == immuneSystem)
        {
            return 0;
        }
        else if (target.immuneTo.Contains(attackType))
        {
            return 0;
        }
        else if (target.weakTo.Contains(attackType))
        {
            return effectivePower * 2;
        }
        else
        {
            return effectivePower;
        }
    }
}