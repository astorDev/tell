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

    [TestMethod]
    public void ParsesDependencies()
    {
        var target = Target.Parser.Parse("all: prepare build");
        target.Dependencies.Select(dependency => dependency.Value).ShouldBe(["prepare", "build"]);
    }
}