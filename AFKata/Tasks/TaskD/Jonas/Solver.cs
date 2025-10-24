using System;
using System.Text;

namespace Contestants.Jonas.TaskD;

public static class Solver
{
    private const string SampleMessage = "Meet me at the old bridge at midnight.";

    public static string Cipher(string plaintext)
    {
        if (plaintext is null)
        {
            throw new ArgumentNullException(nameof(plaintext));
        }

        // TODO: replace with your custom cipher implementation
        char[] chars = plaintext.ToCharArray();

        long l = 0;
        Random rnd = new Random();
        int key = rnd.Next(0, 9);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < chars.Length; i++)
        {
            var newC = Convert.ToChar(Convert.ToByte(chars[i]) + 5);
            sb.Append(newC);
            Console.WriteLine(newC);
        }
        sb.Append(key);
        return sb.ToString();
    }

    public static string Decipher(string plaintext)
    {
        if (plaintext is null)
        {
            throw new ArgumentNullException(nameof(plaintext));
        }

        // TODO: replace with your custom cipher implementation
        char[] chars = plaintext.ToCharArray();
        char keyC = chars[chars.Length - 1];

        int key = int.Parse(keyC.ToString());
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < chars.Length - 1; i++)
        {
            sb.Append(Convert.ToChar(Convert.ToByte(chars[i]) - 5));
        }
        return sb.ToString();
    }

    public static void RunSmokeTests()
    {
        var encoded = Cipher(SampleMessage);
        Console.WriteLine($"Sample input: {SampleMessage}");
        Console.WriteLine($"Your cipher output: {encoded}");
        Console.WriteLine($"Your decipher output: {Decipher(encoded)}");
    }
}
