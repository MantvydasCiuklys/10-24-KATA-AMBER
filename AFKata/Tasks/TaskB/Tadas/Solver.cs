using System;
using System.Linq;

namespace Contestants.Tadas.TaskB;

public static class Solver
{
    public static long CountDivisibleSubarrays(int[] nums, int k)
    {
        // your code goes here
        return -1;
    }

    public static void RunSmokeTests()
    {
        var cases = new (int[] nums, int k, long expected)[]
        {
            (new[] { 4, 5, 0, -2, -3, 1 }, 5, 7),
            (new[] { 5 }, 9, 0),
            (new[] { 5, 10, 15 }, 5, 6),
            (new[] { 1, 2, 3, 4, 5 }, 3, 7),
        };

        foreach (var (nums, k, expected) in cases)
        {
            try
            {
                var result = CountDivisibleSubarrays(nums, k);
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
