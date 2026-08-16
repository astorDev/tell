var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<TellCommand>();

using var app = builder.Build("A tell.positional CLI application.");

return app.Run(args);