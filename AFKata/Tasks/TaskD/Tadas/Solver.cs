using System;

namespace Contestants.Tadas.TaskD;

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
        return plaintext;
    }

    public static void ShowCase()
    {
        var encoded = Cipher(SampleMessage);
        Console.WriteLine($"Sample input: {SampleMessage}");
        Console.WriteLine($"Your cipher output: {encoded}");
    }
}
