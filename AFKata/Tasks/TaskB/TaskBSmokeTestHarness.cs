using System;

namespace Tasks.TaskB;

public static class TaskBSmokeTestHarness
{
    private static readonly (int[] nums, int k, long expected)[] Cases =
    {
        (new[] { 4, 5, 0, -2, -3, 1 }, 5, 7),
        (new[] { 5 }, 9, 0),
        (new[] { 5, 10, 15 }, 5, 6),
        (new[] { 1, 2, 3, 4, 5 }, 3, 7),
        (new[] { 0, 0, 0 }, 5, 6),
        (new[] { 2, 4, 6 }, 1, 6),
        (new[] { 6 }, 3, 1),
        (new[] { 7, -2, 3 }, 4, 1),
        (new[] { 1, 2, 3 }, 5, 1),
        (new[] { 5, -5, 5, 5 }, 5, 10),
    };

    public static void Run(Func<int[], int, long> solver)
    {
        foreach (var (nums, k, expected) in Cases)
        {
            try
            {
                var result = solver(nums, k);
                Console.WriteLine($"{Format(nums, k)} -> {result} (expected {expected})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Format(nums, k)} -> threw {ex.GetType().Name}: {ex.Message}");
            }
        }
    }

    private static string Format(int[] nums, int k)
    {
        var values = nums is null ? "null" : "[" + string.Join(", ", nums) + "]";
        return $"nums = {values}, k = {k}";
    }
}
