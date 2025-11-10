internal class UserDefinedCompoundAssignment
{
    public static void Execute()
    {
        var money = new Money(100, "USD");
        money += new Money(50, "USD");
        Console.WriteLine($"Money: {money}");
        money += 25;
        Console.WriteLine($"Money: {money}");
        money++;
        Console.WriteLine($"Money: {money}");
    }
}

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

