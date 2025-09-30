using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Tests.Y2019.D02;

namespace AdventOfCode.Tests.Y2019.D25;

[ChallengeName("Cryostasis")]
public class Y2019D25
{
    private readonly string _input = File.ReadAllText(@"Y2019\D25\Y2019D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = SolvePartOne(_input);

        output.Should().Be(268468864);
    }


    private object SolvePartOne(string input)
    {
        var securityRoom = "== Security Checkpoint ==";
        var icm = new IntCodeMachine(input);
        var description = icm.Run().ToAscii();

        VisitRooms(securityRoom, icm, description, args =>
        {
            foreach (var item in args.items)
            {
                if (item != "infinite loop")
                {
                    var takeCmd = "take " + item;
                    var clone = icm.Clone();
                    clone.Run(takeCmd);
                    if (!clone.Halted() && Inventory(clone).Contains(item))
                    {
                        icm.Run(takeCmd);
                    }
                }
            }

            return null;
        });

        var door = VisitRooms(securityRoom, icm, description, args =>
            args.room == securityRoom ? args.doors.Single(door => door != ReverseDir(args.doorTaken)) : null);

        var r = new Random();

        void TakeOrDrop(string cmd, List<string> from, List<string> to)
        {
            var i = r.Next(from.Count);
            var item = from[i];
            from.RemoveAt(i);
            to.Add(item);
            icm.Run(cmd + " " + item);
        }

        var inventory = Inventory(icm).ToList();
        var floor = new List<string>();
        while (true)
        {
            var output = icm.Run(door).ToAscii();
            if (output.Contains("heavier"))
            {
                TakeOrDrop("take", floor, inventory);
            }
            else if (output.Contains("lighter"))
            {
                TakeOrDrop("drop", inventory, floor);
            }
            else
            {
                return long.Parse(Regex.Match(output, @"\d+").Value);
            }
        }
    }

    private List<string> directions = new List<string>() { "south", "east", "west", "north" };
    private string ReverseDir(string direction) => directions[3 - directions.IndexOf(direction)];

    private string VisitRooms(
        string securityRoom,
        IntCodeMachine icm,
        string description,
        Func<(IEnumerable<string> items, string room, string doorTaken, IEnumerable<string> doors), string> callback
    )
    {
        var roomsSeen = new HashSet<string>();

        string DFS(string description, string doorTaken)
        {
            var room = description.Split("\n").Single(x => x.Contains("=="));
            var listing = GetListItems(description).ToHashSet();
            var doors = listing.Intersect(directions);
            var items = listing.Except(doors);

            if (!roomsSeen.Contains(room))
            {
                roomsSeen.Add(room);

                var res = callback((items, room, doorTaken, doors));
                if (res != null)
                {
                    return res;
                }

                if (room != securityRoom)
                {
                    foreach (var door in doors)
                    {
                        res = DFS(icm.Run(door).ToAscii(), door);
                        if (res != null)
                        {
                            return res;
                        }

                        icm.Run(ReverseDir(door));
                    }
                }
            }

            return null;
        }

        return DFS(description, null);
    }

    private IEnumerable<string> Inventory(IntCodeMachine icm) => GetListItems(icm.Run("inv").ToAscii());

    private static IEnumerable<string> GetListItems(string description) =>
        from line in description.Split("\n")
        where line.StartsWith("- ")
        select line.Substring(2);
}