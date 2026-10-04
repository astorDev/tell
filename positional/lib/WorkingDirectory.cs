using Nishe;

namespace Tell;

public record WorkingDirectory
{
    public static readonly Argument<Folder> Argument = CreateArgument();

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

    // Reads the token directly: a non-existing folder is a validation error (so it is not treated as a root token
    // by ParseWithDefaultCommand), and reading the converted value would throw on it.
    public static Folder From(ParseResult parsed)
    {
        var tokens = parsed.GetResult(Argument)?.Tokens;
        if (tokens is not { Count: > 0 }) return new(".");

        var folder = new Folder(tokens[0].Value);
        return folder.Exists ? folder : new(".");
    }

    static Argument<Folder> CreateArgument()
    {
        var argument = new Argument<Folder>("workDir")
        {
            Description = "The working directory.",
            Arity = ArgumentArity.ZeroOrOne,
            DefaultValueFactory = (result) => new("."),
            CustomParser = result =>
            {
                if (result.Tokens.Count == 0) return new(".");

                var token = result.Tokens[0].Value;
                var folder = new Folder(token);
                return folder.Exists ? folder : new(".");
            }
        };

        argument.Validators.Add(result =>
        {
            if (result.Tokens.Count == 0) return;
            if (!new Folder(result.Tokens[0].Value).Exists) result.AddError($"Working directory '{result.Tokens[0].Value}' does not exist.");
        });

        return argument;
    }
}