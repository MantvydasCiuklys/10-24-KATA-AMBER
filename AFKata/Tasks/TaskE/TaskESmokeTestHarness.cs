using System;

namespace Tasks.TaskE;

public static class TaskESmokeTestHarness
{
    private static readonly (int[] input, int[] expected)[] Cases =
    {
        (Array.Empty<int>(), Array.Empty<int>()),
        (new[] { 4 }, new[] { 4 }),
        (new[] { 3, 1, 4, 1, 5, 9, 2, 6 }, new[] { 1, 1, 2, 3, 4, 5, 6, 9 }),
        (new[] { 12, -4, 7, 7, -4, 0, 3, 12, 3 }, new[] { -4, -4, 0, 3, 3, 7, 7, 12, 12 }),
        (new[] { 5, 5, 5, 5, 5 }, new[] { 5, 5, 5, 5, 5 }),
        (new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 }),
        (new[] { 9, 7, 5, 3, 1 }, new[] { 1, 3, 5, 7, 9 }),
        (new[] { -3, -1, -2, -4 }, new[] { -4, -3, -2, -1 }),
        (new[] { 0, -1, 1, 0, -1, 1 }, new[] { -1, -1, 0, 0, 1, 1 }),
        (new[] { int.MaxValue, 0, int.MinValue, 42, -42 }, new[] { int.MinValue, -42, 0, 42, int.MaxValue }),
    };

    public static void Run(Func<int[], int[]> solver)
    {
        if (solver is null)
        {
            throw new ArgumentNullException(nameof(solver));
        }

        foreach (var (input, expected) in Cases)
        {
            var workingCopy = (int[])input.Clone();

            try
            {
                var result = solver(workingCopy);
                Console.WriteLine($"{Format(input)} -> {Format(result)} (expected {Format(expected)})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Format(input)} -> threw {ex.GetType().Name}: {ex.Message}");
            }
        }
    }

    private static string Format(int[] values)
    {
        if (values is null)
        {
            return "null";
        }

        return "[" + string.Join(", ", values) + "]";
    }
}
