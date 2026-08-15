using Microsoft.Extensions.DependencyInjection;
using Tell;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();
//builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddSingleton<RecipeRunner>();
builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<Startup>();

using var app = builder.Build("A tell.rule.runner CLI application.");

var startup = app.Services.GetRequiredService<Startup>();

var runParseResult = startup.Parse(args);
var startupResult = startup.Interpret(runParseResult);
var ruleCommand = RunRuleCommand.From(
    startupResult.Rule, 
    app.Services.GetRequiredService<RuleRunner>(), 
    startupResult.WorkingDirectory
);

await ruleCommand.Run(startupResult.Args);