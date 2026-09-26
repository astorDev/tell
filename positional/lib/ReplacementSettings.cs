namespace Tell;

public record ReplacementSettings(string OriginalIdentifier, string KebabedIdentifier, RecipeFragment[] DefaultValue)
{
    public static ReplacementSettings From(string originalIdentifier, RecipeFragment[] defaultValue) => new(originalIdentifier, Kebab.Of(originalIdentifier), defaultValue);

    public static ReplacementSettings[] AllFor(Rule rule, IReadOnlyDictionary<string, Assignment> assignments)
    {
        var placeholders = rule.Placeholders
            .Select(p => p.Identifier.Value)
            .Select(p => 
            {
                if (!assignments.TryGetValue(p, out var assignment))
                {
                    return From(p, [ RecipeFragment.FromLiteral("") ]);
                }

                return From(p, assignment.ValueFragments);
            });

        return placeholders.ToArray();
    }
}