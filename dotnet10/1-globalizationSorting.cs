using System.Globalization;

internal class GlobalizationSorting
{
    public static void Execute()
    {
        string[] transports = [
            "Transport 10",
            "Transport 9",
            "Transport 2",
            "Transport 12",
            "Transport 20",
            "Transport 105"
        ];
        Console.WriteLine("Sorted:");
        foreach (var transport in transports.Order(StringComparer.OrdinalIgnoreCase))
        {
            Console.WriteLine(transport);
        }

        Console.WriteLine("Natural sorted:");
        StringComparer numericStringComparer = StringComparer.Create(CultureInfo.InvariantCulture, CompareOptions.NumericOrdering);
        foreach (var transport in transports.Order(numericStringComparer))
        {
            Console.WriteLine(transport);
        }
    }
}
