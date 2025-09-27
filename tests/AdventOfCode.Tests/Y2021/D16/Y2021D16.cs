using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D16;

[ChallengeName("Packet Decoder")]
public class Y2021D16
{
    private readonly string _input = File.ReadAllText(@"Y2021\D16\Y2021D16-input.txt", Encoding.UTF8);

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
        GetTotalVersion(GetPacket(GetReader(input)));

    private object PartTwo(string input) =>
        Evaluate(GetPacket(GetReader(input)));

    // recursively sum the versions of a packet and its content for part 1:
    int GetTotalVersion(Packet packet) =>
        packet.version + packet.packets.Select(GetTotalVersion).Sum();

    // recursively evaluate the packet and its contents based on the type tag for part 2:
    long Evaluate(Packet packet)
    {
        var parts = packet.packets.Select(Evaluate).ToArray();
        return packet.type switch
        {
            0 => parts.Sum(),
            1 => parts.Aggregate(1L, (acc, x) => acc * x),
            2 => parts.Min(),
            3 => parts.Max(),
            4 => packet.payload, // <--- literal packet is handled uniformly
            5 => parts[0] > parts[1] ? 1 : 0,
            6 => parts[0] < parts[1] ? 1 : 0,
            7 => parts[0] == parts[1] ? 1 : 0,
            _ => throw new Exception()
        };
    }

    // convert hex string to bit sequence reader
    BitSequenceReader GetReader(string input) => new BitSequenceReader(
        new BitArray((
                from hexChar in input
                // get the 4 bits out of a hex char:
                let value = Convert.ToInt32(hexChar.ToString(), 16)
                // convert to bitmask
                from mask in new[] { 8, 4, 2, 1 }
                select (mask & value) != 0
            ).ToArray()
        ));

    // make sense of the bit sequence:
    Packet GetPacket(BitSequenceReader reader)
    {
        var version = reader.ReadInt(3);
        var type = reader.ReadInt(3);
        var packets = new List<Packet>();
        var payload = 0L;

        if (type == 0x4)
        {
            // literal, payload is encoded in the following bits in 5 bit long chunks:
            while (true)
            {
                var isLast = reader.ReadInt(1) == 0;
                payload = payload * 16 + reader.ReadInt(4);
                if (isLast)
                {
                    break;
                }
            }
        }
        else if (reader.ReadInt(1) == 0)
        {
            // operator, the next 'length' long bit sequence encodes the sub packages:
            var length = reader.ReadInt(15);
            var subPackages = reader.GetBitSequenceReader(length);
            while (subPackages.Any())
            {
                packets.Add(GetPacket(subPackages));
            }
        }
        else
        {
            // operator with 'packetCount' sub packages:
            var packetCount = reader.ReadInt(11);
            packets.AddRange(from _ in Enumerable.Range(0, packetCount) select GetPacket(reader));
        }

        return new Packet(version, type, payload, packets.ToArray());
    }
}