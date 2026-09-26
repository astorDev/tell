using System.CommandLine.Parsing;
using Copaster;
using Microsoft.Extensions.Logging;
using Tell;

namespace Playground;

public class GateCommand : Command
{
    private readonly ILogger<GateCommand> logger;

    public GateCommand(ILogger<GateCommand> logger) : base("gate", "Checks the Makefile ")
    {
        Add(MakefileName.Option);
        Add(WorkingDirectory.Argument);

        // this.TreatUnmatchedTokensAsErrors = false;
        // this.logger = logger;
    }

    public EffectiveCommand GetEffectiveCommand(ParseResult parseResult)
    {
        var workingDirectory = parseResult.GetRequiredValue(WorkingDirectory.Argument);
        return new EffectiveCommand(workingDirectory.Makefile);
    }
}

public class EffectiveCommand : RootCommand
{
    public EffectiveCommand(Makefile makefile)
    {
        Add(MakefileName.Option);
        Add(WorkingDirectory.Argument);

        var ruleCommands = RuleCommand.From(makefile);
        foreach (var ruleCommand in ruleCommands)
        {
            Add(ruleCommand);
        }
    }
}

public class RuleCommand : Command
{
    public const string DescriptionPrefix = "Executes: ";
    public static readonly string DescriptionNextLinePrefix = new(' ', DescriptionPrefix.Length);
    public static readonly string DescriptionSeparator = $"{Environment.NewLine}{DescriptionNextLinePrefix}";

    public static string DescriptionFrom(IEnumerable<Recipe> recipes) 
    {
        var recipeLines = recipes.Select(r => r.ToUnresolvedCommandString());
        return $"{DescriptionPrefix}{String.Join(DescriptionSeparator, recipeLines)}";
    }

    public static string NameFrom(Rule rule)
    {
        return rule.Target.Identifier.Value;
    }

    private readonly List<string> placeholderReplacementSymbolKeys = [];

    public RuleCommand(Rule rule, IEnumerable<Assignment> assignments) : base(NameFrom(rule), DescriptionFrom(rule.Recipes))
    {
        var placeholders = rule.Placeholders
            .Select(p => p.Identifier.Value)
            .Select(p => {
                var assignment = assignments.FirstOrDefault(a => a.Target.Value == p);
                if (assignment == null)
                {
                    return PlaceholderReplacementCandidate.From(p, [ RecipeFragment.FromLiteral("") ]);
                }

                return PlaceholderReplacementCandidate.From(p, assignment.ValueFragments);
            })
            .Select(PlaceholderReplacement.OptionAndArg)
            .ToList();
        
        foreach (var placeholder in placeholders)
        {
            Add(placeholder.Option);
            Add(placeholder.Argument);

            placeholderReplacementSymbolKeys.Add(placeholder.Option.Name);
            placeholderReplacementSymbolKeys.Add(placeholder.Argument.Name);
        }

        SetAction(Execute);
    }

    public static IEnumerable<RuleCommand> From(Makefile makefile)
    {
        foreach (var rule in makefile.Rules.Values)
        {
            yield return new RuleCommand(rule, makefile.Assignments);
        }
    }

    public void Execute(ParseResult parseResult)
    {
        Console.WriteLine($"Matched rule '{this.Name}'");

        var replacementDictionary = placeholderReplacementSymbolKeys
            .Select(k => parseResult.GetRequiredValue<PlaceholderReplacement>(k))
            .Where(p => p != null)
            .Select(p => p!)
            .OrderBy(p => p.IsDefault).DistinctBy(p => p.Identifier)
            .ToDictionary(p => p.Identifier, p => p.Value);

        Console.WriteLine("Replacement dictionary:");
        var variableValues = new Dictionary<string, string>();
        foreach (var kvp in replacementDictionary)
        {
            variableValues[kvp.Key] = kvp.Value.ToCommandString(variableValues);
            Console.WriteLine($"  {kvp.Key} = {variableValues[kvp.Key]}");
        }
    }
}

public record PlaceholderReplacement(string Identifier, RecipeFragment[] Value, bool IsDefault = false)
{
    public static Option<PlaceholderReplacement> Option(PlaceholderReplacementCandidate candidate) => new($"--{candidate.KebabedIdentifier}")
    {
        Description = $"Replacement for '{candidate.OriginalIdentifier}'.",
        CustomParser = (result) => 
        {
            var value = result.Tokens[0].Value;
            return new PlaceholderReplacement(candidate.OriginalIdentifier, [ RecipeFragment.FromLiteral(value) ]);
        },
        DefaultValueFactory = (result) => new PlaceholderReplacement(candidate.OriginalIdentifier, candidate.DefaultValue, IsDefault: true)
    };

    public static Argument<PlaceholderReplacement> Argument(PlaceholderReplacementCandidate candidate) => new($"{candidate.KebabedIdentifier}")
    {
        Description = $"Replacement for '{candidate.OriginalIdentifier}'.",
        CustomParser = (result) => 
        {
            var value = result.Tokens[0].Value;
            return new PlaceholderReplacement(candidate.OriginalIdentifier, [ RecipeFragment.FromLiteral(value) ]);
        },
        DefaultValueFactory = (result) => new PlaceholderReplacement(candidate.OriginalIdentifier, candidate.DefaultValue, IsDefault: true)
    };

    public static (Option<PlaceholderReplacement> Option, Argument<PlaceholderReplacement> Argument) OptionAndArg(PlaceholderReplacementCandidate candidate) => (Option(candidate), Argument(candidate));

    override public string ToString()
    {
        var commandString = Value.ToUnresolvedCommandString();
        return String.IsNullOrEmpty(commandString) ? "\"\"" : commandString;
    }
}

public record PlaceholderReplacementCandidate(string OriginalIdentifier, string KebabedIdentifier, RecipeFragment[] DefaultValue)
{
    public static PlaceholderReplacementCandidate From(string originalIdentifier, RecipeFragment[] defaultValue) => new(originalIdentifier, Kebab.Of(originalIdentifier), defaultValue);
}


public record WorkingDirectory(Folder Folder, Makefile Makefile)
{
    public static readonly Argument<WorkingDirectory> Argument = new("folder")
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

    public static WorkingDirectory? Search(Folder folder, ArgumentResult result)
    {
        if (result.Parent == null)
        {
            result.AddError("Parent command result is missing.");
            return null;
        }

        var makefileName = result.Parent!.GetRequiredValue(MakefileName.Option);
        var makefileInSystem = folder.File(makefileName);

        if (!makefileInSystem.Exists)
        {
            result.AddError($"'{Path.GetFullPath(makefileInSystem.Path)}' does not exist.");
            return null;
        }

        var makefile = Makefile.Load(makefileInSystem.Path);
        return new WorkingDirectory(folder, makefile);
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

public class MakefileName
{
    public static readonly Option<string> Option = new("--file")
    {
        DefaultValueFactory = (x) => "Makefile"
    };
}