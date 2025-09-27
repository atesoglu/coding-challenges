namespace AdventOfCode.Tests;

[AttributeUsage(AttributeTargets.Class)]
public class ChallengeNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}