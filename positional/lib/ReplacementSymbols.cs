namespace Tell;

public record ReplacementSymbols(Option<Replacement> Option, Argument<Replacement> Argument)
{
    public static Option<Replacement> CreateOption(ReplacementSettings candidate) => new($"--{candidate.KebabedIdentifier}")
    {
        Description = $"Replacement for '{candidate.OriginalIdentifier}'.",
        CustomParser = (result) => 
        {
            var value = result.Tokens[0].Value;
            return new Replacement(candidate.OriginalIdentifier, [ RecipeFragment.FromLiteral(value) ]);
        },
        DefaultValueFactory = (result) => new Replacement(candidate.OriginalIdentifier, candidate.DefaultValue, IsDefault: true)
    };

    public static Argument<Replacement> CreateArgument(ReplacementSettings candidate) => new($"{candidate.KebabedIdentifier}")
    {
        Description = $"Replacement for '{candidate.OriginalIdentifier}'.",
        CustomParser = (result) => 
        {
            var value = result.Tokens[0].Value;
            return new Replacement(candidate.OriginalIdentifier, [ RecipeFragment.FromLiteral(value) ]);
        },
        DefaultValueFactory = (result) => new Replacement(candidate.OriginalIdentifier, candidate.DefaultValue, IsDefault: true)
    };

    public static ReplacementSymbols From(ReplacementSettings candidate) => new(
        Option: CreateOption(candidate), 
        Argument: CreateArgument(candidate)
    );

    public Replacement GetValue(ParseResult parseResult)
    {
        var values = new Replacement[] { parseResult.GetRequiredValue(Option), parseResult.GetRequiredValue(Argument) };
        return values.OrderBy(p => p.IsDefault).First();
    }

    public void AddTo(Command command)
    {
        command.Add(Option);
        command.Add(Argument);
    }

    public static IReadOnlyList<ReplacementSymbols> AllFor(Rule rule, IReadOnlyDictionary<string, Assignment> assignments)
    {
        var settings = ReplacementSettings.AllFor(rule, assignments);
        return settings.Select(From).ToArray();
    }
}

public static class ReplacementSymbolsExtensions
{
    public static void AddAllTo(this IEnumerable<ReplacementSymbols> replacementSymbols, Command command)
    {
        foreach (var replacementSymbol in replacementSymbols)
        {
            replacementSymbol.AddTo(command);
        }
    }
}