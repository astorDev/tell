namespace Tell;

public record VarUseCommandParams(
    VarUseCommandParams.Argument? Arg,
    IEnumerable<VarUseCommandParams.Option> Options
)
{
    public static VarUseCommandParams From(IEnumerable<Placeholder> varUses) => new(
        Argument.OptionalFrom(varUses.FirstOrDefault()),
        varUses.Select(Option.From).ToList()
    );

    public void AddTo(Command command)
    {
        if (Arg is not null) command.Add(Arg.Value);
        foreach (var option in Options) command.Add(option.Value);
    }

    public IReadOnlyDictionary<string, string> GetVarValues(ParseResult parseResult)
    {
        var argValues = new Dictionary<string, string>();
        
        if (Arg is not null)
        {
            var firstArgValue = parseResult.GetValue(this.Arg.Value);
            if (firstArgValue is not null)
            {
                argValues[Arg.Placeholder.Identifier.Value] = firstArgValue;
            }
        }

        foreach (var option in Options)
        {
            var optionValue = parseResult.GetValue(option.Value);
            if (optionValue is not null)
            {
                argValues[option.Placeholder.Identifier.Value] = optionValue;
            }
        }

        return argValues;
    }

    public record Argument(Placeholder Placeholder, Argument<string> Value)
    {
        public static Argument From(Placeholder placeholder) => new(
            placeholder,
            new Argument<string>(placeholder.Identifier.Value.ToLower())
            {
                Description = $"The value for the variable '{placeholder.Identifier.Value}'.",
                Arity = ArgumentArity.ZeroOrOne,
            }
        );

        public static Argument? OptionalFrom(Placeholder? placeholder) => placeholder is not null ? From(placeholder) : null;
    }

    public record Option(Placeholder Placeholder, Option<string> Value)
    {
        public static Option From(Placeholder variable) => new(
            variable,
            new Option<string>($"--{variable.Identifier.Value.ToLower()}")
            {
                Description = $"The value for the variable '{variable.Identifier.Value}'.",
                Required = false
            }
        );
    }
}

