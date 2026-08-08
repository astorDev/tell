namespace Tell;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Main()
    {
        var exampleMakefile = """
            
        """;

        var tokens = MakefileLexer.Tokenizer.Tokenize(exampleMakefile);

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}