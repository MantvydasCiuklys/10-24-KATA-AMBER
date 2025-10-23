using System;

namespace Contestants.Tadas;

public static class Solver
{
    public static int? FirstUniqChar(string s)
    {
        // your code goes here
        return null;
    }

    public static void RunSmokeTests()
    {
        var cases = new (string input, int expected)[]
        {
            ("leetcode", 0),
            ("aabb", -1),
        };

        foreach (var (input, expected) in cases)
        {
            try
            {
                int? result = FirstUniqChar(input);
                Console.WriteLine($"{Format(input)} -> {result} (expected {expected})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Format(input)} -> threw {ex.GetType().Name}: {ex.Message}");
            }
        }
    }

    private static string Format(string value) => value is null ? "null" : $"\"{value}\"";
}
