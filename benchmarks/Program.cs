// dotnet run -c Release -f net9.0 --filter "*" --runtimes net9.0 net10.0

using benchmarkdotnet.attributes;
using benchmarkdotnet.running;

//BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

//[DisassemblyDiagnoser]
//[MemoryDiagnoser(displayGenColumns: false)]
//[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD", "y")]
//public partial class Tests
//{
//    [Benchmark]
//    [Arguments(42)]
//    public int Sum(int y)
//    {
//        Func<int, int> addY = x => x + y;
//        return DoubleResult(addY, y);
//    }

//    private int DoubleResult(Func<int, int> func, int arg)
//    {
//        int result = func(arg);
//        return result + result;
//    }
//}



// dotnet run -c Release -f net9.0 --filter "*" --runtimes net9.0 net10.0

//using BenchmarkDotNet.Attributes;
//using BenchmarkDotNet.Running;
//using System.Runtime.CompilerServices;

//BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

[MemoryDiagnoser(displayGenColumns: false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public partial class Tests
{
    [Benchmark]
    public void Test()
    {
        Process(new string[] { "a", "b", "c" });

        static void Process(string[] inputs)
        {
            foreach (string input in inputs)
            {
                Use(input);
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            static void Use(string input)
            { }
        }
    }
}





// dotnet run -c Release -f net9.0 --filter "*" --runtimes net9.0 net10.0

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Runtime.CompilerServices;

BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

[MemoryDiagnoser(displayGenColumns: false)]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public partial class Tests
{
    private byte[] _buffer = new byte[3];

    [Benchmark]
    public void Test() => Copy3Bytes(0x12345678, _buffer);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Copy3Bytes(int value, Span<byte> dest) =>
        BitConverter.GetBytes(value).AsSpan(0, 3).CopyTo(dest);
}



//before dotnet 10
//using System;
//using System.Collections;

//class Program
//{
//    static void Main()
//    {
//        int[] numbers = { 1, 2, 3, 4, 5 };
//        IEnumerable enumerable = numbers; // Casting to IEnumerable
//        foreach (int num in enumerable) // Virtual call to GetEnumerator()
//        {
//            Console.Write(num + " ");
//        }
//    }
//}
//dotnet 10 and later
//using System;
//using System.Collections;

//class Program
//{
//    static void Main()
//    {
//        int[] numbers = { 1, 2, 3, 4, 5 };
//        foreach (int num in numbers) // JIT now eliminates the virtual call!
//        {
//            Console.Write(num + " ");
//        }
//    }
//}


//before dotnet 10
//using System;

//class Program
//{
//    static void Main()
//    {
//        int sum = 0;
//        int[] numbers = { 1, 2, 3, 4, 5 };
//        for (int i = 0; i < numbers.Length; i++) // Condition checked every iteration
//        {
//            sum += numbers[i];
//        }
//        Console.WriteLine(sum);
//    }
//}


//loop hoisting
//using System;

//class Program
//{
//    static void Main()
//    {
//        int sum = 0;
//        int[] numbers = { 1, 2, 3, 4, 5 };
//        int length = numbers.Length; // Loop hoisting: moving condition outside
//        for (int i = 0; i < length; i++) // Now length is cached
//        {
//            sum += numbers[i];
//        }
//        Console.WriteLine(sum);
//    }
//}