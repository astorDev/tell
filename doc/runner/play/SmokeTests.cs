using Microsoft.Extensions.Logging;

namespace Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Parse()
    {
        var exampleArgs = "--file example.Makefile greeting Servus --name Egor";
        var startup = new Startup(new LoggerFactory().CreateLogger<Startup>());
        
        var parseResult = startup.Parse(exampleArgs.Split(' '));
        var runParams = startup.Interpret(parseResult);

        var ruleParams = String.Join(" ", runParams.Args);

        Console.WriteLine("Remaining Args: " + ruleParams);
        Console.WriteLine(runParams.Rule);
        Console.WriteLine("Working Directory: " + runParams.WorkingDirectory);
    }
}