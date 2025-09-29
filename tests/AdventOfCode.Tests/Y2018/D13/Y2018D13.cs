using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D13;

[ChallengeName("Mine Cart Madness")]
public class Y2018D13
{
    private readonly string _input = File.ReadAllText(@"Y2018\D13\Y2018D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (mat, carts) = Parse(_input);
        List<Cart> crashedCarts;

        while (true)
        {
            (crashedCarts, carts) = Step(mat, carts);
            if (crashedCarts.Any())
            {
                var first = crashedCarts.OrderBy(c => c.pos.irow).ThenBy(c => c.pos.icol).First();
                var output = Tsto(first);
                output.Should().Be("41,22");
                break;
            }
        }
    }

    [Fact]
    public void PartTwo()
    {
        var (mat, carts) = Parse(_input);

        while (carts.Count > 1)
        {
            var (_, remainingCarts) = Step(mat, carts);
            carts = remainingCarts;
        }

        var output = Tsto(carts[0]);
        output.Should().Be("84,90");
    }

    private string Tsto(Cart cart) => $"{cart.pos.icol},{cart.pos.irow}";

    private (List<Cart> crashed, List<Cart> remainingCarts) Step(string[] mat, List<Cart> carts)
    {
        var crashed = new HashSet<Cart>();
        var remaining = carts.ToList();

        foreach (var cart in carts.OrderBy(c => c.pos.irow).ThenBy(c => c.pos.icol))
        {
            if (crashed.Contains(cart)) continue;

            // Move the cart
            cart.pos = (cart.pos.irow + cart.drow, cart.pos.icol + cart.dcol);

            // Check bounds
            if (cart.pos.irow < 0 || cart.pos.irow >= mat.Length ||
                cart.pos.icol < 0 || cart.pos.icol >= mat[cart.pos.irow].Length)
                throw new Exception($"Cart went out of bounds at {cart.pos.irow},{cart.pos.icol}");

            // Check for collisions
            var collided = remaining.FirstOrDefault(c => c != cart && c.pos == cart.pos);
            if (collided != null)
            {
                crashed.Add(cart);
                crashed.Add(collided);
                remaining.Remove(cart);
                remaining.Remove(collided);
                continue;
            }

            // Update cart direction based on track
            var track = mat[cart.pos.irow][cart.pos.icol];
            switch (track)
            {
                case '\\':
                    if (cart.dcol != 0)
                        cart.Rotate(Dir.Right);
                    else
                        cart.Rotate(Dir.Left);
                    break;
                case '/':
                    if (cart.dcol != 0)
                        cart.Rotate(Dir.Left);
                    else
                        cart.Rotate(Dir.Right);
                    break;
                case '+':
                    cart.Turn();
                    break;
            }
        }

        return (crashed.ToList(), remaining);
    }

    private (string[] mat, List<Cart> carts) Parse(string input)
    {
        var mat = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        var carts = new List<Cart>();

        for (var irow = 0; irow < mat.Length; irow++)
        {
            for (var icol = 0; icol < mat[irow].Length; icol++)
            {
                var ch = mat[irow][icol];
                switch (ch)
                {
                    case '^': carts.Add(new Cart { pos = (irow, icol), drow = -1, dcol = 0 }); break;
                    case 'v': carts.Add(new Cart { pos = (irow, icol), drow = 1, dcol = 0 }); break;
                    case '<': carts.Add(new Cart { pos = (irow, icol), drow = 0, dcol = -1 }); break;
                    case '>': carts.Add(new Cart { pos = (irow, icol), drow = 0, dcol = 1 }); break;
                }
            }
        }

        return (mat, carts);
    }
}

enum Dir { Left, Forward, Right }

class Cart
{
    public (int irow, int icol) pos;
    public int drow;
    public int dcol;
    private Dir nextTurn = Dir.Left;

    public void Rotate(Dir dir)
    {
        (drow, dcol) = dir switch
        {
            Dir.Left => (-dcol, drow),
            Dir.Right => (dcol, -drow),
            Dir.Forward => (drow, dcol),
            _ => throw new ArgumentException()
        };
    }

    public void Turn()
    {
        Rotate(nextTurn);
        nextTurn = (Dir)(((int)nextTurn + 1) % 3);
    }
}
