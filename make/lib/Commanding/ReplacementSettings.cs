using Copaster;

namespace Tell;

public record ReplacementSettings(string OriginalIdentifier, string KebabedIdentifier, RecipeFragment[] DefaultValue)
{
    public static ReplacementSettings From(string originalIdentifier, RecipeFragment[] defaultValue) => new(originalIdentifier, Kebab.Of(originalIdentifier), defaultValue);

    public static ReplacementSettings[] AllFor(Makefile makefile, Rule rule)
    {
        var rules = RuleDependencyTraversal.Ordered(makefile, rule);
        var placeholders = rules.SelectMany(dependency => dependency.Placeholders);
        return AllFor(placeholders, makefile.Assignments);
    }

    public static ReplacementSettings[] AllFor(IEnumerable<Placeholder> placeholders, IReadOnlyDictionary<string, Assignment> assignments) =>
        placeholders.Select(placeholder => From(placeholder.Identifier.Value, assignments)).DistinctBy(settings => settings.OriginalIdentifier).ToArray();

    public static ReplacementSettings From(string identifier, IReadOnlyDictionary<string, Assignment> assignments) =>
        assignments.TryGetValue(identifier, out var assignment)
            ? From(identifier, assignment.ValueFragments)
            : From(identifier, [RecipeFragment.FromLiteral("")]);
}