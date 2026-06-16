using TodoApp;

var store = TodoStore.CreateDefault();
var app = new App(store);
app.Run();
