namespace Tell.Playground;

[TestClass]
public class MakefileEndToEndTests
{
    private const string MakefileContent =
"""
NAME = Egor
GREETING = Hello

hello:
	echo $(GREETING) $(NAME)

all: prepare build
	echo all

prepare: shared
	echo prepare $(NAME)

build: shared
	echo build $(NAME)

shared:
	echo shared $(NAME)
""";

    [DataTestMethod]
    [DataRow("hello", new string[] { "echo Hello Egor" })]
    [DataRow("all", new string[] { "echo shared Egor", "echo prepare Egor", "echo build Egor", "echo all" })]
    [DataRow("prepare", new string[] { "echo shared Egor", "echo prepare Egor" })]
    [DataRow("build", new string[] { "echo shared Egor", "echo build Egor" })]
    [DataRow("shared", new string[] { "echo shared Egor" })]
    public void BuildsCommandsForTarget(string targetName, string[] expectedCommands)
    {
        var makefile = Makefile.Parse(MakefileContent);
        var rule = makefile.GetRule(targetName);
        var variables = makefile.Assignments.TransformVariables(new Dictionary<string, string>());
        var commands = RecipeCommands.From(rule, variables);
        commands.ShouldBe(expectedCommands);
    }
}
