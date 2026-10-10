using Tell;

namespace Playground;

[TestClass]
public class MakeVariableArgumentsTests
{
    [DataTestMethod]
    [DataRow("--name Bob", "NAME=Bob")]
    [DataRow("--name=Bob", "NAME=Bob")]
    [DataRow("--first-name Bob --age=42", "FIRST-NAME=Bob,AGE=42")]
    [DataRow("--name Bob=Smith", "NAME=Bob=Smith")]
    [DataRow("--name=Bob=Smith", "NAME=Bob=Smith")]
    public void ConvertsNamedArguments(string input, string expected)
    {
        var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        MakeVariableOptionCollection.From(tokens);
        MakeVariableOptionCollection.From(tokens).ToMakeArguments().ShouldBe(expected.Split(','));
    }

    [TestMethod]
    public void PreservesEmptyAndSpaceContainingValues()
    {
        var collection = MakeVariableOptionCollection.From(["--name", "Egor Tarasov", "--empty="]);
        collection.ToMakeArguments().ShouldBe([ "NAME=Egor Tarasov", "EMPTY=" ]);
    }

    [TestMethod]
    public void ReturnsNoAssignmentsForNoArguments() =>
        MakeVariableOptionCollection.From([]).ToMakeArguments().ShouldBeEmpty();

    [DataTestMethod]
    [DataRow("Bob")]
    [DataRow("--name")]
    [DataRow("--name Bob extra")]
    [DataRow("--=Bob")]
    public void RejectsInvalidNamedArguments(string input)
    {
        var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Assert.ThrowsExactly<NotSupportedException>(() => MakeVariableOptionCollection.From(tokens).Items.ToArray());
    }
}
