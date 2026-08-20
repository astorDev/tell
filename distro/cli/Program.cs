using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tell;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<RecipeRunner>();

builder.AddCommand<TellCommand>();

using var app = builder.Build("A tell CLI application.");

var tell = app.Services.GetRequiredService<TellCommand>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
var runner = app.Services.GetRequiredService<RuleRunner>();

try
{
    var runParams = tell.GetRunRuleParams(args);

    var runCommand = RunRuleCommand.From(runParams.Rule);
    var ruleCommandParseResult = runCommand.Parse(runParams.Args);
    var parsedVarValues = runCommand.Parameters.GetVarValues(ruleCommandParseResult);
    var varValues = runParams.Assignments.TransformVariables(parsedVarValues);

    await runner.Run(runParams.Rule, runParams.WorkingDirectory, varValues);
}
catch (Exception ex)
{
    logger.LogError("{Message}", ex.Message);
    Environment.Exit(1);
}