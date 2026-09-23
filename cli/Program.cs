using Hesive;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tell;

var builder = new AppBuilder();

builder.Configuration.AddLoggingCliOptions(args);
builder.Logging.AddNiceShell();

builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<RecipeRunner>();

builder.Services.AddSingleton<EntryGate>();

var app = builder.Build();

var gate = app.ServiceProvider.GetRequiredService<EntryGate>();
var logger = app.ServiceProvider.GetRequiredService<ILogger<Program>>();
var runner = app.ServiceProvider.GetRequiredService<RuleRunner>();

try
{
    var gateParseResult = gate.Parse(args);
    var runParams = gate.RunRuleParamsFrom(gateParseResult);

    var allRuleRunCommands = runParams.Doc.Rules.Select(r => new RunRuleCommand(
        new RuleRunParams(r.Value, runParams.Doc, runParams.WorkingDirectory, runParams.Args), runner));

    var defaultRuleRunCommand = new RunRuleCommand(runParams, runner);

    var tell = new TellCommand(allRuleRunCommands, defaultRuleRunCommand);
    tell.AddLoggingCliOptions();

    logger.LogDebug("Executing tell command with original args: {Args}", runParams.Args);

    return await tell.Parse(args).InvokeAsync();
}
catch (Exception ex)
{
    logger.LogError("{Message}", ex.Message);
    return 1;
}