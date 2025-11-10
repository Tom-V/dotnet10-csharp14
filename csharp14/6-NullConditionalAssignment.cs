internal class NullConditionalAssignment
{
    public static void Execute()
    {
        int? length = null;
        length ??= 0;
        Console.WriteLine($"Length: {length}");
    }
}
