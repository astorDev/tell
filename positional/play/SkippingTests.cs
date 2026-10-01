using System.CommandLine.Parsing;

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
                if (InWhitelist(parsing, out var value)) return value;
                return "def-from-parser";
            };
        });
    }

    [TestMethod]
    public void CustomParserWithOnlyTakeZero()
    {
        var result = ParseOneTwo(o =>
        {
            o.CustomParser = (parsing) =>
            {
                if (InWhitelist(parsing, out var value)) return value;

                parsing.OnlyTake(0);
                return "def-from-parser";
            };
        });

        result.OptionalValue.ShouldBe("def-from-parser");

        // Actually expected:
        // result.P1.ShouldBe("one");
        // result.P2.ShouldBe("two");

        // Received:
        result.P1.ShouldBe("two");
        result.P2.ShouldBe("one");

        // The reason is that two gets assigned to P1 on the first "unvalidated" run.
        // Therefore after OnlyTake(0) "one" gets assigned to the next available positional argument, which is P2.
    }

    [TestMethod]
    public void CustomParseWithError()
    {
        ParseOneTwo(o =>
            {
                o.CustomParser = (parsing) =>
                {
                    if (InWhitelist(parsing, out var value)) return value;

                    parsing.AddError("Not in whitelist");
                    return "def-from-parser";
                };
            },
            getValue: (parseResult) => "defaulted from error"
        );
    }

    public static bool InWhitelist(ArgumentResult parsing, out string? value)
    {
        var whitelist = new[] { "alpha", "beta" };

        var token = parsing.Tokens[0].Value;
        var allowed = whitelist.Contains(token);
        value = allowed ? token : null;
        return allowed;
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
}