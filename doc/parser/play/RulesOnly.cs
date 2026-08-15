using Superpower;

namespace Playground;

[TestClass]
public class RulesOnly
{
    [TestMethod]
    public void Multi()
    {
        var example = 
"""
run:
    dotnet run --environment=$(ENV)

play:
    apply version $(VERSION)
    make run
""";

        var doc = Tell.Doc.Parser.Parse(example);

        Console.WriteLine($"Doc: {doc.Fragments.Count} fragments");
        
        foreach (var fragment in doc.Fragments)
        {
            Console.WriteLine(fragment);

            if (fragment.Rule is not null)
            {
                Console.WriteLine($"\t{fragment.Rule.Target}");
                foreach (var recipe in fragment.Rule.Recipes)
                {
                    Console.WriteLine($"\t\t{recipe}");
                    foreach (var recipeFragment in recipe.Fragments)
                    {
                        Console.WriteLine($"\t\t\t{recipeFragment}");
                    }
                }
            }
        }
        
    }
}