# 02 — Official Setup: Install the Tools

The ModSmith project has an official guided setup that covers everything you need to install. Follow it now.

## The official guide

👉 [https://cpimhoff.github.io/Sts2-ModSmith/docs/setup/guided.html](https://cpimhoff.github.io/Sts2-ModSmith/docs/setup/guided.html)

Work through that guide completely. It covers:

- Installing the .NET SDK
- Installing MegaDot (the Godot editor used by STS2)
- Configuring your Steam library path
- Opening the project

### .NET version note

The guide may suggest installing .NET 9 via a curl script. On Mac, the simpler path is:

```bash
brew install --cask dotnet-sdk
```

This installs .NET 10, which works fine — the mod project targets `net9.0` but any newer SDK can build it. If `dotnet --version` shows 10.x after installing, you're good.

## Recommended editor: VS Code

The guide may mention several editors. Use **Visual Studio Code** — it's free, fast, and has excellent C# support. You'll use it for the rest of this curriculum.

Download it at [https://code.visualstudio.com](https://code.visualstudio.com).

Once installed, add the **C# Dev Kit** extension:
1. Open VS Code
2. Click the Extensions icon in the left sidebar (or press `Cmd+Shift+X` / `Ctrl+Shift+X`)
3. Search for "C# Dev Kit" and install it

## What you cloned

When you ran `git clone https://github.com/cpimhoff/Sts2-ModSmith.git` in step 01, it created a folder called `Sts2-ModSmith` wherever your terminal was. Inside that folder is the actual mod template you'll work with, called `ModTemplate`:

```
Sts2-ModSmith/
  ModTemplate/          <-- this is your mod project
    src/
      StarterContent/
        cards/
          CoinFlip.cs
        ...
    project.godot
    ModTemplate.json
  ...
```

Every time the lessons refer to a file like `src/StarterContent/cards/CoinFlip.cs`, that path is relative to the `ModTemplate` folder.

## Try to build

Open a terminal, navigate into `ModTemplate`, and run:

```bash
cd Sts2-ModSmith/ModTemplate
```

Then:

```bash
dotnet build
```

**Something is going to go wrong.** That's intentional. Read the output carefully before moving on.

---

Next up: [03-the-bug.md](03-the-bug.md) — let's look at what just happened.
