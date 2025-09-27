using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D23;

[ChallengeName("Amphipod")]
public class Y2021D23
{
    private readonly string _input = File.ReadAllText(@"Y2021\D23\Y2021D23-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => Solve(input);
    private object PartTwo(string input) => Solve(Upscale(input));

    string Upscale(string input)
    {
        var lines = input.Split("\n").ToList();
        lines.Insert(3, "  #D#C#B#A#");
        lines.Insert(4, "  #D#B#A#C#");
        return string.Join("\n", lines);
    }

    int Solve(string input)
    {
        var maze = Maze.Parse(input);

        var q = new PriorityQueue<Maze, int>();
        var cost = new Dictionary<Maze, int>();

        q.Enqueue(maze, 0);
        cost.Add(maze, 0);

        while (q.Count > 0)
        {
            maze = q.Dequeue();

            if (maze.Finished())
            {
                return cost[maze];
            }

            foreach (var n in Neighbours(maze))
            {
                if (cost[maze] + n.cost < cost.GetValueOrDefault(n.maze, int.MaxValue))
                {
                    cost[n.maze] = cost[maze] + n.cost;
                    q.Enqueue(n.maze, cost[n.maze]);
                }
            }
        }

        throw new Exception();
    }

    int stepCost(char actor)
    {
        return actor == 'A' ? 1 : actor == 'B' ? 10 : actor == 'C' ? 100 : 1000;
    }

    int getIcolDst(char ch)
    {
        return
            ch == 'A' ? 3 :
            ch == 'B' ? 5 :
            ch == 'C' ? 7 :
            ch == 'D' ? 9 :
            throw new Exception();
    }

    (Maze maze, int cost) HallwayToRoom(Maze maze)
    {
        for (var icol = 1; icol < 12; icol++)
        {
            var ch = maze.ItemAt(new Point(1, icol));

            if (ch == '.')
            {
                continue;
            }

            var icolDst = getIcolDst(ch);

            if (maze.CanMoveToDoor(icol, icolDst) && maze.CanEnterRoom(ch))
            {
                var steps = Math.Abs(icolDst - icol);
                var pt = new Point(1, icolDst);

                while (maze.ItemAt(pt.Below) == '.')
                {
                    pt = pt.Below;
                    steps++;
                }

                var l = HallwayToRoom(maze.Move(new Point(1, icol), pt));
                return (l.maze, l.cost + steps * stepCost(ch));
            }
        }

        return (maze, 0);
    }

    IEnumerable<(Maze maze, int cost)> RoomToHallway(Maze maze)
    {
        var hallwayColumns = new int[] { 1, 2, 4, 6, 8, 10, 11 };

        foreach (var roomColumn in new[] { 3, 5, 7, 9 })
        {
            if (maze.FinishedColumn(roomColumn))
            {
                continue;
            }

            var stepsV = 0;
            var ptSrc = new Point(1, roomColumn);
            while (maze.ItemAt(ptSrc) == '.')
            {
                ptSrc = ptSrc.Below;
                stepsV++;
            }

            var ch = maze.ItemAt(ptSrc);
            if (ch == '#')
            {
                continue;
            }

            foreach (var dj in new[] { -1, 1 })
            {
                var stepsH = 0;
                var ptDst = new Point(1, roomColumn);
                while (maze.ItemAt(ptDst) == '.')
                {
                    if (hallwayColumns.Contains(ptDst.icol))
                    {
                        yield return (maze.Move(ptSrc, ptDst), (stepsV + stepsH) * stepCost(ch));
                    }

                    if (dj == -1)
                    {
                        ptDst = ptDst.Left;
                    }
                    else
                    {
                        ptDst = ptDst.Right;
                    }

                    stepsH++;
                }
            }
        }
    }

    IEnumerable<(Maze maze, int cost)> Neighbours(Maze maze)
    {
        var hallwayToRoom = HallwayToRoom(maze);
        return hallwayToRoom.cost != 0 ? new[] { hallwayToRoom } : RoomToHallway(maze);
    }
}