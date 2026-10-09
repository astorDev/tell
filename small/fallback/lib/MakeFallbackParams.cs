namespace Tell;

public record MakeFallbackParams( 
    string? WorkingDirectoryChange, 
    string? RuleName,
    string? Filename
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
}