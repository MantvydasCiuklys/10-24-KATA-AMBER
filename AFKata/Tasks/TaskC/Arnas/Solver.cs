using System;
using System.Text;

namespace Contestants.Arnas.TaskC;

public static class Solver
{
    private const string Sample = "ABEDSAGHASADBABABASDED";

    public static string Compress(string payload)
    {
        if (payload is null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        using var stream = new MemoryStream();
        Tuple<(byte, short)?, (byte, short)?> current = new(null, null);
        for (int i = 0; i < payload.Length; i++)
        {
            if (current.Item1 is not null && current.Item2 is not null)
            {
                current = new(null, null);
            }
            var c = payload[i];
            var index = GetIndex(c);
            short count = 1;
            while (i + count < payload.Length && payload[i + count] == c)
            {
                count++;
            }
            if (current.Item1 is null)
            {
                current = new(new(index, count), null);
            }
            else
            {
                current = new(current.Item1, new(index, count));
            }
            
            if (current.Item1 is not null && current.Item2 is not null)
            {
                var firstBytePrefix = Convert.ToByte(current.Item1.Value.Item1 << 2);
                var firstByteAppendix = Convert.ToByte(current.Item1.Value.Item2 >> 12);
                var firstByte = (byte)(firstBytePrefix | firstByteAppendix);
                var secondByte = Convert.ToByte(current.Item1.Value.Item2 << 4 >> 8);
                var thirdBytePrefix = Convert.ToByte(current.Item1.Value.Item2 & 0x07);
                var thirdByteAppendix = Convert.ToByte(current.Item2.Value.Item1 >> 2 << 4);
                var thirdByte = (byte)(thirdBytePrefix | thirdByteAppendix);
                var fourthBytePrefix= Convert.ToByte(current.Item2.Value.Item1 & 0x03);
                var fourthByteAppendix = Convert.ToByte(current.Item2.Value.Item2 >> 8);
                var fourthByte = (byte)(fourthBytePrefix | fourthByteAppendix);
                var fifthByte = Convert.ToByte(current.Item2.Value.Item2 & 0xFF);
                stream.WriteByte(firstByte);
                stream.WriteByte(secondByte);
                stream.WriteByte(thirdByte);
                stream.WriteByte(fourthByte);
                stream.WriteByte(fifthByte);
            }
        }
        using var fileStream = File.OpenWrite("test.txt");
        stream.CopyToAsync(fileStream);
        fileStream.Flush();

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
    
    private static byte GetIndex(char c) {
        return c switch
        {
            >= 'A' and <= 'Z' => (byte)(c - 'A'),
            >= 'a' and <= 'z' => (byte)(c - 'a' + 26),
            >= '0' and <= '9' => (byte)(c - '0' + 52),
            _ => byte.MaxValue
        };
    }

    public static void RunSmokeTests()
    {
        var cases = new[]
        {
            string.Empty,
            "AAAAABBBBCCCCDDDD",
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
