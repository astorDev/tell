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

    TellContext tellContext;

    try
    {
        tellContext = fileSystem.ToContext();
    }
    catch (Superpower.ParseException ex)
    {
        app.Logger.LogWarning("Failed to parse Makefile: {Message}. Trying fallback to make", ex.Message);

        app.Logger.LogTrace("Fallback params: Folder: {Folder}, File: {File}", fileSystem.WorkingDir.Path, fileSystem.File.Path);

        throw new NotImplementedException("Fallback to make is not implemented yet.");
    }

    root.HandleTellRunning(tellContext, runner);

    return await root.Parse(args).InvokeAsync();
});