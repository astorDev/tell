using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tell;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<RecipeRunner>();

builder.AddCommand<EntryGate>();

using var app = builder.Build("A tell CLI application.");

var gate = app.Services.GetRequiredService<EntryGate>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
var runner = app.Services.GetRequiredService<RuleRunner>();

try
{
    var gateParseResult = gate.Parse(args);
    var runParams = gate.RunRuleParamsFrom(gateParseResult);

    var allRuleRunCommands = runParams.Doc.Rules.Select(r => new RunRuleCommand(
        new RuleRunParams(r.Value, runParams.Doc, runParams.WorkingDirectory, runParams.Args), runner));

    var defaultRuleRunCommand = new RunRuleCommand(runParams, runner);

    Command tell = gateParseResult.Action is not null
        ? new InfoTellCommand(allRuleRunCommands, defaultRuleRunCommand)
        : new EffectiveTellCommand(allRuleRunCommands, defaultRuleRunCommand);

    logger.LogDebug("Executing tell command with args: {Args}", runParams.Args);

    await tell.Parse(runParams.Args).InvokeAsync();
}
catch (Exception ex)
{
    logger.LogError("{Message}", ex.Message);
    Environment.Exit(1);
}