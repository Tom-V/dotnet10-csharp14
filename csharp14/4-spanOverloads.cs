internal class SpanOverloads
{
    public static void Execute()
    {
        double[] x1 = [];
        //Span<ulong> y1 = MemoryMarshal.Cast<double, ulong>(x1); // previously worked, now compilation error
        Span<ulong> z1 = MemoryMarshal.Cast<double, ulong>(x1.AsSpan());

        var x2 = new long[] { 1 };
        var y2 = new long[] { 2 };
        Assert.AreEqual(y2.AsSpan(), [2]);
        Assert.AreEqual(x2, y2);
    }

    static class MemoryMarshal
    {
        public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span) => default;
        public static Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span) => default;

    }

    static class Assert
    {
        public static bool AreEqual<T>(T a, T b) => default;
        public static bool AreEqual<T>(Span<T> a, Span<T> b) => default;
        //public static bool AreEqual<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b) => default;
    }

}
