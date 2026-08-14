var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<CommandNameCommand>();

using var app = builder.Build("A tell.doc.parser CLI application.");

return app.Run(args);