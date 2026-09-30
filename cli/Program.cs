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
    var root = new RootCommand("Executes commands defined in the Makefile")
    {
        TellFileSystem.Symbols
    };

    root.AddLoggingCliOptions();

    var initialParse = root.Parse(args);
    var fileSystem = TellFileSystem.From(initialParse);

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

    var parsed = root.ParseWithDefaultCommand(args, context.Makefile.FirstRule.Name);
    return await parsed.InvokeAsync();
});