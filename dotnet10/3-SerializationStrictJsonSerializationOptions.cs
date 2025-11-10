
using System.Text.Json;

internal class SerializationStrictJsonSerializationOptions
{
    public static void Execute()
    {
        var options = JsonSerializerOptions.Strict;

        Deserialize("""{ "Value": "1", "Value": "-1", "Value2": "2" }""");
        Deserialize("""{ "Value": "1", "Value2": "2", "Value3": "3" }""");
        Deserialize("""{ "Value": null, "Value2": "2" }""");
        Deserialize("""{ "Value2": "2" }""");
        Deserialize("""{ "Value": "1" }""");
    }

    public class MyJsonObject
    {
        public required string Value { get; set; }
        public string? Value2 { get; set; }
    }


    private static void Deserialize(string json)
    {
        try
        {
            Console.WriteLine("Deserializing " + json);
            var result = JsonSerializer.Deserialize<MyJsonObject>(json, JsonSerializerOptions.Strict);
            Console.WriteLine("Success");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed" + ex.GetType() + ": " + ex.Message);
        }
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
    }

}
