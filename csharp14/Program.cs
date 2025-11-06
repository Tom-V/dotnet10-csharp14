Console.WriteLine("hello world");

//#define FIELD_KEYWORD
//#define GENERIC_NAMEOF
//#define LAMBDA_MODIFIERS
//#define BREAKING_SPAN_OVERLOADS
//#define EXTENSION_BLOCKS
//#define NULL_CONDITIONAL_ASSIGNMENT
//#define USER_DEFINED_COMPOUND_ASSIGNMENT

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


#if EXTENSION_BLOCKS

using System.Security.Claims;

var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role,"Admin")]));
Console.WriteLine($"Is Admin: {user.IsAdmin}");
Console.WriteLine("Is in Role 'User': " + user.IsInRole("User"));


public static class UserExtenions
{
    // Extension block
    extension(ClaimsPrincipal user) // extension members for IEnumerable<TSource>
    {
        // Extension property:
        public bool IsAdmin => user.HasClaim(ClaimTypes.Role, "Admin");

        // Extension method:
        public bool IsInRole(string role) => user.HasClaim(ClaimTypes.Role, role);
    }

    // Extension block with generic type parameter
    extension<TUser> (TUser user) where TUser : ClaimsPrincipal
    {
        public bool IsInRole(string role) => user.HasClaim(ClaimTypes.Role, role);
    }
}

#endif

#if NULL_CONDITIONAL_ASSIGNMENT

int? length = null;
length ??= 0;
Console.WriteLine($"Length: {length}");

#endif

#if USER_DEFINED_COMPOUND_ASSIGNMENT

var money = new Money(100, "USD");
money += new Money(50, "USD");
Console.WriteLine($"Money: {money}");
money += 25; 
Console.WriteLine($"Money: {money}");
money++;
Console.WriteLine($"Money: {money}");

public class Money(decimal amount, string currency)
{
    public decimal Amount { get; private set; } = amount;
    public string Currency { get; } = currency;

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add different currencies");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    // User-defined compound assignment operator
    public void operator +=(int amount)
    {
        Amount += amount;
    }

    // User-defined increment operator
    public void operator ++()
    {
        Amount += 1;
    }

    public override string ToString() => $"{Amount} {Currency}";
}

#endif
