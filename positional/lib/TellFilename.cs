namespace Tell;

public class TellFilename
{
    public static readonly Option<string> Option = new("--file")
    {
        DefaultValueFactory = (x) => "Makefile"
    };
}