using TodoApp;

namespace TodoApp.Tests;

public class TodoStoreTests
{
    [Fact]
    public void Todo_CanBeCreated_WithAllFields()
    {
        var todo = new Todo("abc", "Buy milk", 2, false, null);

        Assert.Equal("abc", todo.Id);
        Assert.Equal("Buy milk", todo.Text);
        Assert.Equal(2, todo.Priority);
        Assert.False(todo.Completed);
        Assert.Null(todo.CompletedAt);
    }

    [Fact]
    public void Todo_WithExpression_ProducesUpdatedCopy()
    {
        var original = new Todo("abc", "Buy milk", 1, false, null);
        var updated = original with { Priority = 3 };

        Assert.Equal(1, original.Priority);
        Assert.Equal(3, updated.Priority);
    }

    [Fact]
    public void Load_WhenFileDoesNotExist_ReturnsEmptyList()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var store = new TodoStore(path);

        var result = store.Load();

        Assert.Empty(result);
    }

    [Fact]
    public void Save_ThenLoad_RoundTrips()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var store = new TodoStore(path);
        var todos = new List<Todo>
        {
            new("id1", "First", 1, false, null),
            new("id2", "Second", 2, true, DateOnly.FromDateTime(DateTime.Today))
        };

        store.Save(todos);
        var loaded = store.Load();

        Assert.Equal(2, loaded.Count);
        Assert.Equal("Second", loaded[0].Text);
        Assert.Equal(2, loaded[0].Priority);
        Assert.Equal("First", loaded[1].Text);
    }

    [Fact]
    public void Load_RemovesCompleted_FromPreviousDays()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var store = new TodoStore(path);
        var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        store.Save([new("id1", "Old done", 1, true, yesterday)]);

        var loaded = store.Load();

        Assert.Empty(loaded);
    }

    [Fact]
    public void Load_KeepsCompleted_FromToday()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var store = new TodoStore(path);
        var today = DateOnly.FromDateTime(DateTime.Today);
        store.Save([new("id1", "Done today", 1, true, today)]);

        var loaded = store.Load();

        Assert.Single(loaded);
    }

    [Fact]
    public void Load_KeepsIncomplete_Todos()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var store = new TodoStore(path);
        store.Save([new("id1", "Not done", 1, false, null)]);

        var loaded = store.Load();

        Assert.Single(loaded);
    }
}
