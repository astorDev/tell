using Superpower;

namespace Tell;

public record WorkingDirectory(string Path)
{
    public static WorkingDirectory Default => new (Directory.GetCurrentDirectory());

    public static bool TryUse(string workdirArgument, out WorkingDirectory workingDirectory)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = System.IO.Path.Combine(currentDirectory, workdirArgument);
        workingDirectory = new WorkingDirectory(path);
        return Directory.Exists(path);
    }

    public MakefileSearchResult GetMakefile(string? file)
    {
        var makefilePath = System.IO.Path.Combine(Path, file ?? "Makefile");
        if (!File.Exists(makefilePath))
        {
            throw new FileNotFoundException($"Makefile not found at {makefilePath}");
        }

        var makefileContent = File.ReadAllText(makefilePath);
        var doc = Doc.Parser.Parse(makefileContent);
        return new (makefilePath, doc);
    }
}

public record MakefileSearchResult(
    string Path,
    Doc Doc
);