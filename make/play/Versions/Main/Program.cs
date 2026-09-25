global using Tell;
global using Superpower;
global using Microsoft.Extensions.DependencyInjection;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.Services.AddSingleton<Startup>();
builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<RecipeRunner>();

using var app = builder.Build("A tell.doc.runner CLI application.");

var startup = app.Services.GetRequiredService<Startup>();
var runner = app.Services.GetRequiredService<RuleRunner>();

var runParseResult = startup.Parse(args);
var runParams = startup.Interpret(runParseResult);

var runCommand = new RunRuleCommand(runParams, runner);
var ruleCommandParseResult = runCommand.Parse(runParams.Args);
await ruleCommandParseResult.InvokeAsync();