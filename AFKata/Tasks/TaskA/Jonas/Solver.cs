using Tasks.TaskA;

namespace Contestants.Jonas.TaskA;

public static class Solver
{
    public static int? FirstUniqChar(string s)
    {
        // your code goes here

        Dictionary<char, int> count = new Dictionary<char, int>();
        foreach (char c in s)
        {
            if (count.ContainsKey(c))
            {
                count[c]++;
            }
            else
            {
                count[c] = 0;
            }
        }
        return count.Min(c => c.Value);
    }

    public static void RunSmokeTests() => TaskASmokeTestHarness.Run(FirstUniqChar);
}
