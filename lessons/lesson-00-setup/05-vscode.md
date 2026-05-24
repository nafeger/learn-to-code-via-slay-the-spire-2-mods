# 05 — VS Code: Your Editor and Debugger

You can write code in any text editor, but a good editor makes you significantly faster. VS Code is the recommended editor for this curriculum. It's free, runs on any platform, and has excellent support for C#.

## Make sure VS Code is set up

From step 02 you should already have:
- VS Code installed
- The **C# Dev Kit** extension installed

If not, go back to [02-setup.md](02-setup.md) and do those steps now.

## Open the project

You want to open your mod — the one you scaffolded in step 04, at `~/code/JacksMod` (or whatever you named it).

1. Open VS Code
2. Go to **File → Open Folder**
3. Navigate to `~/code/JacksMod` and select it

VS Code will load the project. The C# Dev Kit will analyze the code — give it a moment to finish (you'll see a spinner in the status bar at the bottom).

## Set a breakpoint

A **breakpoint** tells the debugger "pause execution here so I can inspect what's happening." It's one of the most powerful tools you have.

1. In the left sidebar, click the **Explorer** icon and open `src/ModTemplate.cs`
2. Find the `Initialize()` method
3. Click in the grey margin to the left of the line that says `Logger.Info("Initializing...");`
4. A red dot should appear — that's your breakpoint

## Attach the debugger to the game

The game runs as a separate process. To debug your mod, you attach VS Code to the running game.

1. Run `dotnet publish` from the terminal in your `ModTemplate` folder to build and copy your mod to the game
2. Launch Slay the Spire 2 with mods enabled
3. In VS Code, open the **Run and Debug** panel (left sidebar, or `Cmd+Shift+D` / `Ctrl+Shift+D`)
4. Click **"create a launch.json file"** if prompted, and select **.NET**
5. From the dropdown at the top, select **".NET: Attach to process"**
6. Click the green play button
7. Search for "Slay the Spire" in the process list and select it

When the game loads your mod, it will hit the breakpoint and pause. VS Code will show you exactly where execution stopped.

## What to look at when paused

When the debugger pauses at your breakpoint:

- **Variables panel** (left) — shows the current values of variables in scope
- **Call stack** (left) — shows how the code got to this point, like a breadcrumb trail
- **Debug toolbar** (top) — lets you step forward one line at a time, step into a method, or continue running

Press the green **Continue** button (or `F5`) to let the game keep running.

## Vocabulary

- **debugger** — a tool that lets you pause a running program, inspect its state, and step through it line by line
- **breakpoint** — a marker that tells the debugger to pause at a specific line
- **call stack** — the chain of method calls that led to where you are right now
- **attach** — connecting the debugger to a process that's already running

## Things to look up

- "how to debug C# in VS Code"
- "what is a debugger in programming"
- "call stack explained"

---

Next up: [06-first-commit.md](06-first-commit.md) — make the mod yours.
