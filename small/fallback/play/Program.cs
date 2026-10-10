using Hesive;
using Microsoft.Extensions.Logging;
using Tell;

var builder = new AppBuilder();

builder.Configuration.AddLoggingCliOptions(args);
builder.Logging.AddNiceShell();

var app = builder.Build();

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
throw new InvalidOperationException("Parsing error handling was expected, got context: " + context);