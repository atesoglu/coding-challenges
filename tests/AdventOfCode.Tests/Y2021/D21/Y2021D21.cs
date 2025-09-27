using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D21;

[ChallengeName("Dirac Dice")]
public class Y2021D21
{
    private readonly string _input = File.ReadAllText(@"Y2021\D21\Y2021D21-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input)
    {
        // â­ we can convert 3 consecutive throws to a 3-throw with the new .Net 6 Chunk function:
        var threeRoll = DeterministicThrows().Chunk(3).Select(x => x.Sum());

        // take turns until the active player wins:
        var round = 0;
        var (active, other) = Parse(input);
        foreach (var steps in threeRoll)
        {
            round++;
            active = active.Move(steps);
            if (active.score >= 1000)
            {
                break;
            }

            (active, other) = (other, active);
        }

        return other.score * 3 * round;
    }

    private object PartTwo(string input)
    {
        // win counts tells us how many times the active and the other player wins
        // if they are starting from the given positions and scores.

        // this function needs to be cached, because we don't have time till eternity.
        var cache = new Dictionary<(Player, Player), (long, long)>();

        (long activeWins, long otherWins) winCounts((Player active, Player other) players)
        {
            if (players.other.score >= 21)
            {
                return (0, 1);
            }

            if (!cache.ContainsKey(players))
            {
                var (activeWins, otherWins) = (0L, 0L);
                foreach (var steps in DiracThrows())
                {
                    var wins = winCounts((players.other, players.active.Move(steps)));
                    // they are switching roles here ^
                    // hence the return value needs to be swapped as well
                    activeWins += wins.otherWins;
                    otherWins += wins.activeWins;
                }

                cache[players] = (activeWins, otherWins);
            }

            return cache[players];
        }

        var wins = winCounts(Parse(input));

        // which player wins more:
        return Math.Max(wins.activeWins, wins.otherWins);
    }

    IEnumerable<int> DeterministicThrows() =>
        from i in Enumerable.Range(1, int.MaxValue)
        select (i - 1) % 100 + 1;

    IEnumerable<int> DiracThrows() =>
        from i in new[] { 1, 2, 3 }
        from j in new[] { 1, 2, 3 }
        from k in new[] { 1, 2, 3 }
        select i + j + k;

    (Player active, Player other) Parse(string input)
    {
        var players = (
            from line in input.Split("\n")
            let parts = line.Split(": ")
            select new Player(0, int.Parse(parts[1]))
        ).ToArray();
        return (players[0], players[1]);
    }
}