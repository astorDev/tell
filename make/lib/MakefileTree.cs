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

    public static Result<MakefileTree> Parsing(Copaster.File file)
    {
        if (!file.Exists) throw new FileNotFoundException($"Makefile not found at `{file}`.");
        return Parser.TryParse(file.Content);
    }

    public static bool TryParse(Copaster.File file, out MakefileTree? makefile, out ParseException? error)
    {
        var result = Parsing(file);
        return result.AsClassicTry(out makefile, out error);
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

// TODO: Remove in favor of version in superpower/lib when it's available as nuget
public static class ResultExtension
{
    public static bool AsClassicTry<T>(this Result<T> result, out T? value, out ParseException? error) => 
        result.AsClassicTry(x => x, out value, out error);

    public static bool AsClassicTry<TDirect, TFinal>(this Result<TDirect> result, Func<TDirect, TFinal> converter, out TFinal? value, out ParseException? error)
    {
        if (result.HasValue)
        {
            value = converter(result.Value);
            error = null;
            return true;
        }

        value = default;
        error = new ParseException(result.ToString(), result.ErrorPosition);

        return false;
    }

    public static T Thrown<T>(this Result<T> result)
    {
        if (result.HasValue) return result.Value;
        throw new ParseException(result.ToString(), result.ErrorPosition);
    }
}