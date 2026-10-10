namespace Tell;

public static class ResultExtension
{
    public static bool AsClassicTry<T>(this Result<T> result, out T? value, out ParseException? error) => 
        result.AsClassicTry(x => x, out value, out error);

    public static bool AsClassicTry<TDirect, TFinal>(this Result<TDirect> result, Func<TDirect, TFinal> converter, out TFinal? value, out ParseException? error)
    {
        if (result.HasValue)
        {
            value = converter(result.Value);
            error = null;
            return true;
        }

        value = default;
        error = new ParseException(result.ToString(), result.ErrorPosition);

        return false;
    }
}