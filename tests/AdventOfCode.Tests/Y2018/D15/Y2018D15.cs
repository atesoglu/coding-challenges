using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2018.D15;

[ChallengeName("Beverage Bandits")]
public class Y2018D15
{
    private readonly string _input = File.ReadAllText(@"Y2018\D15\Y2018D15-input.txt", Encoding.UTF8);

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
        return Outcome(input, 3, 3, false).score;
    }

    private object PartTwo(string input)
    {
        var elfAp = 4;
        while (true)
        {
            var outcome = Outcome(input, 3, elfAp, false);
            if (outcome.noElfDied)
            {
                return outcome.score;
            }

            elfAp++;
        }
    }

    (bool noElfDied, int score) Outcome(string input, int goblinAp, int elfAp, bool tsto)
    {
        var game = Parse(input, goblinAp, elfAp);
        var elfCount = game.players.Count(player => player.elf);

        if (tsto)
        {
            Console.WriteLine(game.Tsto());
        }

        while (!game.Finished())
        {
            game.Step();
            if (tsto)
            {
                Console.WriteLine(game.Tsto());
            }
        }

        return (game.players.Count(p => p.elf) == elfCount, game.rounds * game.players.Select(player => player.hp).Sum());
    }


    Game Parse(string input, int goblinAp, int elfAp)
    {
        var players = new List<Player>();
        var lines = input.Split("\n");
        var mtx = new Block[lines.Length, lines[0].Length];

        var game = new Game { mtx = mtx, players = players };

        for (var irow = 0; irow < lines.Length; irow++)
        {
            for (var icol = 0; icol < lines[0].Length; icol++)
            {
                switch (lines[irow][icol])
                {
                    case '#':
                        mtx[irow, icol] = Wall.Block;
                        break;
                    case '.':
                        mtx[irow, icol] = Empty.Block;
                        break;
                    case var ch when ch == 'G' || ch == 'E':
                        var player = new Player
                        {
                            elf = ch == 'E',
                            ap = ch == 'E' ? elfAp : goblinAp,
                            pos = (irow, icol),
                            game = game
                        };
                        players.Add(player);
                        mtx[irow, icol] = player;
                        break;
                }
            }
        }

        return game;
    }
}