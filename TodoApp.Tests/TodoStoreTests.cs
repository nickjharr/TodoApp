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
}
