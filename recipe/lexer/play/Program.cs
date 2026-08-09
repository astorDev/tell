using Tell.Playground;

var builder = new CliBuilder();

builder.Logging.AddNiceShell();

builder.AddCommand<TokensCommand>();

using var app = builder.Build("A tell.recipe.lexer CLI application.");

return app.Run(args);