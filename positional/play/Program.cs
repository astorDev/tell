using Hesive;
using Tell;

var builder = new AppBuilder();

builder.Logging.AddNiceShell();

var app = builder.Build();

var rootCommand = new RootCommand();
rootCommand.AddTellContextSymbols();

var initialParseResult = rootCommand.Parse(args);
var tellContext = initialParseResult.GetRequiredValue(TellContext.Argument);

foreach (var rule in tellContext.Makefile.Rules.Values)
{
    var replacementSymbols = ReplacementSymbols.AllFor(rule, tellContext.Makefile.Assignments);
    var printingCommand = new RulePrintingCommand(rule, replacementSymbols);
    rootCommand.Add(printingCommand);
}

await rootCommand.Parse(args).InvokeAsync();

public class RulePrintingCommand(Rule rule, IReadOnlyList<ReplacementSymbols> replacementSymbols) : RuleCommandBase(rule, replacementSymbols)
{
    public override void Execute(IReadOnlyDictionary<string, string> replacements)
    {
        Console.WriteLine("Materialized replacements:");

        foreach (var kvp in replacements)
        {
            Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
        }
    }
}