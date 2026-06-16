using TodoApp;

namespace TodoApp.Tests;

public class AppTests
{
    private static App MakeApp(params Todo[] todos)
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var store = new TodoStore(path);
        store.Save(todos.ToList());
        return new App(store);
    }

    [Fact]
    public void MoveCursorDown_IncrementsIndex()
    {
        var app = MakeApp(
            new Todo("1", "First", 1, false, null),
            new Todo("2", "Second", 1, false, null));

        app.MoveCursorDown();

        Assert.Equal(1, app.CursorIndex);
    }

    [Fact]
    public void MoveCursorDown_ClampsAtEnd()
    {
        var app = MakeApp(new Todo("1", "Only", 1, false, null));

        app.MoveCursorDown();

        Assert.Equal(0, app.CursorIndex);
    }

    [Fact]
    public void MoveCursorUp_DecrementsIndex()
    {
        var app = MakeApp(
            new Todo("1", "First", 1, false, null),
            new Todo("2", "Second", 1, false, null));
        app.MoveCursorDown();

        app.MoveCursorUp();

        Assert.Equal(0, app.CursorIndex);
    }

    [Fact]
    public void MoveCursorUp_ClampsAtStart()
    {
        var app = MakeApp(new Todo("1", "Only", 1, false, null));

        app.MoveCursorUp();

        Assert.Equal(0, app.CursorIndex);
    }

    [Fact]
    public void ToggleComplete_MarksItemComplete_WithToday()
    {
        var app = MakeApp(new Todo("1", "Task", 1, false, null));

        app.ToggleComplete();

        Assert.True(app.Todos[0].Completed);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), app.Todos[0].CompletedAt);
    }

    [Fact]
    public void ToggleComplete_UncompletesClearsDate()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var app = MakeApp(new Todo("1", "Task", 1, true, today));

        app.ToggleComplete();

        Assert.False(app.Todos[0].Completed);
        Assert.Null(app.Todos[0].CompletedAt);
    }

    [Fact]
    public void SetPriority_UpdatesPriority()
    {
        var app = MakeApp(new Todo("1", "Task", 1, false, null));

        app.SetPriority(3);

        Assert.Equal(3, app.Todos[0].Priority);
    }

    [Fact]
    public void SetPriority_IgnoresInvalidValues()
    {
        var app = MakeApp(new Todo("1", "Task", 2, false, null));

        app.SetPriority(0);
        app.SetPriority(4);

        Assert.Equal(2, app.Todos[0].Priority);
    }

    [Fact]
    public void DeleteSelected_RemovesTodo()
    {
        var app = MakeApp(
            new Todo("1", "First", 1, false, null),
            new Todo("2", "Second", 1, false, null));

        app.DeleteSelected();

        Assert.Single(app.Todos);
        Assert.Equal("Second", app.Todos[0].Text);
    }

    [Fact]
    public void DeleteSelected_AdjustsCursor_WhenDeletingLastItem()
    {
        var app = MakeApp(
            new Todo("1", "First", 1, false, null),
            new Todo("2", "Second", 1, false, null));
        app.MoveCursorDown();

        app.DeleteSelected();

        Assert.Equal(0, app.CursorIndex);
    }

    [Fact]
    public void AddTodo_AppendsTodo_AndMovesCursor()
    {
        var app = MakeApp(new Todo("1", "Existing", 1, false, null));

        app.AddTodo("New task");

        Assert.Equal(2, app.Todos.Count);
        Assert.Equal("New task", app.Todos[1].Text);
        Assert.Equal(1, app.CursorIndex);
    }

    [Fact]
    public void AddTodo_IgnoresBlankText()
    {
        var app = MakeApp(new Todo("1", "Existing", 1, false, null));

        app.AddTodo("   ");

        Assert.Single(app.Todos);
    }

    [Fact]
    public void AddTodo_SetsPriorityToOne()
    {
        var app = MakeApp();

        app.AddTodo("Task");

        Assert.Equal(1, app.Todos[0].Priority);
    }
}
