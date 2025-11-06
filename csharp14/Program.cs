Console.WriteLine("hello world");

//#define FIELD_KEYWORD
//#define GENERIC_NAMEOF
//#define LAMBDA_MODIFIERS

#if FIELD_KEYWORD
var student = new Student
{
    Name = "Alice",
    Age = 20
};

Console.WriteLine(student);

public record Student
{
    public required string Name
    {
        get;
        set => field = value ?? throw new ArgumentNullException(nameof(value));
    }

    public required int Age
    {
        get;
        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), "Age cannot be negative.");
    }
}
#endif


#if GENERIC_NAMEOF

Console.WriteLine(nameof(List<>));

#endif


#if LAMBDA_MODIFIERS

TryParse<int> parse1 = (text, out result) => int.TryParse(text, out result); // Implicit types allowed in .NET 10
TryParse<int> parse2 = (string text, out int result) => int.TryParse(text, out result); // Explicit types required in .NET 9 and earlier

Console.WriteLine(parse1("123", out var value1) ? $"Parsed: {value1}" : "Failed to parse.");

delegate bool TryParse<T>(string text, out T result);

#endif
