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
var ruleRunner = app.Services.GetRequiredService<RuleRunner>();

try
{
    var ruleRunParams = tell.GetRunRuleParams(args);

    var ruleRunCommand = RunRuleCommand.From(
        ruleRunParams.Rule,
        ruleRunner,
        ruleRunParams.WorkingDirectory
    );

    await ruleRunCommand.Run(ruleRunParams.Args);
}
catch (Exception ex)
{
    //logger.LogError(ex, "An error occurred while running the command.");
    logger.LogError(ex.Message);
    Environment.Exit(1);
}