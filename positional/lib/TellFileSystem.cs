namespace Tell;

public record TellFileSystem(Folder WorkingDir, Copaster.File File)
{
    public static TellFileSystem From(ParseResult parsed)
    {
        var folder = parsed.GetRequiredValue(WorkingDirectory.ConditionalArgument);
        var filename = parsed.GetRequiredValue(TellFilename.Option);
        var file = folder.File(filename);
        if (!file.Exists) throw new FileNotFoundException($"'{Path.GetFullPath(file.Path)}' does not exist.");

        return new TellFileSystem(folder, file);
    }

    public static TellFileSystem From(Folder folder, string filename)
    {
        var file = folder.File(filename);
        if (!file.Exists) throw new FileNotFoundException($"'{Path.GetFullPath(file.Path)}' does not exist.");

        return new TellFileSystem(folder, file);
    }
}