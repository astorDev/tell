namespace Playground;

[TestClass]
public class ConditionalTests
{
    [TestMethod]
    public void BuiltIn()
    {
        var customParsable = new Argument<string>("customParsable")
        {
            Arity = ArgumentArity.ZeroOrOne,
            CustomParser = (parsing) =>
            {
                if (NumbersWhitelist.TryGet(parsing, out var value)) return value;
                parsing.OnlyTake(0);
                return "def-from-parser";
            }
        };

        var command = new RootCommand()
        {
            customParsable,
            SkippingExperimentCommand.P1,
            SkippingExperimentCommand.P2
        };

        var parseResult = command.Parse("one two");
        var result = SkippingResult.From(parseResult, pr => pr.GetValue(customParsable) ?? "def-after-get-value");

        Console.WriteLine(result);

        result.OptionalValue.ShouldBe("def-from-parser");

        // The lines below will fail, since values will be mixed up (P1 = two, P2 = one)
        // The reason is that two gets assigned to P1 on the first "unvalidated" run.
        // Therefore after OnlyTake(0) "one" gets assigned to the next available positional argument, which is P2.
        result.P1.ShouldBe("one");
        result.P2.ShouldBe("two");
    }

    [TestMethod]
    public void AdvancedCommand()
    {
        var whitelister = new ConditionalArgument<string>(
            "optional",
            (value) => new[] { "alpha", "beta", "gamma" }.Contains(value),
            "alpha"
        );

        var command = new AdvancedRootCommand()
        {
            whitelister,
            SkippingExperimentCommand.P1,
            SkippingExperimentCommand.P2
        };

        var parsed = command.Parse("one two");
        var result = new SkippingResult(
            parsed.GetValue(whitelister),
            parsed.GetValue(SkippingExperimentCommand.P1),
            parsed.GetValue(SkippingExperimentCommand.P2)
        );

        result.OptionalValue.ShouldBe("alpha");
        result.P1.ShouldBe("one");
        result.P2.ShouldBe("two");
    }
}
