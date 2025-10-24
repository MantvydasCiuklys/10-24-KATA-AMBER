using System.Text;

namespace Contestants.Arnas.TaskD;

public static class Solver
{
    private const string SampleMessage = "Meet me at the old bridge at midnight.";

    private readonly static long Key = new Random().NextInt64();

    public static string Cipher(string plaintext)
    {
        if (plaintext is null)
        {
            throw new ArgumentNullException(nameof(plaintext));
        }

        List<byte> bytes = [];
        foreach (var c in plaintext)
        {
            var currByte = (byte)c;
            currByte = (byte)(currByte ^ (byte)(Key >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 8 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 16 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 24 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 32 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 40 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 48 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 56 >> 56));
            
            bytes.Add(currByte);
        }
        
        var result = Convert.ToBase64String(bytes.ToArray());
        return result;
    }
    
    public static string Decipher(string plaintext)
    {
        if (plaintext is null)
        {
            throw new ArgumentNullException(nameof(plaintext));
        }

        var bytes = Convert.FromBase64String(plaintext);
        var builder = new StringBuilder();
        foreach (var c in bytes)
        {
            var currByte = c;
            currByte = (byte)(currByte ^ (byte)(Key << 56 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 48 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 40 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 32 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 24 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 16 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key << 8 >> 56));
            currByte = (byte)(currByte ^ (byte)(Key >> 56));

            builder.Append((char)currByte);
        }

        var result = builder.ToString();
        return result;
    }
    
    public static void RunSmokeTests()
    {
        var encoded = Cipher(SampleMessage);
        Console.WriteLine($"Sample input: {SampleMessage}");
        Console.WriteLine($"Your cipher output: {encoded}");
        var decoded = Decipher(encoded);
        Console.WriteLine($"Your decipher output: {decoded}");
    }
}
