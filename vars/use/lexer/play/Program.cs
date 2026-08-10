var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<TokensCommand>();

using var app = builder.Build("A tell.vars.use.lexer CLI application.");

return app.Run(args);