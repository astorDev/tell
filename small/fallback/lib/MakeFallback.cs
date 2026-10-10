using Microsoft.Extensions.Logging;
using NiceShell;

namespace Tell;

public record MakeFallback( 
    string? WorkingDirectoryChange, 
    string? Filename,
    string? RuleName
)
{
    public string ToMakeArguments()
    {
        var list = GetMakeArguments();
        return string.Join(" ", list);
    }

    public IEnumerable<string> GetMakeArguments()
    {
        if (WorkingDirectoryChange is not null) yield return $"-C {WorkingDirectoryChange}";
        if (Filename is not null) yield return $"-f {Filename}";
        if (RuleName is not null) yield return $"{RuleName}";
    }

    public async Task<int> Execute(ILogger logger)
    {
        logger.LogTrace("Fallback params: {params}", this);

        var arguments = this.ToMakeArguments();
        logger.LogDebug("Attempting to fallback to make with arguments: {args}", arguments);

        var command = new System.Diagnostics.ProcessStartInfo("make", arguments);
        var result = await command.Run();
        return result.ExitCode;
    }
}