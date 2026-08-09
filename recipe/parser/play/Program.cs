var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<ParseCommand>();

using var app = builder.Build("A tell.recipe.parser CLI application.");

return app.Run(args);