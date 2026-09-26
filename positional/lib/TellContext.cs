using Microsoft.Extensions.Logging;

namespace Tell;

public static class TellContextExtensions
{
    public static void AddTellContextSymbols(this Command command)
    {
        command.Add(TellFilename.Option);
        command.Add(TellContext.Argument);
    }
}

public record TellContext(Folder Folder, Makefile Makefile)
{
    public class Command : System.CommandLine.Command
    {
        public Command() : base("gate", "Checks the Makefile ")
        {
            Add(TellFilename.Option);
            Add(Argument);
        }

        public TellContext GetContextFrom(string[] args)
        {
            var parseResult = Parse(args);
            return GetContextFrom(parseResult);
        }

        public TellContext GetContextFrom(ParseResult parseResult) => parseResult.GetRequiredValue(Argument);
    }

    public static readonly Argument<TellContext> Argument = new("folder")
    {
        CustomParser = result =>
        {
            var folder = ExistingFolder(result);
            return Search(folder, result);
        },
        Description = "The working directory containing the Makefile.",
        DefaultValueFactory = (result) => 
        {
            var defaultFolder = new Folder(".");
            return Search(defaultFolder, result);
        },
        Arity = ArgumentArity.ZeroOrOne
    };

    public static TellContext? Search(Folder folder, ArgumentResult result)
    {
        if (result.Parent == null)
        {
            result.AddError("Parent command result is missing.");
            return null;
        }

        var makefileName = result.Parent!.GetRequiredValue(TellFilename.Option);
        var makefileInSystem = folder.File(makefileName);

        if (!makefileInSystem.Exists)
        {
            result.AddError($"'{Path.GetFullPath(makefileInSystem.Path)}' does not exist.");
            return null;
        }

        var makefile = Makefile.Load(makefileInSystem.Path);
        return new TellContext(folder, makefile);
    }
    
    public static Folder ExistingFolder(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            return new Folder(".");
        }

        var token = result.Tokens[0].Value;
        var folder = new Folder(token);
        if (folder.Exists)
        {
            return folder;
        }

        return new Folder(".");
    }
}
