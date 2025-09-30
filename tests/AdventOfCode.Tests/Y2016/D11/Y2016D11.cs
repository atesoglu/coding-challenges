using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D11;

[ChallengeName("Radioisotope Thermoelectric Generators")]
public class Y2016D11
{
    private readonly string _input = File.ReadAllText(@"Y2016\D11\Y2016D11-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(Parse(_input));

        output.Should().Be(33);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(Parse(_input)
            .AddGenerator(0, Element.Elerium).AddChip(0, Element.Elerium)
            .AddGenerator(0, Element.Dilithium).AddChip(0, Element.Dilithium)
        );

        output.Should().Be(57);
    }

    private int Solve(ulong state)
    {
        var steps = 0;
        var seen = new HashSet<ulong>();
        var q = new Queue<(int steps, ulong state)>();
        q.Enqueue((0, state));
        while (q.Any())
        {
            (steps, state) = q.Dequeue();

            if (state.Final())
            {
                return steps;
            }

            foreach (var nextState in state.NextStates())
            {
                if (!seen.Contains(nextState))
                {
                    q.Enqueue((steps + 1, nextState));
                    seen.Add(nextState);
                }
            }
        }

        return 0;
    }

    private ulong Parse(string input)
    {
        var nextMask = 1;
        var elementToMask = new Dictionary<string, int>();

        int mask(string element)
        {
            if (!elementToMask.ContainsKey(element))
            {
                if (elementToMask.Count() == 5)
                {
                    throw new NotImplementedException();
                }

                elementToMask[element] = nextMask;
                nextMask <<= 1;
            }

            return elementToMask[element];
        }

        ulong state = 0;
        var floor = 0;
        foreach (var line in input.Split('\n'))
        {
            var chips = (from m in Regex.Matches(line, @"(\w+)-compatible")
                let element = m.Groups[1].Value
                select mask(element)).Sum();

            var generators = (from m in Regex.Matches(line, @"(\w+) generator")
                let element = m.Groups[1].Value
                select mask(element)).Sum();
            state = state.SetFloor(floor, (ulong)chips, (ulong)generators);
            floor++;
        }

        return state;
    }
}

internal static class StateExtensions
{
    private const int elementCount = 7;  // now supports 7 elements
    private const int itemsPerFloor = 2 * elementCount;
    private const int floorCount = 4;
    private const int elevatorBits = 2;  // 0..3 floors, need 2 bits

    // floor bit offsets
    private static int[] floorShift = Enumerable.Range(0, floorCount).Select(f => f * itemsPerFloor).ToArray();
    private const int elevatorShift = floorCount * itemsPerFloor;

    // mask for clearing elevator bits
    private const ulong elevatorMask = ~((1UL << elevatorBits) - 1UL << elevatorShift);

    // mask for extracting chips/generators
    private static ulong floorMask(int floor) => ((1UL << itemsPerFloor) - 1UL) << floorShift[floor];

    public static ulong SetFloor(this ulong state, int floor, ulong chips, ulong generators) =>
        (state & ~floorMask(floor)) |
        (((generators) | (chips << elementCount)) << floorShift[floor]);

    public static ulong GetElevator(this ulong state) =>
        (state >> elevatorShift) & ((1UL << elevatorBits) - 1);

    public static ulong SetElevator(this ulong state, ulong elevator) =>
        (state & elevatorMask) | (elevator << elevatorShift);

    public static ulong GetChips(this ulong state, int floor) =>
        (state >> (floorShift[floor] + elementCount)) & ((1UL << elementCount) - 1);

    public static ulong GetGenerators(this ulong state, int floor) =>
        (state >> floorShift[floor]) & ((1UL << elementCount) - 1);

    public static ulong AddChip(this ulong state, int floor, Element chip) =>
        state | ((ulong)chip << (floorShift[floor] + elementCount));

    public static ulong AddGenerator(this ulong state, int floor, Element generator) =>
        state | ((ulong)generator << floorShift[floor]);

    public static bool Valid(this ulong state)
    {
        for (var floor = 0; floor < floorCount; floor++)
        {
            var chips = state.GetChips(floor);
            var generators = state.GetGenerators(floor);
            var unpairedChips = chips & ~generators;
            if (unpairedChips != 0 && generators != 0) return false;
        }
        return true;
    }

    public static IEnumerable<ulong> NextStates(this ulong state)
    {
        var floor = (int)state.GetElevator();
        var chips = state.GetChips(floor);
        var generators = state.GetGenerators(floor);

        // create list of item indices (0..2*elementCount-1)
        var items = new List<int>();
        for (var i = 0; i < elementCount; i++)
        {
            if (((chips >> i) & 1) != 0) items.Add(i);           // chips
            if (((generators >> i) & 1) != 0) items.Add(i + elementCount); // generators
        }

        // all combinations of 1 or 2 items
        foreach (var combo in Combinations(items, 1).Concat(Combinations(items, 2)))
        {
            foreach (var nextFloor in new[] { floor - 1, floor + 1 })
            {
                if (nextFloor < 0 || nextFloor >= floorCount) continue;

                var nextState = state;

                // remove items from current floor
                foreach (var bit in combo)
                {
                    if (bit < elementCount)
                        nextState &= ~(1UL << (floorShift[floor] + elementCount + bit)); // chip
                    else
                        nextState &= ~(1UL << (floorShift[floor] + bit - elementCount)); // generator
                }

                // add items to next floor
                foreach (var bit in combo)
                {
                    if (bit < elementCount)
                        nextState |= 1UL << (floorShift[nextFloor] + elementCount + bit); // chip
                    else
                        nextState |= 1UL << (floorShift[nextFloor] + bit - elementCount); // generator
                }

                nextState = nextState.SetElevator((ulong)nextFloor);

                if (nextState.Valid())
                    yield return nextState;
            }
        }
    }

    // helper: generate combinations of k elements
    private static IEnumerable<List<T>> Combinations<T>(List<T> list, int k)
    {
        if (k == 0) yield return new List<T>();
        else
        {
            for (var i = 0; i <= list.Count - k; i++)
            {
                foreach (var tail in Combinations(list.GetRange(i + 1, list.Count - (i + 1)), k - 1))
                {
                    tail.Insert(0, list[i]);
                    yield return tail;
                }
            }
        }
    }

    public static bool Final(this ulong state)
    {
        // all items on top floor
        for (var floor = 0; floor < floorCount - 1; floor++)
        {
            if ((state & floorMask(floor)) != 0) return false;
        }
        return true;
    }
}

internal enum Element
{
    Thulium = 0b1,
    Plutonium = 0b10,
    Strontium = 0b100,
    Promethium = 0b1000,
    Ruthenium = 0b10000,
    Elerium = 0b100000,
    Dilithium = 0b1000000
}