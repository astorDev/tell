using Superpower;
using Tell;

namespace Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Tokens()
    {
        var example = """
        run:
            echo "Hello, $(NAME)"
            dotnet run --environment $(ENV)
            echo "Done. Used: $$(NAME), $$(ENV)"
        """;

        var tokens = Rule.Tokenizer.Tokenize(example);
        foreach (var token in tokens) Console.WriteLine(token);
    }
}