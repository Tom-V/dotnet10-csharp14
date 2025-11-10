
internal class LambdModifiers
{
    delegate bool TryParse<T>(string text, out T result);

    public static void Execute()
    {
        TryParse<int> parse1 = (string text, out int result) => int.TryParse(text, out result); // Explicit types required in .NET 9 and earlier
        TryParse<int> parse2 = (text, out result) => int.TryParse(text, out result); // Implicit types allowed in .NET 10

        Console.WriteLine(parse1("123", out var value1) ? $"Parsed: {value1}" : "Failed to parse.");
        Console.WriteLine(parse2("123", out var value2) ? $"Parsed: {value2}" : "Failed to parse.");
    }
}
