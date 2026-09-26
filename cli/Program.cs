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

var runner = app.Services.GetRequiredService<RuleRunner>();

try
{
    var tell = new RootCommand("Executes commands defined in the Makefile");
    tell.AddLoggingCliOptions();
    tell.AddTellContextSymbols();

    ParseResult initialPass;

    try
    {
        initialPass = tell.Parse(args);
    }
    catch (Superpower.ParseException ex)
    {
        app.Logger.LogWarning("Failed to parse Makefile: {Message}. Trying fallback to make.", ex.Message);
        throw new NotImplementedException("Fallback to make is not implemented yet.");
    }

    var context = initialPass.GetRequiredValue(TellContext.Argument);
    var makefile = context.Makefile;
    var firstRuleSymbols = ReplacementSymbols.AllFor(makefile.FirstRule, makefile.Assignments);
    tell.Add(firstRuleSymbols);
    tell.Description = tell.Description + $"\nRuns the first rule ({makefile.FirstRule.Name}) by default i.e.\n" + $"{RuleInfoCommand.DescriptionFrom(makefile.FirstRule.Recipes)}";
    tell.SetAction(async parseResult =>
    {
        var replacements = firstRuleSymbols.GetMaterializedReplacements(parseResult);
        await runner.Run(makefile.FirstRule.Recipes, context.WorkingDirectory.Path, replacements);
    });
    
    foreach (var rule in makefile.Rules.Values)
    {
        var ruleSymbols = ReplacementSymbols.AllFor(rule, makefile.Assignments);
        var ruleInfoCommand = new RuleInfoCommand(rule)
        {
            ruleSymbols
        };
        ruleInfoCommand.AddLoggingCliOptions();

        ruleInfoCommand.SetAction(async parseResult =>
        {
            var replacements = ruleSymbols.GetMaterializedReplacements(parseResult);
            await runner.Run(rule.Recipes, context.WorkingDirectory.Path, replacements);
        });

        tell.Add(ruleInfoCommand);
    }

    return await tell.Parse(args).InvokeAsync();
}
catch (Exception ex)
{
    app.Logger.LogError("{Message}", ex.Message);
    return 1;
}