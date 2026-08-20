using System.CommandLine;

namespace Tell;

public record TellCommandParams(
    string? FirstArgument,
    string? SecondArgument,
    string? ThirdArgument,
    string? FileOption,
    IReadOnlyList<string> UnmatchedTokens
)
{
    public static readonly Argument<string> firstArgument = new("first")
    {
        Description = "The first positional argument. Can be: working directory, target or first argument of the rule.",
        Arity = ArgumentArity.ZeroOrOne
    };

    public static readonly Argument<string> secondArgument = new("second")
    {
        Description = "The second positional argument. Can be: target or first argument of the rule.",
        Arity = ArgumentArity.ZeroOrOne
    };

    public static readonly Argument<string> thirdArgument = new("third")
    {
        Description = "The third positional argument. If present, represents first argument of the rule.",
        Arity = ArgumentArity.ZeroOrOne
    };

    public static readonly Option<string> fileOption = new("--file")
    {
        Description = "The path to the Makefile to use.",
        Required = false
    };

    public static TellCommandParams From(ParseResult parseResult)
    {
        var dirty = DirtyFrom(parseResult);
        return dirty.Cleaned();
    }

    public static TellCommandParams DirtyFrom(ParseResult parseResult)
    {
        var first = parseResult.GetValue(firstArgument);
        var second = parseResult.GetValue(secondArgument);
        var third = parseResult.GetValue(thirdArgument);
        var file = parseResult.GetValue(fileOption);
        var unmatchedTokens = parseResult.UnmatchedTokens.ToList();

        return new TellCommandParams(first, second, third, file, unmatchedTokens);
    }

    /// <summary>
    /// For some reason System.CommandLine treats options as positional arguments if it can't parse them in other way.
    /// So we need to filter them out from positional arguments and put them into unmatched tokens.
    /// </summary>
    public TellCommandParams Cleaned()
    {
        var (first, second, third, _, initialUnmatchedTokens) = this;
        var unmatchedTokens = initialUnmatchedTokens.ToList();

        if (first is not null && first.StartsWith('-'))
        {
            unmatchedTokens.Add(first);
            first = null;
        }

        if (second is not null && second.StartsWith('-'))
        {
            unmatchedTokens.Add(second);
            second = null;
        }

        if (third is not null && third.StartsWith('-'))
        {
            unmatchedTokens.Add(third);
            third = null;
        }

        var all = new string?[] { first, second, third }.Where(x => x is not null).ToList();

        var result = this with
        {
            FirstArgument = all.ElementAtOrDefault(0),
            SecondArgument = all.ElementAtOrDefault(1),
            ThirdArgument = all.ElementAtOrDefault(2),
            UnmatchedTokens = unmatchedTokens
        };

        return result;
    }
}