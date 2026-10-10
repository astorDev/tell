global using Tell;
global using Superpower;
global using Microsoft.Extensions.DependencyInjection;
using Copaster;
using Hesive;
using Microsoft.Extensions.Logging;

var builder = new AppBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNiceShell();

builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<RecipeRunner>();

var app = builder.Build();

var runner = app.Services.GetRequiredService<RuleRunner>();

var root = new RootCommand()
{
    TellFilename.Option
};

var initialParse = root.Parse(args);
var filename = initialParse.GetRequiredValue(TellFilename.Option);

var folder = new Folder(".");
var fs = TellFileSystem.From(folder, filename);
var makefile = MakefileTree.Load(fs.File.Path).ToMakefile();

foreach (var rule in makefile.Rules.Values)
{
    var ruleCommand = new RuleInfoCommand(rule);

    ruleCommand.MakeRuleCommand(rule, fs.WorkingDir, runner.Run);

    root.Add(ruleCommand);
}

return await root.Parse(args).InvokeAsync();