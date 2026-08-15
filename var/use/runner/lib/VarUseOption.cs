using System.CommandLine;

namespace Tell;

public record VarUseOption(
    VarUse VarUse,
    Option<string> Value
)
{
    public static VarUseOption From(VarUse variable) => new(
        variable, 
        new Option<string>($"--{variable.Identifier.Value.ToLower()}")
        {
            Description = $"The value for the variable '{variable.Identifier.Value}'.",
            Required = false
        }
    );
}
