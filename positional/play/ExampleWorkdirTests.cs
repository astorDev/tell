namespace Playground;

[TestClass]
public class ExampleWorkdirTests
{
    [TestMethod]
    public void Empty()
    {
        var result = TellRun.On("example");
        Console.WriteLine(result);
        result.Replacements.ShouldContainKeyAndValue("GREETING", "Hello");
        result.Replacements.ShouldContainKeyAndValue("NAME", "World");
    }

    [TestMethod]
    public void ServusEgorGreet()
    {
        var result = TellRun.On("example greet Servus Egor");
        Console.WriteLine(result);
        result.Replacements.ShouldContainKeyAndValue("GREETING", "Servus");
        result.Replacements.ShouldContainKeyAndValue("NAME", "Egor");
    }

    [TestMethod]
    public void ServusEgorNoCommand()
    {
        var result = TellRun.On("example Servus Egor");
        Console.WriteLine(result);
        result.Replacements.ShouldContainKeyAndValue("GREETING", "Servus");
        result.Replacements.ShouldContainKeyAndValue("NAME", "Egor");
    }
}