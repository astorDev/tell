using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace Tell;

public class FallbackCli(TellFileSystem fileSystem, ILogger logger)
{
    public static readonly Argument<string> TargetArgument = new("target")
    {
        Arity = ArgumentArity.ZeroOrOne,
        Description = "The target to execute."
    };

    public static void ThrowIfUnmatchedTokens(ParseResult parseResult)
    {
        if (parseResult.UnmatchedTokens.Any())
        {
            throw new NotSupportedException("Fallback is only available when no arguments to a rule are provided. Invalid tokens: " + string.Join(", ", parseResult.UnmatchedTokens));
        }
    }

    public async Task<int> Action(ParseResult parseResult)
    {
        ThrowIfUnmatchedTokens(parseResult);

        var targetName = parseResult.GetValue(TargetArgument);

        var fallbackParams = new MakeFallback(fileSystem.WorkingDir.Path, fileSystem.File.Name, targetName);

        return await fallbackParams.Execute(logger);
    }
}