using Microsoft.Extensions.DependencyInjection;
using Tell;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.Services.AddSingleton<Startup>();
builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<RecipeRunner>();

using var app = builder.Build("A tell.doc.runner CLI application.");

var startup = app.Services.GetRequiredService<Startup>();

var runParseResult = startup.Parse(args);
var runParams = startup.Interpret(runParseResult);

var runCommand = RunRuleCommand.From(
    runParams.Rule, 
    app.Services.GetRequiredService<RuleRunner>(), 
    runParams.WorkingDirectory
);

var ruleCommandParseResult = runCommand.Parse(runParams.Args);
await runCommand.Execute(ruleCommandParseResult);