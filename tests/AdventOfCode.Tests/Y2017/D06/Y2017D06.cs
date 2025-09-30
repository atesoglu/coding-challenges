using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2017.D06;

[ChallengeName("Memory Reallocation")]
public class Y2017D06
{
    private readonly string _input = File.ReadAllText(@"Y2017\D06\Y2017D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetStepCount(Parse(_input));

        output.Should().Be(3156);
    }

    [Fact]
    public void PartTwo()
    {
        var numbers = Parse(_input);
        GetStepCount(numbers);

        var output = GetStepCount(numbers);

        output.Should().Be(1610);
    }


    private static List<int> Parse(string input) => input.Split('\t').Select(int.Parse).ToList();

    private int GetStepCount(List<int> numbers)
    {
        var stepCount = 0;
        var seen = new HashSet<string>();
        while (true)
        {
            var key = string.Join(";", numbers.Select(x => x.ToString()));
            if (seen.Contains(key))
            {
                return stepCount;
            }

            seen.Add(key);
            Redistribute(numbers);
            stepCount++;
        }
    }

    private static void Redistribute(List<int> numbers)
    {
        var max = numbers.Max();
        var i = numbers.IndexOf(max);
        numbers[i] = 0;
        while (max > 0)
        {
            i++;
            numbers[i % numbers.Count]++;
            max--;
        }
    }
}