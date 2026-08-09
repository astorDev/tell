using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tell;

var builder = new CliBuilder();

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNiceShell();

builder.Services.AddSingleton<RecipeRunner>();

builder.AddCommand<RunCommand>();

using var app = builder.Build("A tell.recipe.runner CLI application.");

return app.Run(args);