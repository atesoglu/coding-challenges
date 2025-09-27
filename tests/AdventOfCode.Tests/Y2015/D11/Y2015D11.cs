using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D11;

[ChallengeName("Corporate Policy")]
public class Y2015D11
{
    private readonly string _input = File.ReadAllText(@"Y2015\D11\Y2015D11-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = NextValidPassword(_input);

        output.Should().Be("hxbxxyzz");
    }

    [Fact]
    public void PartTwo()
    {
        var output = NextValidPassword("hxbxxyzz");

        output.Should().Be("hxcaabcc");
    }

    private static string NextValidPassword(string password)
    {
        do
        {
            password = Increment(password);
        } while (!IsValid(password));

        return password;
    }

    private static string Increment(string password)
    {
        var chars = password.ToCharArray();

        for (var i = chars.Length - 1; i >= 0; i--)
        {
            if (chars[i] == 'z')
            {
                chars[i] = 'a';
            }
            else
            {
                chars[i]++;

                // Skip invalid letters
                if (chars[i] == 'i' || chars[i] == 'o' || chars[i] == 'l')
                    chars[i]++;

                // Reset all letters to the right to 'a'
                for (var j = i + 1; j < chars.Length; j++)
                    chars[j] = 'a';

                break;
            }
        }

        return new string(chars);
    }

    private static bool IsValid(string password)
    {
        return HasStraight(password) && !ContainsInvalidChars(password) && HasTwoPairs(password);
    }

    private static bool HasStraight(string password)
    {
        for (var i = 0; i < password.Length - 2; i++)
        {
            if (password[i + 1] == password[i] + 1 &&
                password[i + 2] == password[i] + 2)
            {
                return true;
            }
        }

        return false;
    }

    private static bool ContainsInvalidChars(string password) => password.Any(c => "iol".Contains(c));

    private static bool HasTwoPairs(string password)
    {
        var count = 0;
        var i = 0;
        while (i < password.Length - 1)
        {
            if (password[i] == password[i + 1])
            {
                count++;
                i += 2; // skip next char to avoid overlapping pairs
            }
            else
            {
                i++;
            }
        }

        return count >= 2;
    }
}