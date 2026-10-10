namespace Tell;

public record TellFileSystem(Folder WorkingDir, Copaster.File File)
{
    public TellFileSystem Checked() 
    {
        if (!File.Exists) throw new FileNotFoundException($"'{Path.GetFullPath(File.Path)}' does not exist.");
        return this;
    }
    
    public static TellFileSystem From(ParseResult parsed) => UncheckedFrom(parsed).Checked();
    public static TellFileSystem From(Folder folder, string filename) => UncheckedFrom(folder, filename).Checked();

    public static TellFileSystem UncheckedFrom(Folder folder, string filename)
    {
        var file = folder.File(filename);
        return new TellFileSystem(folder, file);
    }

    public static TellFileSystem UncheckedFrom(ParseResult parsed)
    {
        var folder = parsed.GetRequiredValue(WorkingDirectory.ConditionalArgument);
        var filename = parsed.GetRequiredValue(TellFilename.Option);

        return UncheckedFrom(folder, filename);
    }
}