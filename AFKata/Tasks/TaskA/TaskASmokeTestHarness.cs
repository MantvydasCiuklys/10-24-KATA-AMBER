using System;

namespace Tasks.TaskA;

public static class TaskASmokeTestHarness
{
    private static readonly (string input, int expected)[] Cases =
    {
        ("leetcode", 0),
        ("loveleetcode", 2),
        ("aabb", -1),
        ("aabbccd", 6),
        ("z", 0),
        ("AaBb", 0),
        ("abcddcbaef", 8),
        ("abcabcdd", -1),
    };

    public static void Run(Func<string, int?> solver)
    {
        foreach (var (input, expected) in Cases)
        {
            try
            {
                var result = solver(input);
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
