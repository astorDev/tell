using Tell.Vars.Use.Lexer;

namespace Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Basic()
    {
        var recipe = 
"""
prt:
    echo "Hello, $(NAME)!"
""";

        var tokens = VarUseTokens.Tokenizer.TryTokenize(recipe);
        
        foreach (var token in tokens.Value) Console.WriteLine(token);
    }
}