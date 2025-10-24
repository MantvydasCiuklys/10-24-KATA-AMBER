using Tasks.TaskA;

namespace Contestants.Arnas.TaskA;

public static class Solver
{
    public static int? FirstUniqChar(string s)
    {
        if (s.Length == 0) return -1;
        var dict = new Dictionary<char, Option>();
        for (int i = 0; i < s.Length; i++)
        {
            var value = dict.GetValueOrDefault(s[i]);
            if (value is null)
            {
                dict.Add(s[i], new Option { Index = i, Count = 1 });
            }
            else
            {
                value.Count++;
            }
        }
        foreach (var val in dict.Where(val => val.Value.Count == 1))
        {
            return val.Value.Index;
        }

        return -1;
    }

    public class Option
    {
        public int Index { get; set; }
        public int Count { get; set; }
    }

    public static void RunSmokeTests() => TaskASmokeTestHarness.Run(FirstUniqChar);
}
