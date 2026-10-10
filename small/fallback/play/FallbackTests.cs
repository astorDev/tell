using Tell;
using Microsoft.Extensions.Logging.Abstractions;

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

    [DataTestMethod]
    [DataRow("cli/examples --file deps.Makefile meet --name=Egor", "-C cli/examples -f deps.Makefile meet NAME=Egor")]
    public void CreatesFallbackArgumentsFromCommandLine(string original, string fallback)
    {
        var root = new RootCommand { 
            WorkingDirectory.ConditionalArgument, 
            TellFilename.Option
        };

        var initialParse = root.Parse(original);
        var fileSystem = TellFileSystem.UncheckedFrom(initialParse);

        root.Add(FallbackCli.TargetArgument);
        root.TreatUnmatchedTokensAsErrors = false;
        var fallbackParse = root.Parse(original);

        var fb = FallbackCli.CreateFallbackFrom(fallbackParse, fileSystem);
        fb.ToMakeArguments().ShouldBe(fallback);
    }

    [TestMethod]
    public void PreservesFilenamePassedToMakeFallback()
    {
        var fallback = new MakeFallback(null, "config/deps.Makefile", "meet");
        fallback.GetMakeArguments().ShouldContain("-f config/deps.Makefile");
    }
}
