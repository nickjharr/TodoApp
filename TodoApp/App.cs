using Spectre.Console;

namespace TodoApp;

public class App
{
    private readonly TodoStore _store;
    public List<Todo> Todos { get; private set; }
    public int CursorIndex { get; private set; }

    private bool _insertMode;
    private string _inputBuffer = "";
    private bool _dArmed;
    private DateTime _dArmedAt;

    public App(TodoStore store)
    {
        _store = store;
        Todos = store.Load();
        CursorIndex = 0;
    }

    public void MoveCursorDown()
    {
        if (CursorIndex < Todos.Count - 1)
            CursorIndex++;
    }

    public void MoveCursorUp()
    {
        if (CursorIndex > 0)
            CursorIndex--;
    }

    public void ToggleComplete()
    {
        if (Todos.Count == 0) return;
        var todo = Todos[CursorIndex];
        var today = DateOnly.FromDateTime(DateTime.Today);
        Todos[CursorIndex] = todo with
        {
            Completed = !todo.Completed,
            CompletedAt = !todo.Completed ? today : null
        };
        _store.Save(Todos);
    }

    public void SetPriority(int priority)
    {
        if (Todos.Count == 0 || priority < 1 || priority > 3) return;
        Todos[CursorIndex] = Todos[CursorIndex] with { Priority = priority };
        _store.Save(Todos);
    }

    public void DeleteSelected()
    {
        if (Todos.Count == 0) return;
        Todos.RemoveAt(CursorIndex);
        if (CursorIndex >= Todos.Count && CursorIndex > 0)
            CursorIndex--;
        _store.Save(Todos);
    }

    public void AddTodo(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        var todo = new Todo(Guid.NewGuid().ToString(), text.Trim(), 1, false, null);
        Todos.Add(todo);
        CursorIndex = Todos.Count - 1;
        _store.Save(Todos);
    }

    private void Render()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[bold white]todo[/]");
        AnsiConsole.WriteLine();

        if (Todos.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No todos. Press [bold]o[/] to add one.[/]");
        }
        else
        {
            for (int i = 0; i < Todos.Count; i++)
            {
                var todo = Todos[i];
                bool isCursor = i == CursorIndex;
                var escaped = Markup.Escape(todo.Text);

                string line;
                if (todo.Completed)
                {
                    line = $"[grey strikethrough][x] {escaped}[/]";
                }
                else
                {
                    var color = todo.Priority switch
                    {
                        2 => "yellow",
                        3 => "red",
                        _ => "white"
                    };
                    line = $"[{color}][ ] {escaped}[/]";
                }

                var prefix = isCursor ? "[bold white]>[/]" : " ";
                AnsiConsole.MarkupLine($" {prefix} {line}");
            }
        }

        AnsiConsole.WriteLine();

        if (_insertMode)
        {
            AnsiConsole.Markup($"[bold white]>[/] {Markup.Escape(_inputBuffer)}");
        }
        else
        {
            if (_dArmed)
                AnsiConsole.MarkupLine("[yellow]d[/][grey] — press d again to delete[/]");
            else
                AnsiConsole.MarkupLine("[grey]j/k · o: add · Enter: done · dd: delete · 1/2/3: priority · q: quit[/]");
        }
    }

    public void Run() => throw new NotImplementedException();
}
