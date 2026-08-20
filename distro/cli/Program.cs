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
    var runParams = gate.GetRunRuleParams(args);

    var allRuleRunCommands = runParams.Doc.Rules.Select(r => new RunRuleCommand(
        new RuleRunParams(r.Value, runParams.Doc, runParams.WorkingDirectory, runParams.Args), runner));
    
    //Console.WriteLine(runParams.ToString());

    var defaultRuleRunCommand = new RunRuleCommand(runParams, runner);
    var tell = new TellCommand(allRuleRunCommands, defaultRuleRunCommand);

    await tell.Parse(args).InvokeAsync();

    // var runCommand = ParseOnlyRunRuleCommand.From(runParams.Rule);
    // var ruleCommandParseResult = runCommand.Parse(runParams.Args);
    // var parsedVarValues = runCommand.VarUseParams.GetVarValues(ruleCommandParseResult);
    // var varValues = runParams.Assignments.TransformVariables(parsedVarValues);

    // await runner.Run(runParams.Rule, runParams.WorkingDirectory, varValues);
}
catch (Exception ex)
{
    logger.LogError("{Message}", ex.Message);
    Environment.Exit(1);
}