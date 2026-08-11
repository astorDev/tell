using Superpower.Model;

namespace Tell;

public static class TokenListExtensions
{
    public static void ForEach<T>(this TokenList<T> tokenList, Action<Token<T>> action)
    {
        foreach (var token in tokenList) action(token);
    }
}