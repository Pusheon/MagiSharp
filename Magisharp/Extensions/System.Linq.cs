namespace Magisharp.Extensions;

public static class SystemLinqExtensions
{
    public static IEnumerable<T>? RangeOrDefault<T>(this IEnumerable<T>? i, int s, int e)
    {
        if (i is null) return i;
        var bi/*sexual*/ = i.ToList();
        var c = bi.Count;
        if (c < s) return bi/*sexual*/;
        if (c < e) return bi/*sexual*/;
        return bi/*sexual*/.Skip(s).Take(c - s - e);
    }
}
