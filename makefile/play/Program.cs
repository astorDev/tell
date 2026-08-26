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

var runCommand = ParseOnlyRunRuleCommand.From(runParams.Rule);
var ruleCommandParseResult = runCommand.Parse(runParams.Args);
var parsedVarValues = runCommand.VarUseParams.GetVarValues(ruleCommandParseResult);
var varValues = runParams.Assignments.TransformVariables(parsedVarValues);

await runner.Run(runParams.Rule, runParams.WorkingDirectory, varValues);