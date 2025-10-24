using System;
using System.Linq;
using Tasks.TaskE;

namespace Contestants.Arnas.TaskE;

public static class Solver
{
    public static int[] CustomSort(int[] values)
    {
        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        if (values.Length is 0 or 1)
        {
            return values;
        }

        for (var i = 0; i < values.Length; i++)
        {
            for (var j = i + 1; j < values.Length; j++)
            {
                if (values[i] > values[j])
                {
                    (values[i], values[j]) = (values[j], values[i]);
                }
            }
        }

        // TODO: replace with your custom sorting algorithm (no Array.Sort!)
        return values.ToArray(); // return copy to avoid mutating caller
    }

    public static void RunSmokeTests() => TaskESmokeTestHarness.Run(CustomSort);

    public static void ShowCase() => RunSmokeTests();
}
