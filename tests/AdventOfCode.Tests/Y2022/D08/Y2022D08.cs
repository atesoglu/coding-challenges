using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D08;

[ChallengeName("Treetop Tree House")]
public class Y2022D08
{
    private readonly string _input = File.ReadAllText(@"Y2022\D08\Y2022D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var forest = Parse(_input);

        var output = forest.Trees().Count(tree =>
            forest.IsTallest(tree, Left) || forest.IsTallest(tree, Right) ||
            forest.IsTallest(tree, Up) || forest.IsTallest(tree, Down)
        );

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var forest = Parse(_input);

        var output = forest.Trees().Select(tree =>
            forest.ViewDistance(tree, Left) * forest.ViewDistance(tree, Right) *
            forest.ViewDistance(tree, Up) * forest.ViewDistance(tree, Down)
        ).Max();

        output.Should().Be(0);
    }


    static Direction Left = new Direction(0, -1);
    static Direction Right = new Direction(0, 1);
    static Direction Up = new Direction(-1, 0);
    static Direction Down = new Direction(1, 0);

    Forest Parse(string input)
    {
        var items = input.Split("\n");
        var (ccol, crow) = (items[0].Length, items.Length);
        return new Forest(items, crow, ccol);
    }

    record Direction(int drow, int dcol);

    record Tree(int height, int irow, int icol);

    record Forest(string[] items, int crow, int ccol)
    {
        public IEnumerable<Tree> Trees() =>
            from irow in Enumerable.Range(0, crow)
            from icol in Enumerable.Range(0, ccol)
            select new Tree(items[irow][icol], irow, icol);

        public int ViewDistance(Tree tree, Direction dir) =>
            IsTallest(tree, dir)
                ? TreesInDirection(tree, dir).Count()
                : SmallerTrees(tree, dir).Count() + 1;

        public bool IsTallest(Tree tree, Direction dir) =>
            TreesInDirection(tree, dir).All(treeT => treeT.height < tree.height);

        IEnumerable<Tree> SmallerTrees(Tree tree, Direction dir) =>
            TreesInDirection(tree, dir).TakeWhile(treeT => treeT.height < tree.height);

        IEnumerable<Tree> TreesInDirection(Tree tree, Direction dir)
        {
            var (first, irow, icol) = (true, tree.irow, tree.icol);
            while (irow >= 0 && irow < crow && icol >= 0 && icol < ccol)
            {
                if (!first)
                {
                    yield return new Tree(height: items[irow][icol], irow: irow, icol: icol);
                }

                (first, irow, icol) = (false, irow + dir.drow, icol + dir.dcol);
            }
        }
    }
}