internal class Fieldkeyword
{

    public static void Execute()
    {
        var student = new Student
        {
            Name = "Alice",
            Age = 20
        };

        Console.WriteLine(student);
    }
}


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

#region "Breaking Changes in C# 14.0"
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

#endregion