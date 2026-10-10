namespace Tell;

public record MakeVariableOption(string Key, string Value)
{
    public static Exception InvalidSyntaxException => new NotSupportedException("Fallback named arguments must use the form '--name value' or '--name=value'.");

    public static MakeVariableOption From(string[] chunk)
    {
        if (chunk.Length != 2) throw InvalidSyntaxException;

        (var key, var value) = (chunk[0], chunk[1]);

        if (!ValidKey(key)) throw InvalidSyntaxException;

        return new MakeVariableOption(key, value);
    }

    public static bool ValidKey(string token) 
        => token.StartsWith("--", StringComparison.Ordinal) && token.Length > 2;

    public string ToMakeVariableArgument()
        => $"{Key[2..].ToUpperInvariant()}={Value}";
}

public record MakeVariableOptionCollection(IReadOnlyList<MakeVariableOption> Items)
{
    public static MakeVariableOptionCollection From(IEnumerable<string> unmatchedTokens) => new(
        unmatchedTokens.SelectMany(ExpandEquals)
            .Chunk(2)
            .Select(MakeVariableOption.From)
            .ToArray()
    );

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