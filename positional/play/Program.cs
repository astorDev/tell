using Hesive;
using Tell;

var builder = new AppBuilder();

builder.Logging.AddNiceShell();

var app = builder.Build();

var rootCommand = new Nishe.RootCommand
{
    WorkingDirectory.ConditionalArgument,
    TellFilename.Option
};

var initialParseResult = rootCommand.Parse(args);
var workdir = initialParseResult.GetRequiredValue(WorkingDirectory.ConditionalArgument);
var filename = initialParseResult.GetRequiredValue(TellFilename.Option);

var filesystem = TellFileSystem.From(workdir, filename);
var context = filesystem.ToContext();

foreach (var rule in context.Makefile.Rules.Values)
{
    var ruleCommand = new RuleInfoCommand(rule).MakeRuleCommand(rule, context, Console.WriteLine);
    rootCommand.Add(ruleCommand);
}

rootCommand.SetDefaultSubcommand(context.Makefile.FirstRule.Name);

var parsed = rootCommand.Parse(args);

await parsed.InvokeAsync();