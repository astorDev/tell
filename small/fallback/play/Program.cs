var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<RunCommand>();

using var app = builder.Build("A tell.fallback CLI application.");

return app.Run(args);