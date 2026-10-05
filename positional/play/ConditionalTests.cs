using Nishe;
using Tell;

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
                if (AlphaBetaGamma.TryGet(parsing, out var value)) return value;
                parsing.OnlyTake(0);
                return AlphaBetaGamma.DefaultValue;
            }
        };

        var command = new System.CommandLine.RootCommand()
        {
            customParsable,
            SkippingExperimentCommand.P1,
            SkippingExperimentCommand.P2
        };

        var parseResult = command.Parse("one two");
        var result = SkippingResult.From(parseResult, pr => pr.GetValue(customParsable) ?? "def-after-get-value");

        Console.WriteLine(result);

        result.OptionalValue.ShouldBe(AlphaBetaGamma.DefaultValue);

        // The lines below will fail, since values will be mixed up (P1 = two, P2 = one)
        // The reason is that two gets assigned to P1 on the first "unvalidated" run.
        // Therefore after OnlyTake(0) "one" gets assigned to the next available positional argument, which is P2.
        //
        // result.P1.ShouldBe("one");
        // result.P2.ShouldBe("two");
    }

    [TestMethod]
    public void AdvancedCommand()
    {
        var conditional = AlphaBetaGamma.ConditionalArgument;
        var command = new Nishe.RootCommand("Advanced command with conditional argument")
        {
            conditional,
            SkippingExperimentCommand.P1,
            SkippingExperimentCommand.P2
        };

        var parsed = command.Parse("one two");
        var result = SkippingResult.PrintedFrom(parsed, (p) => parsed.GetValue(conditional));

        result.OptionalValue.ShouldBe(AlphaBetaGamma.DefaultValue);
        result.P1.ShouldBe("one");
        result.P2.ShouldBe("two");
    }

    [TestMethod]
    [DataRow("one two", "alpha", "one", "two")]
    [DataRow("--name foo one two", "alpha", "one", "two")]
    [DataRow("--name alpha one two", "alpha", "one", "two")]
    [DataRow("--name=foo one two", "alpha", "one", "two")]
    [DataRow("one --name foo two", "alpha", "one", "two")]
    [DataRow("one two --name foo", "alpha", "one", "two")]
    [DataRow("--flag one two", "alpha", "one", "two")]
    [DataRow("beta one two", "beta", "one", "two")]
    [DataRow("--name foo beta --flag one two", "beta", "one", "two")]
    [DataRow("--name beta one", "alpha", "one", null)]
    [DataRow("--name foo", "alpha", null, null)]
    [DataRow("--name one one two", "alpha", "one", "two")]
    public void AdvancedCommandWithOptions(string args, string optional, string? p1, string? p2)
    {
        var whitelister = AlphaBetaGamma.ConditionalArgument;
        var name = new Option<string>("--name");
        var flag = new Option<bool>("--flag");

        var command = new Nishe.RootCommand("Advanced command with conditional argument")
        {
            name,
            whitelister,
            flag,
            SkippingExperimentCommand.P1,
            SkippingExperimentCommand.P2
        };

        var parsed = command.Parse(args);
        var result = SkippingResult.PrintedFrom(parsed, (p) => parsed.GetValue(whitelister));

        parsed.GetValue(whitelister).ShouldBe(optional);
        parsed.GetValue(SkippingExperimentCommand.P1).ShouldBe(p1 ?? "p1-def");
        parsed.GetValue(SkippingExperimentCommand.P2).ShouldBe(p2 ?? "p2-def");
    }

    [TestMethod]
    public void TwoConditionalArgumentsWithOptions()
    {
        var first = new ConditionalArgument<string>("first", v => v is "a1" or "a2", "a1");
        var second = new ConditionalArgument<string>("second", v => v is "b1" or "b2", "b1");
        var name = new Option<string>("--name");

        var command = new Nishe.RootCommand("Two conditional arguments with options") { name, first, second, SkippingExperimentCommand.P1 };

        var parsed = command.Parse("--name x rest");
        parsed.GetValue(first).ShouldBe("a1");
        parsed.GetValue(second).ShouldBe("b1");
        parsed.GetValue(SkippingExperimentCommand.P1).ShouldBe("rest");

        parsed = command.Parse("a2 --name x rest");
        parsed.GetValue(first).ShouldBe("a2");
        parsed.GetValue(second).ShouldBe("b1");
        parsed.GetValue(SkippingExperimentCommand.P1).ShouldBe("rest");

        parsed = command.Parse("a2 --name x b2 rest");
        parsed.GetValue(first).ShouldBe("a2");
        parsed.GetValue(second).ShouldBe("b2");
        parsed.GetValue(SkippingExperimentCommand.P1).ShouldBe("rest");
        parsed.GetValue(name).ShouldBe("x");
    }

    [TestMethod]
    [DataRow("", "run", "alpha", null, null, null)]
    [DataRow("list", "list", "alpha", null, null, null)]
    [DataRow("beta", "run", "beta", null, null, null)]
    [DataRow("beta list", "list", "beta", null, null, null)]
    [DataRow("--name x", "run", "alpha", "x", null, null)]
    [DataRow("--name x list", "list", "alpha", "x", null, null)]
    [DataRow("target", "run", "alpha", null, null, "target")]
    [DataRow("beta target", "run", "beta", null, null, "target")]
    [DataRow("--opt 5", "run", "alpha", null, "5", null)]
    [DataRow("--name x --opt 5 target", "run", "alpha", "x", "5", "target")]
    [DataRow("beta --name x --opt 5 target", "run", "beta", "x", "5", "target")]
    [DataRow("run --opt 5", "run", "alpha", null, "5", null)]
    [DataRow("beta run --opt 5 target", "run", "beta", null, "5", "target")]
    public void DefaultSubcommandWithOptions(string args, string subcommand, string optional, string? name, string? opt, string? target)
    {
        var whitelister = AlphaBetaGamma.ConditionalArgument;
        var nameOption = new Option<string>("--name");
        var runOption = new Option<string>("--opt");
        var runTarget = new Argument<string>("target") { Arity = ArgumentArity.ZeroOrOne };

        var command = new Nishe.RootCommand("Default subcommand with options"   )
        {
            nameOption,
            whitelister,
            new Command("run") { runOption, runTarget },
            new Command("list")
        };
        command.SetDefaultSubcommand("run");

        var parsed = command.Parse(args);

        parsed.Errors.ShouldBeEmpty();
        parsed.CommandResult.Command.Name.ShouldBe(subcommand);
        parsed.GetValue(whitelister).ShouldBe(optional);
        parsed.GetValue(nameOption).ShouldBe(name);
        parsed.GetValue(runOption).ShouldBe(opt);
        parsed.GetValue(runTarget).ShouldBe(target);
    }

    [TestMethod]
    [DataRow("--help")]
    [DataRow("-h")]
    [DataRow("-?")]
    [DataRow("/h")]
    [DataRow("/?")]
    public void DefaultSubcommandIsNotAppliedForHelp(string helpOption)
    {
        var command = new Nishe.RootCommand("Default subcommand with options")
        {
            new Command("run"),
            new Command("list")
        };
        command.SetDefaultSubcommand("run");

        var parsed = command.Parse(helpOption);

        parsed.CommandResult.Command.ShouldBe(command);
    }
}
