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

    var fallbackParams = new MakeFallbackParams(
        fileSystem.WorkingDir.Path,
        null,
        filename
    );

    app.Logger.LogTrace("Fallback params: {params}", fallbackParams);
    app.Logger.LogDebug("Attempting to fallback to make with arguments: {args}", fallbackParams.ToMakeArguments());

    throw new NotImplementedException("Fallback to make is not implemented yet.");
}

var context = new TellContext(fileSystem.WorkingDir, makefile!);
throw new InvalidOperationException("Parsing error handling was expected, got context: " + context);