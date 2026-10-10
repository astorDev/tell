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
        WorkingDirectoryCli.ConditionalArgument,
        TellFilename.Option,
    };

    root.AddLoggingCliOptions();

    var initialParse = root.Parse(args);
    var workdir = initialParse.GetRequiredValue(WorkingDirectoryCli.ConditionalArgument);
    var filename = initialParse.GetRequiredValue(TellFilename.Option);
    var fileSystem = TellFileSystem.From(workdir, filename);

    if (!MakefileTree.TryParse(fileSystem.File, out var makefileTree, out var error))
    {
        app.Logger.LogWarning("Failed to parse Makefile: {error}. Trying fallback to make", error!.Message);

        root.Add(FallbackCli.TargetArgument);
        root.TreatUnmatchedTokensAsErrors = false;
        root.SetAction(pr => FallbackCli.Action(pr, fileSystem, app.Logger));

        var fallbackParse = root.Parse(args);
        return await fallbackParse.InvokeAsync();
    }

    var makefile = makefileTree!.ToMakefile();

    foreach (var rule in makefile.Rules.Values)
    {
        var ruleCommand = new RuleInfoCommand(rule);
        ruleCommand.AddLoggingCliOptions();

        ruleCommand.MakeRuleCommand(rule, fileSystem.WorkingDir, runner.Run);

        root.Add(ruleCommand);
    }

    root.SetDefaultSubcommand(makefile.FirstRule.Name);

    var parsed = root.Parse(args);
    return await parsed.InvokeAsync();
});