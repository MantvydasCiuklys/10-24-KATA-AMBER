using Tasks.TaskB;

namespace Contestants.Arnas.TaskB;

public static class Solver
{
    public static long CountDivisibleSubarrays(int[] nums, int k)
    {
        var count = 0;
        var n = nums.Length;
        for (var start = 0; start < n; start++)
        {
            long sum = 0;
            for (var end = start; end < n; end++)
            {
                sum += nums[end];
                if (sum % k == 0)
                {
                    count++;
                }
            }
        }

        return count;
        // your code goes here
    }

    public static void RunSmokeTests() => TaskBSmokeTestHarness.Run(CountDivisibleSubarrays);
}
