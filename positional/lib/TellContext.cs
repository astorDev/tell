namespace Tell;

public record TellContext(Folder WorkingDirectory, Makefile Makefile);

public static class TellContextExtensions
{
    public static TellContext ToContext(this TellFileSystem fileSystem)
    {
        var makefile = Makefile.Load(fileSystem.File.Path);
        return new TellContext(fileSystem.WorkingDir, makefile);
    }
}
