using CliWrap;

namespace Tell;

public static class StandardCommand
{
    public static Command From(string commandString, string workingDirectory, bool pipeToConsole = true)
    {
        var parts = commandString.Split(' ');
        var command = parts[0];
        var args = parts.Skip(1).ToArray();

        var basicCommand = Cli.Wrap(command)
            .WithArguments(args)
            .WithWorkingDirectory(workingDirectory);

        if (!pipeToConsole) return basicCommand;

        return basicCommand
            .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.Error.WriteLine))
            .WithStandardOutputPipe(PipeTarget.ToDelegate(Console.WriteLine));
    }
}
