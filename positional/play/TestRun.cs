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

        var rootCommand = new RootCommand("Tell Run in Tests")
        {
            TellFileSystem.Symbols
        };

        var initialParseResult = rootCommand.Parse(args);
        var tellFiles = TellFileSystem.From(initialParseResult);
        var tellContext = tellFiles.ToContext();

        foreach (var rule in tellContext.Makefile.Rules.Values)
        {
            var ruleCommand = new RuleInfoCommand(rule).MakeRuleCommand(rule, tellContext, (x) =>
            {
                result = x;
                Console.WriteLine(result);
                return 0;
            });

            rootCommand.Add(ruleCommand);
        }

        Console.WriteLine($"Initial parse result unmatched tokens: {String.Join(", ", initialParseResult.UnmatchedTokens)}");

        var parsed = rootCommand.ParseWithDefaultCommand(args, tellContext.Makefile.FirstRule.Name);

        parsed.Invoke();

        return result;
    }
}