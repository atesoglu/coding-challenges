using System.Text;
using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Tests.Y2020.D18;

[ChallengeName("Operation Order")]
public class Y2020D18
{
    private readonly string _input = File.ReadAllText(@"Y2020\D18\Y2020D18-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, true);

        output.Should().Be(464478013511);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, false);

        output.Should().Be(85660197232452);
    }

    private static long Solve(string input, bool part1)
    {
        var sum = 0L;
        foreach (var line in input.Split("\n"))
        {
            // https://en.wikipedia.org/wiki/Shunting-yard_algorithm

            var opStack = new Stack<char>();
            var valStack = new Stack<long>();

            void evalUntil(string ops)
            {
                while (!ops.Contains(opStack.Peek()))
                {
                    if (opStack.Pop() == '+')
                    {
                        valStack.Push(valStack.Pop() + valStack.Pop());
                    }
                    else
                    {
                        valStack.Push(valStack.Pop() * valStack.Pop());
                    }
                }
            }

            opStack.Push('(');

            foreach (var ch in line)
            {
                switch (ch)
                {
                    case ' ':
                        break;
                    case '*':
                        evalUntil("(");
                        opStack.Push('*');
                        break;
                    case '+':
                        evalUntil(part1 ? "(" : "(*");
                        opStack.Push('+');
                        break;
                    case '(':
                        opStack.Push('(');
                        break;
                    case ')':
                        evalUntil("(");
                        opStack.Pop();
                        break;
                    default:
                        valStack.Push(long.Parse(ch.ToString()));
                        break;
                }
            }

            evalUntil("(");

            sum += valStack.Single();
        }

        return sum;
    }
}