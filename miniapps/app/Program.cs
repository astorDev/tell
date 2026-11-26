using Copaster;
using Microsoft.Extensions.DependencyInjection;
using Tell;

var builder = new AppBuilder();

builder.Configuration.AddLoggingCliOptions(args);
builder.Logging.AddNiceShell();

builder.Services.AddSingleton<RuleRunner>();
builder.Services.AddSingleton<RecipeRunner>();

var app = builder.Build();

return await app.RunCliAsync(async (RuleRunner runner) =>
{
    var root = new Nishe.RootCommand("Executes commands defined in the bundled Makefile")
    {
        WorkingDirectoryCli.ConditionalArgument,
    };

    root.AddLoggingCliOptions();

    var workdir = root.Parse(args).GetRequiredValue(WorkingDirectoryCli.ConditionalArgument);
    var file = new Folder(AppContext.BaseDirectory).File("Makefile");
    var makefile = Makefile.From(file);

    foreach (var rule in makefile.Rules.Values)
    {
        var ruleCommand = new RuleInfoCommand(rule);
        ruleCommand.AddLoggingCliOptions();
        ruleCommand.MakeRuleCommand(rule, workdir, runner.Run);
        root.Add(ruleCommand);
    }

    root.SetDefaultSubcommand(makefile.FirstRule.Name);

    return await root.Parse(args).InvokeAsync();
});