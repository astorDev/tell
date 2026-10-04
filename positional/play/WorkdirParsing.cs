using Copaster;
using Tell;

namespace Playground;

[TestClass]
public class FilesystemTests
{
    [TestMethod]
    [DataRow("", ".", "Makefile")]
    [DataRow("examples", "examples", "Makefile")]
    [DataRow("examples --file CustomMakefile", "examples", "CustomMakefile")]
    [DataRow("--file CustomMakefile", ".", "CustomMakefile")]
    [DataRow("--file CustomMakefile examples", "examples", "CustomMakefile")]
    public void OnEmpty(string input, string expectedFolder, string expectedFilename)
    {
        var workDirArg = WorkingDirectory.ConditionalArgument;

        var command = new Nishe.RootCommand()
        {
            workDirArg,
            TellFilename.Option
        };

        var parseResult = command.Parse(input);
        var result = new
        {
            workDir = parseResult.GetValue(workDirArg),
            filename = parseResult.GetValue(TellFilename.Option)
        };

        Console.WriteLine(result);

        result.workDir.ShouldBe(new Folder(expectedFolder));
        result.filename.ShouldBe(expectedFilename);
    }
}