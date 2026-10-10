using Copaster;

namespace Tell;

public record ReplacementSettings(string OriginalIdentifier, string KebabedIdentifier, RecipeFragment[] DefaultValue)
{
    public static ReplacementSettings From(string originalIdentifier, RecipeFragment[] defaultValue) => new(originalIdentifier, Kebab.Of(originalIdentifier), defaultValue);

    public static ReplacementSettings[] AllFor(Rule rule)
    {
        var rules = rule.RulesFromDependenciesAndBody;
        var placeholders = rules.SelectMany(dependency => dependency.Placeholders);
        return AllFor(placeholders, rule.Assignments);
    }

    public static ReplacementSettings[] AllFor(IEnumerable<Placeholder> placeholders, IReadOnlyDictionary<string, Assignment> assignments) =>
        placeholders.Select(placeholder => From(placeholder.Identifier.Value, assignments)).DistinctBy(settings => settings.OriginalIdentifier).ToArray();

    public static ReplacementSettings From(string identifier, IReadOnlyDictionary<string, Assignment> assignments) =>
        assignments.TryGetValue(identifier, out var assignment)
            ? From(identifier, assignment.ValueFragments)
            : From(identifier, [RecipeFragment.FromLiteral("")]);
}