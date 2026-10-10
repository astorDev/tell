using Hesive;
using Tell;

namespace Playground;

[TestClass]
public class ExampleWorkdirTests
{
    [TestMethod]
    [DataRow("examples", "Hello", "World")]
    [DataRow("examples greet-from-example Servus Egor", "Servus", "Egor")]
    [DataRow("examples Servus Egor", "Servus", "Egor")]
    public void All(string command, string expectedGreeting, string expectedName)
    {
        var result = TellRun.On(command);
        Console.WriteLine(result);
        result.Replacements.ShouldContainKeyAndValue("GREETING", expectedGreeting);
        result.Replacements.ShouldContainKeyAndValue("NAME", expectedName);
    }
}

public class TellRun
{
    public static RuleRunParams On(string args)
    {
        var parts = args.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        return On(parts);
    }

    public static RuleRunParams On(string[] args)
    {
        RuleRunParams result = default!;

        var builder = new AppBuilder();

        builder.Logging.AddNiceShell();

        var app = builder.Build();

        var rootCommand = new Nishe.RootCommand("Tell Run in Tests")
        {
            WorkingDirectoryCli.ConditionalArgument,
            TellFilename.Option
        };

        var initialParseResult = rootCommand.Parse(args);
        var workdir = initialParseResult.GetRequiredValue(WorkingDirectoryCli.ConditionalArgument);
        var filename = initialParseResult.GetRequiredValue(TellFilename.Option);
        var tellFiles = TellFileSystem.From(workdir, filename);
        var makefile = Makefile.From(tellFiles.File);

        foreach (var rule in makefile.Rules.Values)
        {
            var ruleCommand = new RuleInfoCommand(rule).MakeRuleCommand(rule, tellFiles.WorkingDir, (x) =>
            {
                result = x;
                Console.WriteLine(result);
                return 0;
            });

            rootCommand.Add(ruleCommand);
        }

        rootCommand.SetDefaultSubcommand(makefile.FirstRule.Name);

        Console.WriteLine($"Initial parse result unmatched tokens: {String.Join(", ", initialParseResult.UnmatchedTokens)}");

        var parsed = rootCommand.Parse(args);

        parsed.Invoke();

        return result;
    }
}