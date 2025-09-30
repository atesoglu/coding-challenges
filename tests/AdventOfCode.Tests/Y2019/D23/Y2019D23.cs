using System.Diagnostics;
using System.Text;
using AdventOfCode.Tests.Y2019.D02;
using FluentAssertions;
using Packets = System.Collections.Generic.List<(long address, long x, long y)>;

namespace AdventOfCode.Tests.Y2019.D23;

[ChallengeName("Category Six")]
public class Y2019D23
{
    private readonly string _input = File.ReadAllText(@"Y2019\D23\Y2019D23-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, false);

        output.Should().Be(23213);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, true);

        output.Should().Be(17874);
    }


    private long Solve(string input, bool hasNat)
    {
        var machines = (
            from address in Enumerable.Range(0, 50)
            select Nic(input, address)
        ).ToList();

        var natAddress = 255;

        if (hasNat)
        {
            machines.Add(Nat(natAddress));
        }

        var packets = new Packets();
        while (!packets.Any(packet => packet.address == natAddress))
        {
            foreach (var machine in machines)
            {
                packets = machine(packets);
            }
        }

        return packets.Single(packet => packet.address == natAddress).y;
    }

    private static (List<long> data, Packets packets) Receive(Packets packets, int address)
    {
        var filteredPackets = new Packets();
        var data = new List<long>();
        foreach (var packet in packets)
        {
            if (packet.address == address)
            {
                data.Add(packet.x);
                data.Add(packet.y);
            }
            else
            {
                filteredPackets.Add(packet);
            }
        }

        return (data, filteredPackets);
    }

    private Func<Packets, Packets> Nic(string program, int address)
    {
        var icm = new IntCodeMachine(program);
        var output = icm.Run(address);
        Debug.Assert(output.Count == 0);

        return (input) =>
        {
            var (data, packets) = Receive(input, address);
            if (!data.Any())
            {
                data.Add(-1);
            }

            var output = icm.Run(data.ToArray());
            for (var d = 0; d < output.Count; d += 3)
            {
                packets.Add((output[d], output[d + 1], output[d + 2]));
            }

            return packets;
        };
    }

    private Func<Packets, Packets> Nat(int address)
    {
        long? yLastSent = null;
        long? x = null;
        long? y = null;
        return (input) =>
        {
            var (data, packets) = Receive(input, address);
            if (data.Any())
            {
                (x, y) = (data[^2], data[^1]);
            }

            if (packets.Count == 0)
            {
                Debug.Assert(x.HasValue && y.HasValue);
                packets.Add((y == yLastSent ? 255 : 0, x.Value, y.Value));
                yLastSent = y;
            }

            return packets;
        };
    }
}