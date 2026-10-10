namespace Tell;

public record VariablesContext(VarUseCommandParams VarUseParams, IReadOnlyDictionary<string, Assignment> Assignments)
{
    public IReadOnlyDictionary<string, string> GetFinalVariables(ParseResult parseResult)
    {
        var variables = this.VarUseParams.GetVarValues(parseResult);
        variables = Assignments.TransformVariables(variables);

        return variables;
    }
}

public static class VariablesContextExtensions
{
    public static VariablesContext ToVariablesContext(this RuleCommandBuildingParams parameters)
    {
        var placeholders = parameters.Rule.RulesFromDependenciesAndBody
            .SelectMany(rule => rule.Placeholders)
            .DistinctBy(placeholder => placeholder.Identifier.Value);
            
        var varUseParams = VarUseCommandParams.From(placeholders);
        return new(varUseParams, parameters.Rule.Assignments);
    }
}