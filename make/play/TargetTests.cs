using Superpower;

namespace Tell.Playground;

[TestClass]
public class TargetTests
{
    [TestMethod]
    public void Basic()
    {
        var example = "run:";

        var target = Target.Parser.Parse(example);
        Console.WriteLine(target);
        target.Identifier.Value.ShouldBe("run");
    }
}