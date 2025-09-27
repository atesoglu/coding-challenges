using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D04;

[ChallengeName("Giant Squid ")]
public class Y2021D04
{
    private readonly string _input = File.ReadAllText(@"Y2021\D04\Y2021D04-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => BoardsInOrderOfCompletion(input).First().score;
    private object PartTwo(string input) => BoardsInOrderOfCompletion(input).Last().score;

    IEnumerable<BingoBoard> BoardsInOrderOfCompletion(string input)
    {
        var blocks = input.Split("\n\n");

        // first block contains the numbers to be drawn, rest describe bingo boards:
        var numbers = blocks[0].Split(",");
        var boards = (from block in blocks.Skip(1) select new BingoBoard(block)).ToHashSet();

        // let's play the game
        foreach (var number in numbers)
        {
            foreach (var board in boards.ToArray())
            {
                board.AddNumber(number);
                if (board.score > 0)
                {
                    yield return board;
                    boards.Remove(board);
                }
            }
        }
    }
}