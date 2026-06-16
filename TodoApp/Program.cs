using Spectre.Console;
using TodoApp;

var store = TodoStore.CreateDefault();

switch (CliArgs.Parse(args))
{
    case ListCommand:
        var todos = store.Load();
        if (todos.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No todos.[/]");
        }
        else
        {
            foreach (var todo in todos)
            {
                var escaped = Markup.Escape(todo.Text);
                if (todo.Completed)
                    AnsiConsole.MarkupLine($"[grey strikethrough][[x]] {escaped}[/]");
                else
                {
                    var color = todo.Priority switch { 2 => "yellow", 3 => "red", _ => "white" };
                    AnsiConsole.MarkupLine($"[{color}][[ ]] {escaped}[/]");
                }
            }
        }
        break;

    case AddCommand add:
        new App(store).AddTodo(add.Text, add.Priority);
        AnsiConsole.MarkupLine($"Added: {Markup.Escape(add.Text)}");
        break;

    case ErrorCommand err:
        Console.Error.WriteLine(err.Message);
        Environment.Exit(1);
        break;

    case LaunchTui:
        new App(store).Run();
        break;
}
