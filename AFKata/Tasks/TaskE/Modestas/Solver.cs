using System;
using System.Linq;
using Tasks.TaskE;

namespace Contestants.Modestas.TaskE;

public static class Solver
{
    public static int[] CustomSort(int[] values)
    {
        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        for (int i = 0; i < values.Length - 1; i++)
        {
            for (int j = 0; j < values.Length - i - 1; j++)
            {
                if (values[j] > values[j + 1])
                {
                    int placeholder = values[j];
                    values[j] = values[j + 1];
                    values[j + 1] = placeholder;
                }
            }
        }

        return values.ToArray();
    }

    public static void RunSmokeTests() => TaskESmokeTestHarness.Run(CustomSort);

    public static void ShowCase() => RunSmokeTests();
}
