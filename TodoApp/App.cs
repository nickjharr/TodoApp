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

    public void Run() => throw new NotImplementedException();
}
