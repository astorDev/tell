using Nishe;

namespace Tell;

public record WorkingDirectoryCli
{
    public const string ArgumentName = "workDir";

    public static readonly ConditionalArgument<Folder> ConditionalArgument = CreateConditionalArgument(folder => folder.Exists);

    public static ConditionalArgument<Folder> CreateConditionalArgument(Func<Folder, bool> predicate) => new(
        ArgumentName,
        predicate,
        "."
    )
    {
        Description = "The working directory.",
        Arity = ArgumentArity.ZeroOrOne,
        CustomParser = (p) =>
        {
            var token = p.Tokens.Single();
            return new Folder(token.Value);
        }
    };
}

public static class ParseResultExtensions
{
    public static Folder GetRequiredWorkingDirectory(this ParseResult parseResult) =>
        parseResult.GetRequiredValue<Folder>(WorkingDirectoryCli.ArgumentName);
}