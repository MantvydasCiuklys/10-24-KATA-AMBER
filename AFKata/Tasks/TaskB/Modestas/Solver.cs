using Tasks.TaskB;

namespace Contestants.Modestas.TaskB;

public static class Solver
{
    public static long CountDivisibleSubarrays(int[] nums, int k)
    {
        var count = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            int sumFirst = 0;
            for (int j = i; j < nums.Length; j++)
            {
                sumFirst += nums[j];
                int sum = nums[i] + nums[j];
                if (sum % k == 0 || sumFirst % k == 0)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public static void RunSmokeTests() => TaskBSmokeTestHarness.Run(CountDivisibleSubarrays);
}
