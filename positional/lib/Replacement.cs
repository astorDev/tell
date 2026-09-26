namespace Tell;

public record Replacement(string Identifier, RecipeFragment[] Value, bool IsDefault = false)
{
    override public string ToString()
    {
        var commandString = Value.ToUnresolvedCommandString();
        return String.IsNullOrEmpty(commandString) ? "\"\"" : commandString;
    }
}

public static class ReplacementExtensions
{
    public static IReadOnlyDictionary<string, string> Materialize(this IEnumerable<Replacement> replacements)
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var replacement in replacements)
        {
            dictionary[replacement.Identifier] = replacement.Value.ToCommandString(dictionary);
        }
        return dictionary;
    }
}
