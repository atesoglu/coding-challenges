using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D21;

[ChallengeName("RPG Simulator 20XX")]
public class Y2015D21
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2015\D21\Y2015D21-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var boss = Parse();
        var minGold = int.MaxValue;
        foreach (var c in Buy())
        {
            if (DefeatsBoss((c.damage, c.armor, 100), boss))
            {
                minGold = Math.Min(c.gold, minGold);
            }
        }

        var output = minGold;
        output.Should().Be(111);
    }

    [Fact]
    public void PartTwo()
    {
        var boss = Parse();
        var maxGold = 0;
        foreach (var c in Buy())
        {
            if (!DefeatsBoss((c.damage, c.armor, 100), boss))
            {
                maxGold = Math.Max(c.gold, maxGold);
            }
        }

        var output = maxGold;
        output.Should().Be(188);
    }

    private (int damage, int armor, int hp) Parse()
    {
        var hp = int.Parse(_lines[0].Split(": ")[1]);
        var damage = int.Parse(_lines[1].Split(": ")[1]);
        var armor = int.Parse(_lines[2].Split(": ")[1]);
        return (damage, armor, hp);
    }

    private static bool DefeatsBoss((int damage, int armor, int hp) player, (int damage, int armor, int hp) boss)
    {
        while (true)
        {
            boss.hp -= Math.Max(player.damage - boss.armor, 1);
            if (boss.hp <= 0)
            {
                return true;
            }

            player.hp -= Math.Max(boss.damage - player.armor, 1);
            if (player.hp <= 0)
            {
                return false;
            }
        }
    }

    private IEnumerable<(int gold, int damage, int armor)> Buy()
    {
        return
            from weapon in Buy(1, 1, new[] { (8, 4, 0), (10, 5, 0), (25, 6, 0), (40, 7, 0), (74, 8, 0) })
            from armor in Buy(0, 1, new[] { (13, 0, 1), (31, 0, 2), (53, 0, 3), (75, 0, 4), (102, 0, 5) })
            from ring in Buy(1, 2, new[] { (25, 1, 0), (50, 2, 0), (100, 3, 0), (20, 0, 1), (40, 0, 2), (80, 0, 3) })
            select Sum(weapon, armor, ring);
    }

    private IEnumerable<(int gold, int damage, int armor)> Buy(int min, int max, (int gold, int damage, int armor)[] items)
    {
        if (min == 0)
        {
            yield return (0, 0, 0);
        }

        foreach (var item in items)
        {
            yield return item;
        }

        if (max == 2)
        {
            for (var i = 0; i < items.Length; i++)
            {
                for (var j = i + 1; j < items.Length; j++)
                {
                    yield return Sum(items[i], items[j]);
                }
            }
        }
    }

    private static (int gold, int damage, int armor) Sum(params (int gold, int damage, int armor)[] items)
    {
        return (items.Select(item => item.gold).Sum(), items.Select(item => item.damage).Sum(), items.Select(item => item.armor).Sum());
    }
}