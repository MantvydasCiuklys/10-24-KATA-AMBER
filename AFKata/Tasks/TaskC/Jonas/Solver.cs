using System;

namespace Contestants.Jonas.TaskC;

public static class Solver
{
    private const string Sample = "ABEDSAGHASADBABABASDED";

    public static string Compress(string payload)
    {
        if (payload is null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        Dictionary<char, string> compressed = new Dictionary<char, string>();
        for (int i = 0; i < payload.Length; i++)
        {
            var c = payload[i];
            if (compressed.ContainsKey(c))
                compressed[c] = compressed[c] + 1;
            byte a = Convert.ToByte(c);
            Console.WriteLine(a);
        }

        // baseline implementation just returns the input; contestants should do better
        return payload;
    }

    public static string Decompress(string encoded)
    {
        if (encoded is null)
        {
            throw new ArgumentNullException(nameof(encoded));
        }

        // baseline implementation assumes identity encoding
        return encoded;
    }

    public static void RunSmokeTests()
    {
        var cases = new[] { string.Empty, "AAAAABBBBCCCCDDDD", "XYZXYZXYZXYZ", Sample };

        foreach (var payload in cases)
        {
            try
            {
                var compressed = Compress(payload);
                var roundtrip = Decompress(compressed);
                Console.WriteLine(
                    $"{Format(payload)} -> compressed {Format(compressed)} -> roundtrip {Format(roundtrip)}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Format(payload)} -> threw {ex.GetType().Name}: {ex.Message}");
            }
        }
    }

    private static string Format(string value) => value is null ? "null" : $"\"{value}\"";
}
