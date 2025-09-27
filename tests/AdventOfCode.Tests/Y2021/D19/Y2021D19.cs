using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D19;

[ChallengeName("Beacon Scanner")]
public class Y2021D19
{
    private readonly string _input = File.ReadAllText(@"Y2021\D19\Y2021D19-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) =>
        LocateScanners(input)
            .SelectMany(scanner => scanner.GetBeaconsInWorld())
            .Distinct()
            .Count();

    private object PartTwo(string input)
    {
        var scanners = LocateScanners(input);
        return (
            from sA in scanners
            from sB in scanners
            where sA != sB
            select
                Math.Abs(sA.center.x - sB.center.x) +
                Math.Abs(sA.center.y - sB.center.y) +
                Math.Abs(sA.center.z - sB.center.z)
        ).Max();
    }

    HashSet<Scanner> LocateScanners(string input)
    {
        var scanners = new HashSet<Scanner>(Parse(input));
        var locatedScanners = new HashSet<Scanner>();
        var q = new Queue<Scanner>();

        // when a scanner is located, it gets into the queue so that we can
        // explore its neighbours.

        locatedScanners.Add(scanners.First());
        q.Enqueue(scanners.First());

        scanners.Remove(scanners.First());

        while (q.Any())
        {
            var scannerA = q.Dequeue();
            foreach (var scannerB in scanners.ToArray())
            {
                var maybeLocatedScanner = TryToLocate(scannerA, scannerB);
                if (maybeLocatedScanner != null)
                {
                    locatedScanners.Add(maybeLocatedScanner);
                    q.Enqueue(maybeLocatedScanner);

                    scanners.Remove(scannerB); // sic! 
                }
            }
        }

        return locatedScanners;
    }

    Scanner TryToLocate(Scanner scannerA, Scanner scannerB)
    {
        var beaconsInA = scannerA.GetBeaconsInWorld().ToArray();

        foreach (var (beaconInA, beaconInB) in PotentialMatchingBeacons(scannerA, scannerB))
        {
            // now try to find the orientation for B:
            var rotatedB = scannerB;
            for (var rotation = 0; rotation < 24; rotation++, rotatedB = rotatedB.Rotate())
            {
                // Moving the rotated scanner so that beaconA and beaconB overlaps. Are there 12 matches? 
                var beaconInRotatedB = rotatedB.Transform(beaconInB);

                var locatedB = rotatedB.Translate(new Coord(
                    beaconInA.x - beaconInRotatedB.x,
                    beaconInA.y - beaconInRotatedB.y,
                    beaconInA.z - beaconInRotatedB.z
                ));

                if (locatedB.GetBeaconsInWorld().Intersect(beaconsInA).Count() >= 12)
                {
                    return locatedB;
                }
            }
        }

        // no luck
        return null;
    }

    IEnumerable<(Coord beaconInA, Coord beaconInB)> PotentialMatchingBeacons(Scanner scannerA, Scanner scannerB)
    {
        // If we had a matching beaconInA and beaconInB and moved the center
        // of the scanners to these then we would find at least 12 beacons 
        // with the same coordinates.

        // The only problem is that the rotation of scannerB is not fixed yet.

        // We need to make our check invariant to that:

        // After the translation, we could form a set from each scanner 
        // taking the absolute values of the x y and z coordinates of their beacons 
        // and compare those. 

        IEnumerable<int> absCoordinates(Scanner scanner) =>
            from coord in scanner.GetBeaconsInWorld()
            from v in new[] { coord.x, coord.y, coord.z }
            select Math.Abs(v);

        // This is the same no matter how we rotate scannerB, so the two sets should 
        // have at least 3 * 12 common values (with multiplicity).

        // ðŸ¦ We can also considerably speed up the search with the pigeonhole principle 
        // which says that it's enough to take all but 11 beacons from A and B. 
        // If there is no match amongst those, there cannot be 12 matching pairs:
        IEnumerable<T> pick<T>(IEnumerable<T> ts) => ts.Take(ts.Count() - 11);

        foreach (var beaconInA in pick(scannerA.GetBeaconsInWorld()))
        {
            var absA = absCoordinates(
                scannerA.Translate(new Coord(-beaconInA.x, -beaconInA.y, -beaconInA.z))
            ).ToHashSet();

            foreach (var beaconInB in pick(scannerB.GetBeaconsInWorld()))
            {
                var absB = absCoordinates(
                    scannerB.Translate(new Coord(-beaconInB.x, -beaconInB.y, -beaconInB.z))
                );

                if (absB.Count(d => absA.Contains(d)) >= 3 * 12)
                {
                    yield return (beaconInA, beaconInB);
                }
            }
        }
    }

    Scanner[] Parse(string input) => (
        from block in input.Split("\n\n")
        let beacons =
            from line in block.Split("\n").Skip(1)
            let parts = line.Split(",").Select(int.Parse).ToArray()
            select new Coord(parts[0], parts[1], parts[2])
        select new Scanner(new Coord(0, 0, 0), 0, beacons.ToList())
    ).ToArray();
}