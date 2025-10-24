using Tasks.TaskA;

namespace Contestants.Arnas.TaskA;

public static class Solver
{
    public static int? FirstUniqChar(string s)
    {
        if (s.Length == 0) return -1;
        var dict = new Dictionary<char, Option>();
        foreach (var c in s)
        {
            var value = dict.GetValueOrDefault(c);
            if (value is null)
            {
                dict.Add(c, new Option { Index = dict.Count, Count = 1 });
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
