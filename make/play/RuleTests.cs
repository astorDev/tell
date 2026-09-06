using Superpower;
using Tell;

namespace Playground;

[TestClass]
public class RuleTests
{
    [TestMethod]
    public void Tokens()
    {
        var example = 
"""
run:
    echo "Hello, $(NAME)"
    dotnet run --environment $(ENV)
    echo "Done. Used: $$(NAME), $$(ENV)"
""";

        var tokens = Rule.Tokenizer.Tokenize(example);
        foreach (var token in tokens) Console.WriteLine(token);
    }

    [TestMethod]
    public void Parse()
    {
        var example = 
"""
run:
    echo "Hello, $(NAME)"
    dotnet run --environment $(ENV)
    echo "Done. Used: $$(NAME), $$(ENV)"
""";

        var rule = Rule.Parser.Parse(example);
        Console.WriteLine(rule);
        foreach (var recipe in rule.Recipes)
        {
            Console.WriteLine(recipe);
            foreach (var fragment in recipe.Fragments)
            {
                Console.WriteLine(fragment);
            }
        }
    }
}