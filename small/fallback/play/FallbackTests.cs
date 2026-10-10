using Tell;

namespace Playground;

[TestClass]
public class FallbackTests
{
    [DataTestMethod]
    [DataRow("meet --name Bob --verbose", "NAME=Bob")]
    [DataRow("meet --name=Bob --verbose", "NAME=Bob")]
    public void ConvertsUnknownNamedArgumentsToMakeAssignments(string commandLine, string assignment)
    {
        var target = new Argument<string>("target") { Arity = ArgumentArity.ZeroOrOne };
        var command = new RootCommand { target, new Option<bool>("--verbose") };
        command.TreatUnmatchedTokensAsErrors = false;
        var parsed = command.Parse(commandLine);
        parsed.GetValue(target).ShouldBe("meet");

        var variableOptions = MakeVariableOptionCollection.From(parsed.UnmatchedTokens);
        variableOptions.ToMakeArguments().ShouldBe([ assignment ]);
    }

    [TestMethod]
    public void PreservesMakeArgumentBoundaries()
    {
        var fallback = new MakeFallback("work dir", "make file", "meet", ["--jobs=2", "NAME=Egor Tarasov"]);
        fallback.GetMakeArguments().ShouldBe(new[] { "-C work dir", "-f make file", "meet", "--jobs=2", "NAME=Egor Tarasov" });
    }
}
