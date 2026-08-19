global using Superpower;
global using Tell;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<RunCommand>();

using var app = builder.Build("A tell.makefile CLI application.");

var makefile = """
NAME ?= Egor
GREETING ?= Servus
""";

throw new NotImplementedException("Implement the CLI application logic here.");

return app.Run(args);