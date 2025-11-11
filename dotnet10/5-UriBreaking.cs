internal class UriBreaking
{
    public static void Execute()
    {
        var baseUri = "https://example.com/path?query=123#fragment";
        var fragment = new string('1', 65519 - baseUri.Length);
        CreateUri(baseUri + fragment);
        fragment += "1";
        CreateUri(baseUri + fragment);
        fragment = new string('1', 100_000_000 - baseUri.Length);
        CreateUri(baseUri + fragment);
    }

    private static void CreateUri(string url)
    {
        try
        {
            var uri = new Uri(url);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("         Created a URI with length: " + url.Length);
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to create a URI with length: " + url.Length);
        }
        Console.ResetColor();
    }
}
