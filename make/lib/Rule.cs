namespace Tell;

public record Rule(
    Identifier Target,
    IReadOnlyList<Rule> Dependencies,
    Recipe[] Recipes,
    IReadOnlyDictionary<string, Assignment> Assignments
)
{
    public string Name => Target.Value;

    public IEnumerable<Placeholder> Placeholders =>
        Recipes.SelectMany(recipe => recipe.Placeholders).DistinctBy(placeholder => placeholder.Identifier.Value);

    public IReadOnlyList<Rule> DependenciesAndSelf =>
        Dependencies
            .SelectMany(dependency => dependency.DependenciesAndSelf)
            .Append(this)
            .DistinctBy(rule => rule.Name)
            .ToArray();

    public IReadOnlyList<Recipe> RecipesFromDependenciesAndBody =>
        DependenciesAndSelf.SelectMany(rule => rule.Recipes).ToArray();

    public override string ToString() =>
        $"{Target}: {string.Join(" ", Dependencies.Select(dependency => dependency.Name))}\n{string.Join("\n", Recipes.Select(recipe => $"  {recipe}"))}";
}
