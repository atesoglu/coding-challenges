namespace AdventOfCode.Tests.Y2015.D02;

/// <summary>
/// --- Day 2: I Was Told There Would Be No Math ---
///
/// The elves are running low on wrapping paper, and so they need to submit an order for more.
/// They have a list of the dimensions (length l, width w, and height h) of each present, and only want to order exactly as much as they need.
/// </summary>
public static class Y2015D02IWasToldThereWouldBeNoMath
{
    /// <summary>
    /// Fortunately, every present is a box (a perfect right rectangular prism),
    /// which makes calculating the required wrapping paper for each gift a little easier: find the surface area of the box, which is 2*l*w + 2*w*h + 2*h*l.
    /// The elves also need a little extra paper for each present: the area of the smallest side.
    ///
    /// For example:
    /// A present with dimensions 2x3x4 requires 2*6 + 2*12 + 2*8 = 52 square feet of wrapping paper plus 6 square feet of slack, for a total of 58 square feet.
    /// A present with dimensions 1x1x10 requires 2*1 + 2*10 + 2*10 = 42 square feet of wrapping paper plus 1 square foot of slack, for a total of 43 square feet.
    ///
    /// All numbers in the elves' list are in feet.
    ///
    /// Question: How many total square feet of wrapping paper should they order?
    /// </summary>
    /// <param name="length">The length of the present in feet.</param>
    /// <param name="width">The width of the present in feet.</param>
    /// <param name="height">The height of the present in feet.</param>
    /// <returns>The total square feet of wrapping paper required for the present, including extra for slack.</returns>
    public static int PartOne(int length, int width, int height)
    {
        var surface = (2 * length * width) + (2 * width * height) + (2 * height * length);

        // Calculate the area of the smallest side of the box, which is used as extra slack paper.
        // There are three sides: length*width, width*height, and height*length.
        // The smallest of these three areas is the extra paper needed.
        var slack = Math.Min(length * width, Math.Min(width * height, height * length));

        return surface + slack;
    }

    /// <summary>
    /// The elves are running low on ribbon.
    /// Ribbon is all the same width, so they only have to worry about the length they need to order, which they would like to be exact.
    ///
    /// The ribbon required to wrap a present is the shortest distance around its sides, or the smallest perimeter of any one face.
    /// Each present also requires a bow made out of ribbon; the feet of ribbon required for the perfect bow is equal to the cubic feet of volume of the present.
    ///
    /// For example:
    /// A present with dimensions 2x3x4 requires 2+2+3+3 = 10 feet of ribbon to wrap the present plus 2*3*4 = 24 feet of ribbon for the bow, for a total of 34 feet.
    /// A present with dimensions 1x1x10 requires 1+1+1+1 = 4 feet of ribbon to wrap the present plus 1*1*10 = 10 feet of ribbon for the bow, for a total of 14 feet.
    ///
    /// Question: How many total feet of ribbon should they order?
    /// </summary>
    /// <param name="length">The length of the present in feet.</param>
    /// <param name="width">The width of the present in feet.</param>
    /// <param name="height">The height of the present in feet.</param>
    /// <returns>The total feet of ribbon required for the present, including the ribbon for the bow.</returns>
    public static int PartTwo(int length, int width, int height)
    {
        var dimensions = new[] { length, width, height };
        Array.Sort(dimensions);

        var smallestPerimeter = (dimensions[0] * 2) + (dimensions[1] * 2);
        var bowLength = dimensions[0] *  dimensions[1] * dimensions[2];

        return smallestPerimeter + bowLength;
    }
}