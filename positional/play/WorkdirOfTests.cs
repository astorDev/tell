using Copaster;

namespace Playground;

[TestClass]
public class WorkdirOfTests
{
    [TestMethod]
    public void Printed()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine(currentDirectory);
    }

    [TestMethod]
    public void GetsMakefile()
    {
        var currentDirectory = new Folder(".");
        var makefile = currentDirectory.File("Makefile");
        makefile.Exists.ShouldBeTrue();
        Console.WriteLine(makefile.Content);
    }
}