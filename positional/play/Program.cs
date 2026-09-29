using Hesive;
using Tell;

var builder = new AppBuilder();

builder.Logging.AddNiceShell();

var app = builder.Build();

var rootCommand = new RootCommand()
{
    TellFileSystem.Symbols
};

var initialParseResult = rootCommand.Parse(args);
var tellFiles = TellFileSystem.From(initialParseResult);
var tellContext = tellFiles.ToContext();

rootCommand.MakeRuleCommand(tellContext.Makefile.FirstRule, tellContext, x =>
{
    Console.WriteLine(x);
    return 0;
});

foreach (var rule in tellContext.Makefile.Rules.Values)
{
    var ruleCommand = new RuleInfoCommand(rule).MakeRuleCommand(rule, tellContext, x =>
    {
        Console.WriteLine(x);
        return 0;
    });

    rootCommand.Add(ruleCommand);
}

await rootCommand.Parse(args).InvokeAsync();

static void PrintReplacements(IReadOnlyDictionary<string, string> replacements)
{
    Console.WriteLine("Materialized replacements:");

    foreach (var kvp in replacements)
    {
        Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
    }
}