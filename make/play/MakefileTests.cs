namespace Playground;

[TestClass]
public class MakefileTests
{
    [TestMethod]
    public void TreeFinalizesDependencyOrder()
    {
        var tree = MakefileTree.Parser.Parse("""
build: prepare test
    echo build
prepare:
    echo prepare
test: prepare
    echo test
""");

        var makefile = tree.ToMakefile();
        var build = makefile.GetRule("build");
        var orderedRules = build.RulesFromDependenciesAndBody.Select(rule => rule.Name);

        build.Name.ShouldBe(build.Name);
        build.RecipesFromDependenciesAndBody.Count.ShouldBe(3);
        build.Dependencies.Select(rule => rule.Name).ShouldBe(["prepare", "test"]);
        orderedRules.ShouldBe(["prepare", "test", "build"]);
    }
}
