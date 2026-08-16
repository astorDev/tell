using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tell;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<TellCommand>();

using var app = builder.Build("A tell.positional CLI application.");

var tell = app.Services.GetRequiredService<TellCommand>();
var logger = app.Services.GetRequiredService<ILogger<TellCommand>>();

RuleRunParams runRuleParams;

try
{
    runRuleParams = tell.GetRunRuleParams(args);    
}
catch (Exception ex)
{
    logger.LogError(ex, "Error while parsing arguments.");
    return 1;
}

logger.LogInformation("Working directory: {WorkingDirectory}", runRuleParams.WorkingDirectory);
logger.LogInformation("Target: {Target}", runRuleParams.Rule.Name);
logger.LogInformation("Arguments: {Arguments}", string.Join(" ", runRuleParams.Args));

return 0;