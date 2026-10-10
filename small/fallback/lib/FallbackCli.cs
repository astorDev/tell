using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace Tell;

public static class FallbackCli
{
    public static readonly Argument<string> TargetArgument = new("target")
    {
        Arity = ArgumentArity.ZeroOrOne,
        Description = "The target to execute."
    };

    public static MakeFallback CreateFallbackFrom(ParseResult parseResult, TellFileSystem fileSystem, ILogger logger)
    {
        var targetName = parseResult.GetValue(TargetArgument);

        logger.LogDebug("Creating MakeVariableOptionCollection from unmatched tokens: {UnmatchedTokens}", string.Join(";", parseResult.UnmatchedTokens));
        var variableOptions = MakeVariableOptionCollection.From(parseResult.UnmatchedTokens);
        var variableArgs = variableOptions.ToMakeArguments().ToArray();

        var filePath = fileSystem.File.PathRelativeTo(fileSystem.WorkingDir);

        return new (
            fileSystem.WorkingDir.Path,
            filePath,
            targetName, 
            variableArgs
        );
    }

    public static async Task<int> Action(ParseResult parseResult, TellFileSystem fileSystem, ILogger logger)
    {
        var fallback = CreateFallbackFrom(parseResult, fileSystem, logger);

        return await fallback.Execute(logger);
    }
}