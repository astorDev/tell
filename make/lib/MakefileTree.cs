global using Superpower;
global using Superpower.Model;
global using Superpower.Parsers;
global using Superpower.Tokenizers;
global using System.CommandLine;

namespace Tell;

public record MakefileTree(
    IReadOnlyList<DocFragment> Fragments
)
{
    public static readonly TextParser<MakefileTree> Parser =
        DocFragment.Parser.Many().Select(From);

    public static MakefileTree Load(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"Makefile not found at `{path}`.");
        var fileContent = File.ReadAllText(path);
        return Parser.Parse(fileContent);
    }

    public static bool TryParse(Copaster.File file, out MakefileTree? makefile, out ParseException? error)
    {
        if (!file.Exists) throw new FileNotFoundException($"Makefile not found at `{file}`.");
        var result = Parser.TryParse(file.Content);
        if (result.HasValue) { makefile = result.Value; error = null; return true; }
        makefile = null; error = new ParseException(result.ToString(), result.ErrorPosition); return false;
    }

    public static MakefileTree From(IReadOnlyList<DocFragment> fragments) => new(fragments);

    public Makefile ToMakefile() => Makefile.From(this);

    public IEnumerable<RuleTree> Rules =>
        Fragments
            .Where(fragment => fragment.RuleTree is not null)
            .Select(fragment => fragment.RuleTree!);

    public IEnumerable<Assignment> Assignments =>
        Fragments
            .Where(fragment => fragment.Assignment is not null)
            .Select(fragment => fragment.Assignment!);
}
