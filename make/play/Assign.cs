namespace Playground;

[TestClass]
public class Assign
{
    [TestMethod]
    public void Basic()
    {
        var makefile = """
NAME ?= Egor
GREETING ?= Servus
""";

        var parser =
            from a in Assignment.Parser.Many()
            select a;

        var assignments = parser.Parse(makefile);

        var existingVariables = new Dictionary<string, string>();

        var transformedVariables = assignments.TransformVariables(existingVariables);

        transformedVariables.Count.ShouldBe(2);
        transformedVariables["NAME"].ShouldBe("Egor");
        transformedVariables["GREETING"].ShouldBe("Servus");
    }
}