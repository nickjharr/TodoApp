using System.Text.Json;

namespace TodoApp;

public class TodoStore
{
    private readonly string _path;

    public TodoStore(string path) => _path = path;

    public static TodoStore CreateDefault()
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".todo");
        Directory.CreateDirectory(dir);
        return new TodoStore(Path.Combine(dir, "todos.json"));
    }

    public List<Todo> Load()
    {
        if (!File.Exists(_path))
            return [];

        var json = File.ReadAllText(_path);
        var todos = JsonSerializer.Deserialize<List<Todo>>(json, JsonOptions) ?? [];
        var today = DateOnly.FromDateTime(DateTime.Today);
        return todos.Where(t => !(t.Completed && t.CompletedAt < today))
                    .OrderByDescending(t => t.Priority)
                    .ToList();
    }

    public void Save(List<Todo> todos)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path, JsonSerializer.Serialize(todos, JsonOptions));
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
}
