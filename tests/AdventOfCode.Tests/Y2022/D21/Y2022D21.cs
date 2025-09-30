using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D21;

[ChallengeName("Monkey Math")]
public class Y2022D21
{
    private readonly string _input = File.ReadAllText(@"Y2022\D21\Y2022D21-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Parse(_input, "root", false).Simplify().ToString();

        output.Should().Be("41857219607906");
    }

    [Fact]
    public void PartTwo()
    {
        var expr = Parse(_input, "root", true) as Eq;

        while (!(expr.left is Var))
        {
            expr = Solve(expr);
        }


        var output = expr.right.ToString();

        output.Should().Be("3916936880448");
    }

    // One step in rearranging the equation to <variable> = <constant> form.
    // It is supposed that there is only one variable occurrence in the whole 
    // expression tree.
    private static Eq Solve(Eq eq) =>
        eq.left switch
        {
            Op(Const l, "+", Expr r) => new Eq(r, new Op(eq.right, "-", l).Simplify()),
            Op(Const l, "*", Expr r) => new Eq(r, new Op(eq.right, "/", l).Simplify()),
            Op(Expr l, "+", Expr r) => new Eq(l, new Op(eq.right, "-", r).Simplify()),
            Op(Expr l, "-", Expr r) => new Eq(l, new Op(eq.right, "+", r).Simplify()),
            Op(Expr l, "*", Expr r) => new Eq(l, new Op(eq.right, "/", r).Simplify()),
            Op(Expr l, "/", Expr r) => new Eq(l, new Op(eq.right, "*", r).Simplify()),
            Const => new Eq(eq.right, eq.left),
            _ => eq
        };

    // parses the input including the special rules for part2 
    // and returns the expression with the specified name
    private static Expr Parse(string input, string name, bool part2)
    {
        var context = new Dictionary<string, string[]>();
        foreach (var line in input.Split("\n"))
        {
            var parts = line.Split(" ");
            context[parts[0].TrimEnd(':')] = parts.Skip(1).ToArray();
        }

        Expr buildExpr(string name)
        {
            var parts = context[name];
            if (part2)
            {
                if (name == "humn")
                {
                    return new Var("humn");
                }
                else if (name == "root")
                {
                    return new Eq(buildExpr(parts[0]), buildExpr(parts[2]));
                }
            }

            if (parts.Length == 1)
            {
                return new Const(long.Parse(parts[0]));
            }
            else
            {
                return new Op(buildExpr(parts[0]), parts[1], buildExpr(parts[2]));
            }
        }

        return buildExpr(name);
    }

    // standard expression tree representation
    private interface Expr
    {
        Expr Simplify();
    }

    private record Const(long Value) : Expr
    {
        public override string ToString() => Value.ToString();
        public Expr Simplify() => this;
    }

    private record Var(string name) : Expr
    {
        public override string ToString() => name;
        public Expr Simplify() => this;
    }

    private record Eq(Expr left, Expr right) : Expr
    {
        public override string ToString() => $"{left} == {right}";
        public Expr Simplify() => new Eq(left.Simplify(), right.Simplify());
    }

    private record Op(Expr left, string op, Expr right) : Expr
    {
        public override string ToString() => $"({left}) {op} ({right})";

        public Expr Simplify()
        {
            return (left.Simplify(), op, right.Simplify()) switch
            {
                (Const l, "+", Const r) => new Const(l.Value + r.Value),
                (Const l, "-", Const r) => new Const(l.Value - r.Value),
                (Const l, "*", Const r) => new Const(l.Value * r.Value),
                (Const l, "/", Const r) => new Const(l.Value / r.Value),
                (Expr l, _, Expr r) => new Op(l, op, r),
            };
        }
    }
}