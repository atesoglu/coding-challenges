using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2018.D13;

[ChallengeName("Mine Cart Madness")]
public class Y2018D13
{
    private readonly string _input = File.ReadAllText(@"Y2018\D13\Y2018D13-input.txt", Encoding.UTF8);

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


    private object PartOne(string input)
    {
        var (mat, carts) = Parse(input);
        while (true)
        {
            var newState = Step(mat, carts);
            if (newState.crashed.Any())
            {
                return Tsto(newState.crashed[0]);
            }
        }
    }

    private object PartTwo(string input)
    {
        var (mat, carts) = Parse(input);
        while (carts.Count > 1)
        {
            var newState = Step(mat, carts);
            carts = newState.carts;
        }

        return Tsto(carts[0]);
    }

    string Tsto(Cart cart) => $"{cart.pos.icol},{cart.pos.irow}";

    (List<Cart> crashed, List<Cart> carts) Step(string[] mat, List<Cart> carts)
    {
        var crashed = new List<Cart>();

        foreach (var cart in carts.OrderBy((cartT) => cartT.pos))
        {
            cart.pos = (irow: cart.pos.irow + cart.drow, icol: cart.pos.icol + cart.dcol);

            foreach (var cart2 in carts.ToArray())
            {
                if (cart != cart2 && cart.pos.irow == cart2.pos.irow && cart.pos.icol == cart2.pos.icol)
                {
                    crashed.Add(cart);
                    crashed.Add(cart2);
                }
            }

            switch (mat[cart.pos.irow][cart.pos.icol])
            {
                case '\\':
                    if (cart.dcol == 1 || cart.dcol == -1)
                    {
                        cart.Rotate(Dir.Right);
                    }
                    else if (cart.drow == -1 || cart.drow == 1)
                    {
                        cart.Rotate(Dir.Left);
                    }
                    else
                    {
                        throw new Exception();
                    }

                    break;
                case '/':
                    if (cart.dcol == 1 || cart.dcol == -1)
                    {
                        cart.Rotate(Dir.Left);
                    }
                    else if (cart.drow == 1 || cart.drow == -1)
                    {
                        cart.Rotate(Dir.Right);
                    }

                    break;
                case '+':
                    cart.Turn();
                    break;
            }
        }

        return (crashed, carts.Where(cart => !crashed.Contains(cart)).ToList());
    }

    (string[] mat, List<Cart> carts) Parse(string input)
    {
        var mat = input.Split("\n");
        var crow = mat.Length;
        var ccol = mat[0].Length;

        var carts = new List<Cart>();
        for (var irow = 0; irow < crow; irow++)
        {
            for (var icol = 0; icol < ccol; icol++)
            {
                var ch = mat[irow][icol];
                switch (ch)
                {
                    case '^':
                        carts.Add(new Cart { pos = (irow: irow, icol: icol), dcol = 0, drow = -1 });
                        break;
                    case 'v':
                        carts.Add(new Cart { pos = (irow: irow, icol: icol), dcol = 0, drow = 1 });
                        break;
                    case '<':
                        carts.Add(new Cart { pos = (irow: irow, icol: icol), dcol = -1, drow = 0 });
                        break;
                    case '>':
                        carts.Add(new Cart { pos = (irow: irow, icol: icol), dcol = 1, drow = 0 });
                        break;
                }
            }
        }

        return (mat, carts);
    }
}