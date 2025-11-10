internal class GenericNameof
{
    public static void Execute()
    {
        Console.WriteLine(nameof(List<int>)); 
        // New: 
        Console.WriteLine(nameof(List<>));
    }
}
