namespace AdventOfCode.Tests.Y2015.D02;

[ChallengeName("I Was Told There Would Be No Math")]
public class Y2015D02
{
    public int PartOne(int length, int width, int height)
    {
        var surface = (2 * length * width) + (2 * width * height) + (2 * height * length);

        // Calculate the area of the smallest side of the box, which is used as extra slack paper.
        // There are three sides: length*width, width*height, and height*length.
        // The smallest of these three areas is the extra paper needed.
        var slack = Math.Min(length * width, Math.Min(width * height, height * length));

        return surface + slack;
    }

    public int PartTwo(int length, int width, int height)
    {
        var dimensions = new[] { length, width, height };
        Array.Sort(dimensions);

        var smallestPerimeter = (dimensions[0] * 2) + (dimensions[1] * 2);
        var bowLength = dimensions[0] * dimensions[1] * dimensions[2];

        return smallestPerimeter + bowLength;
    }
}