using Microsoft.Extensions.Logging;

namespace Tell.Playground;

public class TokensCommand : Command
{
    private readonly Argument<string> recipeArgument = new("recipe")
    {
        Description = "The recipe content to tokenize.",
        Arity = ArgumentArity.ExactlyOne
    };

    private readonly ILogger<TokensCommand> logger;

    public TokensCommand(ILogger<TokensCommand> logger) : base("tokens", "Tokenize a recipe file.")
    {
        Add(recipeArgument);
        SetAction(Execute);

        this.logger = logger;
    }

    private void Execute(ParseResult parseResult)
    {
        var recipe = parseResult.GetRequiredValue(recipeArgument);

        var tokens = RecipeTokens.Tokenizer.Tokenize(recipe);
        var entryNames = tokens.Select(t => t.ToString()).ToList();
        
        entryNames.ForEach(Console.WriteLine);
    }
}