namespace Tell;

public record VariablesContext(VarUseCommandParams VarUseParams, IReadOnlyList<Assignment> Assignments)
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
    public static VariablesContext ToVariablesContext(this RuleRunParams parameters)
    {
        var varUseParams = VarUseCommandParams.From(parameters.Rule.Placeholders);
        return new(varUseParams, parameters.Doc.Assignments);
    }

    public static Task Run(this RuleRunner runner, IEnumerable<Recipe> recipes, string workingDirectory, VariablesContext variablesContext, ParseResult parseResult)
    {
        var variables = variablesContext.GetFinalVariables(parseResult);
        return runner.Run(recipes, workingDirectory, variables);
    }
}