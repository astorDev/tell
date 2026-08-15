using Microsoft.Extensions.Logging;

namespace Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Parse()
    {
        var exampleArgs = "--file example.Makefile Servus --name Egor";
        var startup = new Startup(new LoggerFactory().CreateLogger<Startup>());
        
        var parseResult = startup.Parse(exampleArgs.Split(' '));
        parseResult.ShouldNotBeNull();
    }
}