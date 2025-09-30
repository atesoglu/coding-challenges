using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2016.D19;

[ChallengeName("An Elephant Named Joseph")]
public class Y2016D19
{
    private readonly string _input = File.ReadAllText(@"Y2016\D19\Y2016D19-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var elves = Elves(int.Parse(_input));
        var output = Solve(elves[0], elves[1], elves.Length, (elfVictim, count) => elfVictim.next.next);

        output.Should().Be(1830117);
    }

    [Fact]
    public void PartTwo()
    {

        var elves = Elves(int.Parse(_input));
        var output = Solve(elves[0], elves[elves.Length / 2], elves.Length, (elfVictim, count) => count % 2 == 1 ? elfVictim.next : elfVictim.next.next);

        output.Should().Be(1417887);
    }

    private int Solve(Elf elf, Elf elfVictim, int elfCount, Func<Elf, int, Elf> nextVictim)
    {
        while (elfCount > 1)
        {
            elfVictim.prev.next = elfVictim.next;
            elfVictim.next.prev = elfVictim.prev;
            elf = elf.next;
            elfCount--;
            elfVictim = nextVictim(elfVictim, elfCount);
        }

        return elf.id;
    }

    private Elf[] Elves(int count)
    {
        var elves = Enumerable.Range(0, count).Select(x => new Elf { id = x + 1 }).ToArray();
        for (var i = 0; i < count; i++)
        {
            elves[i].prev = elves[(i - 1 + count) % count];
            elves[i].next = elves[(i + 1) % count];
        }

        return elves;
    }

    private class Elf
    {
        public int id;
        public Elf prev;
        public Elf next;
    }
}