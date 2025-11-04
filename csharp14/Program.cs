//#define FIELD_KEYWORD
//#define GENERIC_NAMEOF
//#define LAMBDA_MODIFIERS
//#define BREAKING_SPAN_OVERLOADS

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

public class DataStorage
{
    private string field;

    public string Field
    {
        // Breaking change: Expression field in a property accessor refers to synthesized backing field
        get { return field; }
        //// Breaking change: Variable named field disallowed in a property accessor
        //get {
        //    var field = "none";
        //    return field; 
        //}
        set { field = value; }
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

#if BREAKING_SPAN_OVERLOADS

double[] x1 = new double[0];
//Span<ulong> y1 = MemoryMarshal.Cast<double, ulong>(x1); // previously worked, now compilation error
Span<ulong> z1 = MemoryMarshal.Cast<double, ulong>(x1.AsSpan());

var x2 = new long[] { 1 };
var y2 = new long[] { 2 };
Assert.AreEqual(y2, [2]);
Assert.AreEqual(x2, y2);


static class MemoryMarshal
{
    public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span) => default;
    public static Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span) => default;

}

static class Assert
{
    public static bool AreEqual<T>(T a, T b) => default;
    //public static bool AreEqual<T>(Span<T> a, Span<T> b) => default;
    //public static bool AreEqual<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b) => default;
}

#endif