using Tasks.TaskA;

namespace Contestants.Modestas.TaskA;

public static class Solver
{
    public static int? FirstUniqChar(string s)
    {
        List<char> chars = s.ToList();
        for (int i = 0; i < chars.Count; i++)
        {
            var currentChar = chars[i];
            for (int j = 0; j < chars.Count; j++)
            {
                var charToCompare = chars[j];
                if (charToCompare == currentChar)
                {
                }

            }   
        }
        return null;
    }

    public static void RunSmokeTests() => TaskASmokeTestHarness.Run(FirstUniqChar);
}
