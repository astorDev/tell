using Hesive;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tell;

var builder = new AppBuilder();

builder.Configuration.AddLoggingCliOptions(args);
builder.Logging.AddNiceShell();

builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<RecipeRunner>();

var app = builder.Build();

return await app.RunCliAsync(async (RuleRunner runner) =>
{
    var root = new Nishe.RootCommand("Executes commands defined in the Makefile")
    {
        WorkingDirectory.ConditionalArgument,
        TellFilename.Option,
    };

    root.AddLoggingCliOptions();

    var initialParse = root.Parse(args);
    var workdir = initialParse.GetRequiredValue(WorkingDirectory.ConditionalArgument);
    var filename = initialParse.GetRequiredValue(TellFilename.Option);
    var fileSystem = TellFileSystem.From(workdir, filename);

    if (!Makefile.TryParse(fileSystem.File, out var makefile, out var error))
    {
        app.Logger.LogWarning("Failed to parse Makefile: {error}. Trying fallback to make", error!.Message);

        root.Add(FallbackCli.TargetArgument);
        root.TreatUnmatchedTokensAsErrors = false;
        root.SetAction(pr => FallbackCli.Action(pr, fileSystem, app.Logger));

        var fallbackParse = root.Parse(args);
        return await fallbackParse.InvokeAsync();
    }

    var context = new TellContext(fileSystem.WorkingDir, makefile!);

    foreach (var rule in context.Makefile.Rules.Values)
    {
        var ruleCommand = new RuleInfoCommand(rule);
        ruleCommand.AddLoggingCliOptions();

        ruleCommand.MakeRuleCommand(rule, context, runner.Run);

        root.Add(ruleCommand);
    }

    root.SetDefaultSubcommand(context.Makefile.FirstRule.Name);

    var parsed = root.Parse(args);
    return await parsed.InvokeAsync();
});