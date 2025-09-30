using System.Text;
using FluentAssertions;
using Boxes = System.Collections.Generic.List<AdventOfCode.Tests.Y2023.D15.Lens>[];

namespace AdventOfCode.Tests.Y2023.D15;

internal record Lens(string label, int focalLength);

internal record Step(string label, int? focalLength);

[ChallengeName("Lens Library")]
public class Y2023D15
{
    private readonly string _input = File.ReadAllText(@"Y2023\D15\Y2023D15-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = _input.Split(',').Select(Hash).Sum();

        output.Should().Be(513643);
    }

    [Fact]
    public void PartTwo()
    {
        // "funcionally imperative of imperatively functional", only for ðŸŽ„
        var output = ParseSteps(_input).Aggregate(MakeBoxes(256), UpdateBoxes, GetFocusingPower);

        output.Should().Be(265345);
    }

    private Boxes UpdateBoxes(Boxes boxes, Step step)
    {
        var box = boxes[Hash(step.label)];
        var ilens = box.FindIndex(lens => lens.label == step.label);

        if (!step.focalLength.HasValue && ilens >= 0)
        {
            box.RemoveAt(ilens);
        }
        else if (step.focalLength.HasValue && ilens >= 0)
        {
            box[ilens] = new Lens(step.label, step.focalLength.Value);
        }
        else if (step.focalLength.HasValue && ilens < 0)
        {
            box.Add(new Lens(step.label, step.focalLength.Value));
        }

        return boxes;
    }

    private static IEnumerable<Step> ParseSteps(string input) =>
        from item in input.Split(',')
        let parts = item.Split('-', '=')
        select new Step(parts[0], parts[1] == "" ? null : int.Parse(parts[1]));

    private static Boxes MakeBoxes(int count) =>
        Enumerable.Range(0, count).Select(_ => new List<Lens>()).ToArray();

    private int GetFocusingPower(Boxes boxes) => (
        from ibox in Enumerable.Range(0, boxes.Length)
        from ilens in Enumerable.Range(0, boxes[ibox].Count)
        select (ibox + 1) * (ilens + 1) * boxes[ibox][ilens].focalLength
    ).Sum();

    private int Hash(string st) => st.Aggregate(0, (ch, a) => (ch + a) * 17 % 256);
}