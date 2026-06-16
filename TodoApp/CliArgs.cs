namespace TodoApp;

public abstract record CliCommand;
public record LaunchTui : CliCommand;
public record ListCommand : CliCommand;
public record AddCommand(string Text, int Priority) : CliCommand;
public record ErrorCommand(string Message) : CliCommand;

public static class CliArgs
{
    public static CliCommand Parse(string[] args)
    {
        if (args.Length == 0) return new LaunchTui();

        return args[0] switch
        {
            "-l" or "list" => new ListCommand(),
            "-a" or "add"  => ParseAdd(args),
            _ => new ErrorCommand($"error: unknown command '{args[0]}'\nusage: todo [-l | list] [-a | add <text> [-p <1|2|3>]]")
        };
    }

    private static CliCommand ParseAdd(string[] args)
    {
        if (args.Length < 2 || string.IsNullOrWhiteSpace(args[1]))
            return new ErrorCommand("error: todo text required");

        var text = args[1];
        var priority = 1;

        if (args.Length > 2)
        {
            if (args.Length < 4 || args[2] != "-p")
                return new ErrorCommand($"error: unknown option '{args[2]}'");
            if (!int.TryParse(args[3], out priority) || priority < 1 || priority > 3)
                return new ErrorCommand("error: priority must be 1, 2, or 3");
        }

        return new AddCommand(text, priority);
    }
}
