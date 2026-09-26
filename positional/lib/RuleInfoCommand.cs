namespace Tell;

public class RuleInfoCommand(Rule rule) : Command(NameFrom(rule), DescriptionFrom(rule.Recipes))
{
    public Rule Rule => rule;

    public const string DescriptionPrefix = "Executes: ";
    public static readonly string DescriptionNextLinePrefix = new(' ', DescriptionPrefix.Length);
    public static readonly string DescriptionSeparator = $"{Environment.NewLine}{DescriptionNextLinePrefix}";

    public static string DescriptionFrom(IEnumerable<Recipe> recipes) 
    {
        var recipeLines = recipes.Select(r => r.ToUnresolvedCommandString());
        return $"{DescriptionPrefix}{String.Join(DescriptionSeparator, recipeLines)}";
    }

    public static string NameFrom(Rule rule)
    {
        return rule.Target.Identifier.Value;
    }
}
