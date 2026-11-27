var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddAsRootCommand<MiniappCommand>();

using var app = builder.Build("A tell.miniapps CLI application.");

return app.Run(args);