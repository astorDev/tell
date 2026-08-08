using Superpower;
using Superpower.Tokenizers;

namespace Tell;

public enum MakefileTokenKind
{
}

public static class MakefileLexer
{
    public static readonly Tokenizer<MakefileTokenKind> Tokenizer = 
        new TokenizerBuilder<MakefileTokenKind>()
        .Build();
}
