# 02 — Official Setup: Install the Tools

The ModSmith project has an official guided setup that covers everything you need to install. Follow it now.

## The official guide

👉 [https://cpimhoff.github.io/Sts2-ModSmith/docs/setup/guided.html](https://cpimhoff.github.io/Sts2-ModSmith/docs/setup/guided.html)

Work through the guide **up to and including** the "Install ModSmith" section. It covers:

- Installing the .NET SDK
- Installing MegaDot (the Godot editor used by STS2)
- Configuring your Steam library path
- Installing ModSmith into the game's mods folder

> **Stop before "Initializing your mod from the template."** That section tells you to run `dotnet new install ModSmith.Templates` and `dotnet new modsmith-mod`. Do not do that yet — the template on NuGet has bugs you will see firsthand in the next step, and step 04 walks you through installing a fixed version instead.

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

When you ran `git clone https://github.com/cpimhoff/Sts2-ModSmith.git` in step 01, it created a folder called `Sts2-ModSmith` inside `~/code`. Inside that folder is a `ModTemplate` directory — this is the example project that ships with the framework:

```
~/code/
  Sts2-ModSmith/
    ModTemplate/        <-- example project (reference material)
      src/
        StarterContent/
          cards/
            CoinFlip.cs
          ...
      project.godot
      ModTemplate.json
    ...
```

You're going to build directly from `ModTemplate` now — just to see what happens. In step 04 you'll create your own mod project that you'll work in for the rest of the lessons.

## Try to build

Open a terminal, navigate into `ModTemplate`, and run:

```bash
cd ~/code/Sts2-ModSmith/ModTemplate
```

Then:

```bash
dotnet build
```

**Something is going to go wrong.** That's intentional. Read the output carefully before moving on.

---

Next up: [03-the-bug.md](03-the-bug.md) — let's look at what just happened.
