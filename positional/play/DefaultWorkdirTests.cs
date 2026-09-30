namespace Playground;

[TestClass]
public class DefaultWorkdirTests
{
    [TestMethod]
    public void Empty()
    {
        var result = TellRun.On("");
        Console.WriteLine(result);
        result.Replacements.ShouldContainKeyAndValue("GREETING", "Hello");
        result.Replacements.ShouldContainKeyAndValue("NAME", "World");
    }

    [TestMethod]
    public void Greet()
    {
        var result = TellRun.On("greet");
        Console.WriteLine(result);
        result.Replacements.ShouldContainKeyAndValue("GREETING", "Hello");
        result.Replacements.ShouldContainKeyAndValue("NAME", "World");
    }

    [TestMethod]
    public void ServusEgorGreet()
    {
        var result = TellRun.On("greet Servus Egor");
        Console.WriteLine(result);
        result.Replacements.ShouldContainKeyAndValue("GREETING", "Servus");
        result.Replacements.ShouldContainKeyAndValue("NAME", "Egor");
    }

    [TestMethod]
    public void ServusEgorNoCommand()
    {
        var result = TellRun.On("Servus Egor");
        Console.WriteLine(result);
        result.Replacements.ShouldContainKeyAndValue("GREETING", "Servus");
        result.Replacements.ShouldContainKeyAndValue("NAME", "Egor");
    }
}