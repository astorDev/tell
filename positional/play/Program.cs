using Hesive;
using Microsoft.Extensions.DependencyInjection;

var builder = new AppBuilder();

builder.Logging.AddNiceShell();

builder.Services.AddSingleton<GateCommand>();

var app = builder.Build();

var gateCommand = app.ServiceProvider.GetRequiredService<GateCommand>();
var gateParseResult = gateCommand.Parse(args);
var effectiveCommand = gateCommand.GetEffectiveCommand(gateParseResult);

await effectiveCommand.Parse(args).InvokeAsync();