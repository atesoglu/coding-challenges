using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Tests.Y2018.D23;

[ChallengeName("Experimental Emergency Teleportation")]
public class Y2018D23
{
    private readonly string _input = File.ReadAllText(@"Y2018\D23\Y2018D23-input.txt", Encoding.UTF8);

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


    int Dist((int x, int y, int z) a, (int x, int y, int z) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);

    private object PartOne(string input)
    {
        var drones = Parse(input);
        var maxRange = drones.Select(drone => drone.r).Max();
        var maxDrone = drones.Single(drone => drone.r == maxRange);
        return drones.Count(drone => Dist(drone.pos, maxDrone.pos) <= maxRange);
    }

    IEnumerable<(int x, int y, int z)> Corners(Drone[] drones) => (
        from drone in drones
        from dx in new[] { -1, 0, 1 }
        from dy in new[] { -1, 0, 1 }
        from dz in new[] { -1, 0, 1 }
        where dx * dx + dy * dy + dz * dz == 1
        select (drone.pos.x + dx * drone.r, drone.pos.y + dy * drone.r, drone.pos.z + dz * drone.r)
    ).ToArray();

    Drone[] Parse(string input) => (
        from line in input.Split("\n")
        let parts = Regex.Matches(line, @"-?\d+").Select(x => int.Parse(x.Value)).ToArray()
        select new Drone((parts[0], parts[1], parts[2]), parts[3])
    ).ToArray();

    private object PartTwo(string input)
    {
        var drones = Parse(input);
        var minX = drones.Select(drone => drone.pos.x).Min();
        var minY = drones.Select(drone => drone.pos.y).Min();
        var minZ = drones.Select(drone => drone.pos.z).Min();

        var maxX = drones.Select(drone => drone.pos.x).Max();
        var maxY = drones.Select(drone => drone.pos.y).Max();
        var maxZ = drones.Select(drone => drone.pos.z).Max();

        return Solve(new Box((minX, minY, minZ), (maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1)), drones).pt;
    }

    (int drones, int pt) Solve(Box box, Drone[] drones)
    {
        var q = new PQueue<(int, int), (Box box, Drone[] drones)>();
        q.Enqueue((0, 0), (box, drones));

        while (q.Any())
        {
            (box, drones) = q.Dequeue();

            if (box.Size() == 1)
            {
                return (drones.Count(drone => drone.Contains(box)), box.Dist());
            }
            else
            {
                foreach (var subBox in box.Divide())
                {
                    var intersectingDrones = drones.Where(drone => drone.Intersects(subBox)).ToArray();
                    q.Enqueue((-intersectingDrones.Count(), subBox.Dist()), (subBox, intersectingDrones));
                }
            }
        }

        throw new Exception();
    }
}