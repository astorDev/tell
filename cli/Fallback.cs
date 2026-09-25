using System.Text;

namespace Tell;

public record MakeFallbackParams(
    Exception ParsingError, 
    string? WorkingDirectoryChange, 
    string? RuleName,
    string? Filename
)
{
    public string ToMakeArguments()
    {
        var makeArgumentsBuilder = new StringBuilder();

        if (WorkingDirectoryChange is not null)
        {
            makeArgumentsBuilder.Append($" -C {WorkingDirectoryChange}");
        }

        if (Filename is not null)
        {
            makeArgumentsBuilder.Append($" -f {Filename}");
        }

        if (RuleName is not null)
        {
            makeArgumentsBuilder.Append($" {RuleName}");
        }

        return makeArgumentsBuilder.ToString();
    }
}