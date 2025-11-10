using System.Text.Json;
using System.Text.Json.Nodes;

internal class SerializationDisallowDuplicateJsonProperties
{
    public static void Execute()
    {
        string json = """{ "Value": 1, "Value": -1 }""";
        Console.WriteLine(JsonSerializer.Deserialize<MyRecord>(json)!.Value); // -1

        JsonSerializerOptions options = new() { AllowDuplicateProperties = false };

        CatchExceptions(() => JsonSerializer.Deserialize<MyRecord>(json, options));// throws JsonException
        CatchExceptions(() => JsonSerializer.Deserialize<JsonObject>(json, options)); // throws JsonException
        CatchExceptions(() => JsonSerializer.Deserialize<Dictionary<string, int>>(json, options)); // throws JsonException

        JsonDocumentOptions docOptions = new() { AllowDuplicateProperties = false };
        CatchExceptions(() =>  JsonDocument.Parse(json, docOptions));   // throws JsonException
    }
    record MyRecord(int Value);

    private static void CatchExceptions(Action action)
    {
        try
        {
            action();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
