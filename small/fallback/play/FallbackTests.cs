using Microsoft.Extensions.Logging;
using Nishe;
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
        var command = new Nishe.RootCommand("FallbackTest -> ConvertsUnknownNamedArgumentsToMakeAssignments") { target, new Option<bool>("--verbose") };
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
    [DataRow("cli/app/examples --file deps.Makefile meet --name=Egor", "-C cli/app/examples -f deps.Makefile meet NAME=Egor")]
    [DataRow("cli/app --file examples/deps.Makefile meet --name=Egor", "-C cli/app -f examples/deps.Makefile meet NAME=Egor")]
    [DataRow("--file cli/app/examples/deps.Makefile meet --name=Egor", "-C . -f cli/app/examples/deps.Makefile meet NAME=Egor")]
    public void CreatesFallbackArgumentsFromCommandLine(string original, string fallback)
    {
        var root = new Nishe.RootCommand("FallbackTest -> CreatesFallbackArgumentsFromCommandLine")
        {
            WorkingDirectoryCli.CreateConditionalArgument(folder => folder.Path.Contains("cli")),
            TellFilename.Option,
            FallbackCli.TargetArgument
        };

        root.TreatUnmatchedTokensAsErrors = false;

        var parsed = root.Parse(original);
        var fileSystem = TellFileSystem.UncheckedFrom(parsed);

        var actualFallback = FallbackCli.CreateFallbackFrom(parsed, fileSystem, TestLogger.Default);
        actualFallback.ToMakeArguments().ShouldBe(fallback);
    }

    [TestMethod]
    public void PreservesFilenamePassedToMakeFallback()
    {
        var fallback = new MakeFallback(null, "config/deps.Makefile", "meet");
        fallback.GetMakeArguments().ShouldContain("-f config/deps.Makefile");
    }
}