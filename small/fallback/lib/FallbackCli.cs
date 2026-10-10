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

    public MakeFallback CreateFallbackFrom(ParseResult parseResult)
    {
        var targetName = parseResult.GetValue(TargetArgument);

        var variableOptions = MakeVariableOptionCollection.From(parseResult.UnmatchedTokens);
        var variableArgs = variableOptions.ToMakeArguments().ToArray();

        return new (
            fileSystem.WorkingDir.Path, 
            fileSystem.File.Path, 
            targetName, 
            variableArgs
        );
    }

    public async Task<int> Action(ParseResult parseResult)
    {
        var fallback = CreateFallbackFrom(parseResult);

        return await fallback.Execute(logger);
    }
}