using Hesive;
using Tell;

var builder = new AppBuilder();

builder.Logging.AddNiceShell();

var app = builder.Build();

var rootCommand = new Nishe.RootCommand("Positional play commands")
{
    WorkingDirectoryCli.ConditionalArgument,
    TellFilename.Option
};

var initialParseResult = rootCommand.Parse(args);
var workdir = initialParseResult.GetRequiredValue(WorkingDirectoryCli.ConditionalArgument);
var filename = initialParseResult.GetRequiredValue(TellFilename.Option);

var filesystem = TellFileSystem.From(workdir, filename);
var makefile = MakefileTree.Load(filesystem.File.Path).ToMakefile();

foreach (var rule in makefile.Rules.Values)
{
    var ruleCommand = new RuleInfoCommand(rule).MakeRuleCommand(rule, filesystem.WorkingDir, Console.WriteLine);
    rootCommand.Add(ruleCommand);
}

rootCommand.SetDefaultSubcommand(makefile.FirstRule.Name);

var parsed = rootCommand.Parse(args);

await parsed.InvokeAsync();