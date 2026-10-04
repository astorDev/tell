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

    TellContext context;

    try
    {
        context = fileSystem.ToContext();
    }
    catch (Superpower.ParseException ex)
    {
        app.Logger.LogWarning("Failed to parse Makefile: {Message}. Trying fallback to make", ex.Message);

        app.Logger.LogTrace("Fallback params: Folder: {Folder}, File: {File}", fileSystem.WorkingDir.Path, fileSystem.File.Path);

        throw new NotImplementedException("Fallback to make is not implemented yet.");
    }

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