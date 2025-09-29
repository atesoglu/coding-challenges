using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2021.D04;

[ChallengeName("Giant Squid ")]
public class Y2021D04
{
    private readonly string _input = File.ReadAllText(@"Y2021\D04\Y2021D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = BoardsInOrderOfCompletion(_input).First().score;

        output.Should().Be(39984);
    }

    [Fact]
    public void PartTwo()
    {
        var output = BoardsInOrderOfCompletion(_input).Last().score;

        output.Should().Be(8468);
    }


    IEnumerable<BingoBoard> BoardsInOrderOfCompletion(string input)
    {
        var blocks = input.Split("\n\n");

        // first block contains the numbers to be drawn, rest describe bingo boards:
        var numbers = blocks[0].Split(",");
        var boards = (from block in blocks.Skip(1) select new BingoBoard(block)).ToHashSet();

        // let's play the game
        foreach (var number in numbers)
        {
            foreach (var board in boards.ToArray())
            {
                board.AddNumber(number);
                if (board.score > 0)
                {
                    yield return board;
                    boards.Remove(board);
                }
            }
        }
    }
}

record Cell(string number, bool marked = false);

// Let's be ho-ho-hoOOP this time.
class BingoBoard
{
    public int score { get; private set; }
    private List<Cell> cells;

    IEnumerable<Cell> CellsInRow(int irow) =>
        from icol in Enumerable.Range(0, 5) select cells[irow * 5 + icol];

    IEnumerable<Cell> CellsInCol(int icol) =>
        from irow in Enumerable.Range(0, 5) select cells[irow * 5 + icol];

    public BingoBoard(string st)
    {
        // split the input into words & read them numbers into cells
        cells = (
            from word in st.Split(" \n".ToArray(), StringSplitOptions.RemoveEmptyEntries)
            select new Cell(word)
        ).ToList();
    }

    public void AddNumber(string number)
    {
        var icell = cells.FindIndex(cell => cell.number == number);

        if (icell >= 0)
        {
            // mark the cell
            cells[icell] = cells[icell] with { marked = true };

            // if the board is completed, compute score
            for (var i = 0; i < 5; i++)
            {
                if (
                    CellsInRow(i).All(cell => cell.marked) ||
                    CellsInCol(i).All(cell => cell.marked)
                )
                {
                    var unmarkedNumbers =
                        from cell in cells where !cell.marked select int.Parse(cell.number);

                    score = int.Parse(number) * unmarkedNumbers.Sum();
                }
            }
        }
    }
}