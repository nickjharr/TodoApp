namespace TodoApp;

public record Todo(string Id, string Text, int Priority, bool Completed, DateOnly? CompletedAt);
