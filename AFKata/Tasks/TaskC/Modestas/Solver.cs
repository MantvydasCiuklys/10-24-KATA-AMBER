using System;

namespace Contestants.Modestas.TaskC;

public enum SymbolCode : byte
{
    A = 0b000001,
    B = 0b000010,
    C = 0b000011,
    D = 0b000100,
    E = 0b000101,
    F = 0b000110,
    G = 0b000111,
    H = 0b001000,
    I = 0b001001,
    J = 0b001010,
    K = 0b001011,
    L = 0b001100,
    M = 0b001101,
    N = 0b001110,
    O = 0b001111,
    P = 0b010000,
    Q = 0b010001,
    R = 0b010010,
    S = 0b010011,
    T = 0b010100,
    U = 0b010101,
    V = 0b010110,
    W = 0b010111,
    X = 0b011000,
    Y = 0b011001,
    Z = 0b011010,
    _1 = 0b011011,
    _2 = 0b011100,
    _3 = 0b011101,
    _4 = 0b011110,
    _5 = 0b011111,
    _6 = 0b100000,
    _7 = 0b100001,
    _8 = 0b100010,
    _9 = 0b100011,
}

public static class Solver
{
    private const string Sample = "2AAABEDS2AGHASAAD2BABABASDED";

    public static string Compress(string payload)
    {
        if (payload is null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        string compressed = "";

        int count = 1;
        for (int i = 0; i < payload.Length; i++)
        {
            if (i + 1 < payload.Length && payload[i] == payload[i + 1] && count < 9)
            {
                count++;
            }
            else
            {
                compressed += payload[i];
                compressed += count.ToString();
                count = 1;
            }
        }

        return compressed;
    }

    public static string Decompress(string encoded)
    {
        if (encoded is null)
        {
            throw new ArgumentNullException(nameof(encoded));
        }

        for (int i = 0; i < encoded.Length; i += 2)
        {
            char symbol = encoded[i];
            int count = int.Parse(encoded[i + 1].ToString());
            encoded = encoded.Remove(i, 2).Insert(i, new string(symbol, count));
            i += count - 2;
        }

        return encoded;
    }

    public static void RunSmokeTests()
    {
        var cases = new[]
        {
            string.Empty,
            "AAAAAAAAAAAAABBBBCCCCDDDD",
            "XYZXYZXYZXYZ",
            Sample,
        };

        foreach (var payload in cases)
        {
            try
            {
                var compressed = Compress(payload);
                var roundtrip = Decompress(compressed);
                Console.WriteLine($"{Format(payload)} -> compressed {Format(compressed)} -> roundtrip {Format(roundtrip)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Format(payload)} -> threw {ex.GetType().Name}: {ex.Message}");
            }
        }
    }

    private static string Format(string value) => value is null ? "null" : $"\"{value}\"";
}
