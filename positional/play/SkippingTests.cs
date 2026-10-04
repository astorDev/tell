using System.CommandLine.Parsing;
using Nishe;

namespace Playground;

[TestClass]
public class SkippingTests
{
    [TestMethod]
    public void DefaultValueFactoryOnly()
    {
        ParseOneTwo(o =>
        {
            o.DefaultValueFactory = (_) => "def";
        });
    }

    [TestMethod]
    public void CustomParser()
    {
        ParseOneTwo(o =>
        {
            o.CustomParser = (parsing) =>
            {

                if (AlphaBetaGamma.TryGet(parsing, out var value)) return value;
                return "def-from-parser";
            };
        });
    }

    private SkippingResult ParseOneTwo(Action<Argument<string>> optionalConfigure, Func<ParseResult, string?>? getValue = null)
    {
        var optional = Optional.Create(optionalConfigure, getValue);
        var command = new SkippingExperimentCommand(optional);
        var parsed = command.Parse("one two");
        var result = command.FromParseResult(parsed);

        Console.WriteLine(result);
        return result;
    }
}

public class AlphaBetaGamma
{
    public static readonly string[] Values = ["alpha", "beta", "gamma"];
    public static bool Contains(string value) => Values.Contains(value);

    public const string DefaultValue = "alpha";

    public static bool TryGet(ArgumentResult parsing, out string? value)
    {
        var token = parsing.Tokens[0].Value;
        var allowed = Contains(token);
        value = allowed ? token : null;
        return allowed;
    }

    public static readonly ConditionalArgument<string> ConditionalArgument = new("conditional", Contains, DefaultValue);
}

public class SkippingExperimentCommand : Command
{
    private readonly Optional optional;

    public static readonly Argument<string> P1 = new("p1")
    {
        Arity = ArgumentArity.ZeroOrOne,
        DefaultValueFactory = (_) => "p1-def"
    };

    public static readonly Argument<string> P2 = new("p2")
    {
        Arity = ArgumentArity.ZeroOrOne,
        DefaultValueFactory = (_) => "p2-def"
    };

    public SkippingExperimentCommand(Optional optional) : base("skipping-experiment")
    {
        this.Add(optional.Arg);
        this.Add(P1);
        this.Add(P2);

        this.optional = optional;
    }

    public SkippingResult FromParseResult(ParseResult parseResult) =>
        new(
            optional.GetValue(parseResult),
            parseResult.GetValue(P1),
            parseResult.GetValue(P2)
        );
}

public record Optional(Argument<string> Arg, Func<ParseResult, string?> GetValue)
{
    public static Optional Create(Action<Argument<string>> configure, Func<ParseResult, string?>? getValue = null)
    {
        var arg = new Argument<string>("optional")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };

        configure(arg);
        return new Optional(arg, getValue ?? ((parseResult) => parseResult.GetValue(arg)));
    }
}

public record SkippingResult(
    string? OptionalValue,
    string? P1,
    string? P2
)
{
    public override string ToString() => $"OptionalValue: {OptionalValue}, P1: {P1}, P2: {P2}";

    public static SkippingResult From(ParseResult parseResult, Func<ParseResult, string?> OptionValueExtractor) => new(
        OptionValueExtractor(parseResult) ?? "null",
        parseResult.GetValue(SkippingExperimentCommand.P1),
        parseResult.GetValue(SkippingExperimentCommand.P2)
    );

    public static SkippingResult PrintedFrom(ParseResult parseResult, Func<ParseResult, string?> OptionValueExtractor)
    {
        var result = From(parseResult, OptionValueExtractor);
        Console.WriteLine(result);
        return result;
    }
}