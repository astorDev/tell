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
}