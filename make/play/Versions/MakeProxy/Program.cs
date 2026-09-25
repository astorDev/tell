global using Tell;
global using Superpower;
using System.Diagnostics;

var proxy = new ProcessStartInfo("make")
{
    Arguments = "-f example.Makefile NAME=John"
};

var result = await proxy.Run();
return result.ExitCode;