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

foreach (var rule in tellContext.Makefile.Rules.Values)
{
    var ruleCommand = new RuleInfoCommand(rule).MakeRuleCommand(rule, tellContext, Console.WriteLine);
    rootCommand.Add(ruleCommand);
}

var parsed = rootCommand.ParseWithDefaultCommand(args, tellContext.Makefile.FirstRule.Name);

await parsed.InvokeAsync();