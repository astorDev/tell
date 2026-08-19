namespace Tell;

public record Assignment(
    Identifier Target,
    AssignmentOperator Operator,
    RecipeFragment[] ValueFragments
)
{
    public static readonly TextParser<Assignment> Parser =
        from target in Identifier.Parser
        from ws1 in Character.WhiteSpace.Optional()
        from op in AssignmentOperator.Parser
        from ws2 in Character.WhiteSpace.Optional()
        from vf in RecipeFragment.Parser.Many()
        from nl in NewLine.SpanParser.OptionalOrDefault()
        select new Assignment(Target: target, Operator: op, ValueFragments: vf);

    public KeyValuePair<string, string>? OptionalVariableToSet(IReadOnlyDictionary<string, string> existingVariables)
    {
        if (Operator.IsEqualsOperator) return ToKeyValuePair(existingVariables);
        if (Operator.IsQuestionEquals && !existingVariables.ContainsKey(Target.Value)) return ToKeyValuePair(existingVariables);

        return null;
    }

    public KeyValuePair<string, string> ToKeyValuePair(IReadOnlyDictionary<string, string> existingVariables)
    {
        var value = ValueFragments.ToCommandString(existingVariables);
        return new KeyValuePair<string, string>(Target.Value, value);
    }
}

public static class AssigmentExtesions
{
    public static IReadOnlyDictionary<string, string> TransformVariables(this IEnumerable<Assignment> assignments, IReadOnlyDictionary<string, string> existingVariables)
    {
        var variables = new Dictionary<string, string>(existingVariables);

        foreach (var assignment in assignments) // TODO: Order assignments by their dependencies (e.g., if one variable depends on another)
        {
            var variableToSet = assignment.OptionalVariableToSet(variables);
            if (variableToSet is not null)
            {
                variables[variableToSet.Value.Key] = variableToSet.Value.Value;
            }
        }

        return variables;
    }
}