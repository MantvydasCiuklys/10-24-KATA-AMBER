using System;
using System.Linq;

namespace Contestants.Modestas.TaskE;

public static class Solver
{
    public static int[] CustomSort(int[] values)
    {
        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        // TODO: replace with your custom sorting algorithm (no Array.Sort!)
        return values.ToArray(); // return copy to avoid mutating caller
    }

    public static void ShowCase()
    {
        var samples = new[]
        {
            Array.Empty<int>(),
            new[] { 4 },
            new[] { 3, 1, 4, 1, 5, 9, 2, 6 },
            new[] { 12, -4, 7, 7, -4, 0, 3, 12, 3 },
            new[] { 5, 5, 5, 5, 5 },
        };

        foreach (var sample in samples)
        {
            var sorted = CustomSort(sample);
            Console.WriteLine($"[{string.Join(", ", sample)}] -> [{string.Join(", ", sorted)}]");
        }
    }
}
