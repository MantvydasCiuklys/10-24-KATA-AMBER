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

        // TODO: replace with your custom sorting algorithm (no Array.Sort!)
        return values.ToArray(); // return copy to avoid mutating caller
    }

    public static void RunSmokeTests() => TaskESmokeTestHarness.Run(CustomSort);

    public static void ShowCase() => RunSmokeTests();
}
