using Playground;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<CommandNameCommand>();

using var app = builder.Build("A tell.superpower CLI application.");

return app.Run(args);