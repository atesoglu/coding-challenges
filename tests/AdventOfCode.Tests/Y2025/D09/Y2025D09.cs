using System.Numerics;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D09;

[ChallengeName("Movie Theater")]
public class Y2025D09
{
    private readonly string _input = File.ReadAllText(@"Y2025\D09\Y2025D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var points = Parse(_input);
        var result = RectanglesByDescendingArea(points).First().Area;

        result.Should().Be(4750176210);
    }

    [Fact]
    public void PartTwo()
    {
        var points = Parse(_input);
        var boundarySegments = BuildBoundary(points).ToArray();

        var result = RectanglesByDescendingArea(points).First(r => boundarySegments.All(b => !r.Intersects(b))).Area;

        result.Should().Be(1574684850);
    }

    private static IEnumerable<Rectangle> RectanglesByDescendingArea(Complex[] pts) =>
        from a in pts
        from b in pts
        let r = Rectangle.FromPoints(a, b)
        orderby r.Area descending
        select r;

    // Simple closed-loop edges between consecutive points
    private static IEnumerable<Rectangle> BuildBoundary(Complex[] pts)
    {
        for (var i = 0; i < pts.Length; i++)
        {
            var a = pts[i];
            var b = pts[(i + 1) % pts.Length];
            yield return Rectangle.FromPoints(a, b);
        }
    }

    private static Complex[] Parse(string input) => input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
        .Select(line =>
        {
            var p = line.Split(',');
            return new Complex(int.Parse(p[0]), int.Parse(p[1]));
        })
        .ToArray();

    private record Rectangle(long Top, long Left, long Bottom, long Right)
    {
        public long Area => (Bottom - Top + 1) * (Right - Left + 1);

        public bool Intersects(Rectangle other) =>
            !(Right <= other.Left || // entirely left
              Left >= other.Right || // entirely right
              Bottom <= other.Top || // entirely above
              Top >= other.Bottom); // entirely below

        public static Rectangle FromPoints(Complex a, Complex b)
        {
            var top = (long)Math.Min(a.Imaginary, b.Imaginary);
            var bottom = (long)Math.Max(a.Imaginary, b.Imaginary);
            var left = (long)Math.Min(a.Real, b.Real);
            var right = (long)Math.Max(a.Real, b.Real);
            return new Rectangle(top, left, bottom, right);
        }
    }
}