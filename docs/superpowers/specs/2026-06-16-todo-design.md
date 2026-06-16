# Todo App — Design Spec
Date: 2026-06-16

## Overview

A .NET 8 console todo app inspired by Vim's Netrw. Single-line todos with priority colours, strikethrough for completion, and Vim-style keyboard navigation. Backed by a JSON file.

## Architecture

Single console app (`TodoApp`) with one NuGet dependency: `Spectre.Console`.

**Files:**
- `Program.cs` — entry point, initialises store and app, starts render loop
- `Todo.cs` — data model
- `TodoStore.cs` — JSON persistence, purges stale completed items on load
- `TodoApp.cs` — main event loop: render, read key, dispatch action

**Data location:** `~/.todo/todos.json`

## Data Model

```json
[
  {
    "id": "uuid",
    "text": "Buy groceries",
    "priority": 1,
    "completed": false,
    "completedAt": null
  }
]
```

`Todo` C# record:
```csharp
record Todo(string Id, string Text, int Priority, bool Completed, DateOnly? CompletedAt);
```

## UI

Full-redraw loop on every keypress (Console.Clear + re-render). Simple and reliable for a short list.

```
  [ ] Buy groceries
  [ ] Fix the CI pipeline       ← cursor (highlighted background)
  [x] ~~Call dentist~~
```

Priority colours (applied to the entire line):
- Priority 1 → white (default)
- Priority 2 → yellow
- Priority 3 → red

Completed items: strikethrough, grey colour, regardless of priority.

## Keybindings

| Key | Action |
|-----|--------|
| `j` | Move cursor down |
| `k` | Move cursor up |
| `o` | Add new todo (inline prompt at bottom of list) |
| `Enter` | Toggle complete on selected item, sets `completedAt` to today |
| `dd` | Delete selected item — first `d` arms (1-second window), second `d` fires |
| `1` / `2` / `3` | Set priority on selected item |
| `q` | Quit |

## Persistence & Completed Item Purge

On startup, `TodoStore.Load()` reads `todos.json` and removes all items where `completed == true && completedAt < DateOnly.FromDateTime(DateTime.Today)`. This implements "disappear the following day" with no background process.

`TodoStore.Save()` is called after every mutation.

## Adding a Todo

Pressing `o` renders an input prompt below the list. The app enters insert mode: typed characters append to a buffer, Backspace removes the last character, Enter saves (if buffer non-empty), Esc cancels. After save, cursor moves to the new item.

## Out of Scope

- Multi-line todos
- Due dates or scheduling
- Sorting or filtering
- Syncing or cloud storage
- Undo
