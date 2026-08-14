using Superpower;
using Superpower.Tokenizers;

namespace Tell.Playground;

[TestClass]
public class Fragments
{
    [TestMethod]
    public void Simple()
    {
        var example = "Hello, $(NAME)!";
        var tokenizer = new TokenizerBuilder<string>().MatchRecipeFragment().Build();
        var tokens = tokenizer.Tokenize(example);

        foreach (var token in tokens) {
            Console.WriteLine(token);
            var fragment = RecipeFragment.Parser.Parse(token.ToStringValue());
            Console.WriteLine(fragment);
        }
    }

    [TestMethod]
    public void Escaped()
    {
        var example = "Hello, $$(NAME)!";
        var tokenizer = new TokenizerBuilder<string>().MatchRecipeFragment().Build();
        var tokens = tokenizer.Tokenize(example);

        foreach (var token in tokens) {
            Console.WriteLine(token);
            var fragment = RecipeFragment.Parser.Parse(token.ToStringValue());
            Console.WriteLine(fragment);
        }
    }

    [TestMethod]
    public void Command()
    {
        var example = "Hello, $(NAME)! Here is some escaping: $$(NAME) and $$(ESCAPED).";
        var tokenizer = new TokenizerBuilder<string>().MatchRecipeFragment().Build();
        var tokens = tokenizer.Tokenize(example);

        var variables = new Dictionary<string, string> { { "NAME", "World" } };
        var fragments = tokens.Select(token => RecipeFragment.Parser.Parse(token.ToStringValue()));
        var commandFragments = fragments.Select(fragment => fragment.ToCommandFragment(variables));
        var command = string.Concat(commandFragments);
        Console.WriteLine(command);
    }
}