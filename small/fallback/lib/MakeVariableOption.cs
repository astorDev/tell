namespace Tell;

public record MakeVariableOption(string Key, string Value)
{
    public static Exception InvalidSyntaxException => new NotSupportedException("Fallback named arguments must use the form '--name value' or '--name=value'.");

    public static MakeVariableOption From(string key, string value)
    {
        if (!ValidKey(key)) throw new ($"Variable key '{key}' is invalid.");

        return new MakeVariableOption(key, value);
    }

    public static bool ValidKey(string token) 
        => token.StartsWith("--", StringComparison.Ordinal) && token.Length > 2;

    public string ToMakeVariableArgument()
        => $"{Key[2..].ToUpperInvariant()}={Value}";
}

public record MakeVariableOptionCollection(IReadOnlyList<MakeVariableOption> Items)
{
    public static MakeVariableOptionCollection From(IEnumerable<string> unmatchedTokens)
    {
        var chunks = unmatchedTokens.SelectMany(ExpandEquals).Chunk(2);
        if (chunks.Any(chunk => chunk.Length != 2)) 
            throw new NotSupportedException($"Unable to parse unmatched tokens ({string.Join(" ", unmatchedTokens)}) into pairs.");

        var result = chunks.Select(chunk => MakeVariableOption.From(chunk[0], chunk[1]));

        return new(result.ToArray());
    }

    public static string[] ExpandEquals(string token)
    {
        var separatorIndex = token.StartsWith("--", StringComparison.Ordinal) ? token.IndexOf('=') : -1;
        return separatorIndex < 0
            ? [ token ]
            : [ token[..separatorIndex], token[(separatorIndex + 1)..] ];
    }

    public IReadOnlyList<string> ToMakeArguments()
        => Items.Select(item => item.ToMakeVariableArgument()).ToArray();
}