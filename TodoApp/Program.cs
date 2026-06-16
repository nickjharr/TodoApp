using Spectre.Console;
using TodoApp;

var store = TodoStore.CreateDefault();

if (args.Length > 0 && args[0] == "-l")
{
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
    return;
}

new App(store).Run();
