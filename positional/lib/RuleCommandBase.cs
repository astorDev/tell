namespace Tell;

public abstract class RuleCommandBase : RuleInfoCommand
{
    private readonly IReadOnlyList<ReplacementSymbols> replacementSymbols;

    public RuleCommandBase(Rule rule, IReadOnlyList<ReplacementSymbols> replacementSymbols) : base(rule)
    {
        replacementSymbols.AddAllTo(this);
        this.replacementSymbols = replacementSymbols;
        this.SetAction(Execute);
    }

    public async Task Execute(ParseResult parseResult)
    {
        var replacementRules = replacementSymbols.Select(rs => rs.GetValue(parseResult));
        var materializedReplacements = replacementRules.Materialize();

        await Execute(materializedReplacements);
    }

    public abstract Task Execute(IReadOnlyDictionary<string, string> replacements);
}
