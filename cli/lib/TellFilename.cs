namespace Tell;

public class TellFilename
{
    public static readonly Option<string> Option = new("--file")
    {
        Description = "The file to read commands from",
        DefaultValueFactory = (x) => "Makefile"
    };
}