using System;

namespace Contestants.Modestas.TaskD;

public static class Solver
{
    private const string SampleMessage = "Meet me at the old bridge at midnight.";

    public static long cipher = DateTime.Today.DayOfYear;
    public static string Cipher(string plaintext)
    {
        if (plaintext is null)
        {
            throw new ArgumentNullException(nameof(plaintext));
        }

        string cypheredText = "";
        for (int i = 0; i < plaintext.Length; i++)
        {
            cypheredText += Convert.ToChar((short)Convert.ToInt64(plaintext[i]) + ((cipher)));
        }

        return cypheredText;
    }

    public static string Decipher(string secretText)
    {
        if (secretText is null)
        {
            throw new ArgumentNullException(nameof(secretText));
        }

        string plainText = "";
        for (int i = 0; i < secretText.Length; i++)
        {
            plainText += Convert.ToChar((short)Convert.ToInt64(secretText[i]) - (cipher));
        }

        return plainText;
    }

    public static void RunSmokeTests()
    {
        var encoded = Cipher(SampleMessage);
        var plain = Decipher(encoded);
        Console.WriteLine($"Sample input: {SampleMessage}");
        Console.WriteLine($"Your cipher output: {encoded}");
        Console.WriteLine($"Your deciphered output: {plain}");
    }
}
