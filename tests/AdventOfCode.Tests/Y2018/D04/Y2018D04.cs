using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D04;

[ChallengeName("Repose Record")]
public class Y2018D04
{
    private readonly string _input = File.ReadAllText(@"Y2018\D04\Y2018D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var foo = from day in Parse(_input)
            group day by day.guard
            into g
            select new
            {
                guard = g.Key,
                totalSleeps = g.Select(day => day.totalSleep).Sum(),
                sleepByMin = Enumerable.Range(0, 60).Select(minT => g.Sum(day => day.sleep[minT])).ToArray()
            };
        var maxSleep = foo.Max(x => x.totalSleeps);
        var fooT = foo.Single(g => g.totalSleeps == maxSleep);
        var maxSleepByMin = Enumerable.Range(0, 60).Max(minT => fooT.sleepByMin[minT]);
        var min = Enumerable.Range(0, 60).Single(minT => fooT.sleepByMin[minT] == maxSleepByMin);

        var output = fooT.guard * min;

        output.Should().Be(76357);
    }

    [Fact]
    public void PartTwo()
    {
        var foo = from day in Parse(_input)
            group day by day.guard
            into g
            select new
            {
                guard = g.Key,
                totalSleeps = g.Select(day => day.totalSleep).Sum(),
                sleepByMin = Enumerable.Range(0, 60).Select(minT => g.Sum(day => day.sleep[minT])).ToArray()
            };

        var maxMaxSleep = foo.Max(x => x.sleepByMin.Max());
        var fooT = foo.Single(x => x.sleepByMin.Max() == maxMaxSleep);
        var min = Enumerable.Range(0, 60).Single(minT => fooT.sleepByMin[minT] == maxMaxSleep);

        var output = fooT.guard * min;

        output.Should().Be(41668);
    }

    IEnumerable<Day> Parse(string input)
    {
        var lines = input.Split("\n").ToList();
        lines.Sort((x, y) => DateTime.Parse(x.Substring(1, "1518-03-25 00:01".Length)).CompareTo(DateTime.Parse(y.Substring(1, "1518-03-25 00:01".Length))));
        var iline = 0;

        while (Int(@"Guard #(\d+) begins shift", out var guard))
        {
            var sleep = new int[60];
            while (Date(@"\[(.*)\] falls asleep", out var fallsAsleap))
            {
                Date(@"\[(.*)\] wakes up", out var wakesUp);

                var from = fallsAsleap.Hour != 0 ? 0 : fallsAsleap.Minute;
                var to = wakesUp.Hour != 0 ? 0 : wakesUp.Minute;

                for (var min = from; min < to; min++)
                {
                    sleep[min] = 1;
                }
            }

            yield return new Day() { guard = guard, sleep = sleep };
        }

        if (iline != lines.Count)
        {
            throw new Exception();
        }

        bool Int(string pattern, out int r)
        {
            r = 0;
            return String(pattern, out var st) && int.TryParse(st, out r);
        }

        bool Date(string pattern, out DateTime r)
        {
            r = DateTime.MinValue;
            return String(pattern, out var st) && DateTime.TryParse(st, out r);
        }

        bool String(string pattern, out string st)
        {
            st = null;
            if (iline >= lines.Count)
            {
                return false;
            }

            var m = Regex.Match(lines[iline], pattern);
            if (m.Success)
            {
                iline++;
                st = m.Groups[1].Value;
            }

            return m.Success;
        }
    }
}

class Day
{
    public int guard;
    public int[] sleep;
    public int totalSleep => sleep.Sum();
}