using Hesive;
using Tell;

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
            WorkingDirectoryCli.CreateConditionalArgument,
            TellFilename.Option
        };

        var initialParseResult = rootCommand.Parse(args);
        var workdir = initialParseResult.GetRequiredValue(WorkingDirectoryCli.CreateConditionalArgument);
        var filename = initialParseResult.GetRequiredValue(TellFilename.Option);
        var tellFiles = TellFileSystem.From(workdir, filename);
        var makefile = MakefileTree.Load(tellFiles.File.Path).ToMakefile();

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