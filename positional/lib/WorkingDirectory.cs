using Nishe;

namespace Tell;

public record WorkingDirectory
{
    public static readonly ConditionalArgument<Folder> ConditionalArgument = new(
        "workDir",
        folder => folder.Exists,
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