using System.CommandLine;

namespace Tell;

public record VarUseArgument(
    VarUse VarUse,
    Argument<string> Value
)
{
    public static VarUseArgument From(VarUse variable) => new(
        variable, 
        new Argument<string>(variable.Identifier.Value.ToLower())
        {
            Description = $"The value for the variable '{variable.Identifier.Value}'.",
            Arity = ArgumentArity.ZeroOrOne,
        }
    );

    public static VarUseArgument? OptionalFrom(VarUse? variable) => variable is not null ? From(variable) : null;
}
