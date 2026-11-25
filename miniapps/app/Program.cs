using Microsoft.Extensions.Logging;

var builder = new AppBuilder();

builder.Logging.AddNiceShell();

var app = builder.Build();

var file = Path.Combine(AppContext.BaseDirectory, "Makefile");

return await app.RunCliAsync(async (ILogger<Program> logger) =>
{
    var makefileContent = File.ReadAllText(file);
    
    logger.LogInformation("Makefile content:\n {content}", makefileContent);
    logger.LogInformation("Working directory: {cwd}", Directory.GetCurrentDirectory());
    foreach (var arg in args)
    {
        logger.LogInformation("Argument: {arg}", arg);
    }

    throw new NotImplementedException();
});