# todo

A terminal todo app inspired by vim's Netrw. Launches a keyboard-driven TUI or accepts CLI subcommands for quick add/list operations.

## Usage

```
todo                        # launch TUI
todo list                   # print todos to stdout
todo add "buy milk"         # add a todo (priority 1)
todo add "fix prod" -p 3    # add a todo with priority 3
```

Flags `-l` and `-a` are aliases for `list` and `add`.

## TUI keybindings

| Key | Action |
|-----|--------|
| `j` / `k` | move cursor down / up |
| `o` | add a new todo |
| `r` | rename selected todo |
| `Enter` | toggle complete |
| `dd` | delete selected todo |
| `1` / `2` / `3` | set priority |
| `q` | quit |

## Priorities

- `1` — white (default)
- `2` — yellow
- `3` — red

Completed todos appear with a strikethrough and are removed the following day.

## Data

Todos are stored as JSON at `~/.todo.json`.

## Requirements

- .NET 8
- [Spectre.Console](https://spectreconsole.net/)

## Build

```
dotnet build
dotnet run --project TodoApp
```
