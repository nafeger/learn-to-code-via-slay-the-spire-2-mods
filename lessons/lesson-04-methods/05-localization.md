# 05 — Localization (Optional Polish)

This chapter is optional. The event works without it. Read it when you want to understand where the placeholder key names come from and what it would take to show real text.

## What localization files are

Localization files are JSON files that map keys to human-readable text. Instead of writing `"Rock"` directly in C# code, you write `"ROCK_PAPER_SCISSORS.options.ROCK"` — a key — and the game looks up the real text in a file at runtime.

This separation exists because games ship in multiple languages. The same key `ROCK_PAPER_SCISSORS.options.ROCK` can map to `"Rock"` in English and `"Pierre"` in French. The C# code never changes; only the localization files do.

Even for a mod that only ships in English, the pattern is the same. You put all your player-facing strings in a localization file instead of scattering them through the code.

## Where the file lives

The localization file for events in this mod lives at:

```
localization/eng/events.json
```

This path is relative to the mod project root. The `eng` folder holds English strings. If you wanted to add French, you would create a `fre` folder alongside it.

## The events.json file

This file has already been created for the Rock Paper Scissors event at `localization/eng/events.json` in your `JacksMod` project. Its contents:

```json
{
  "ROCK_PAPER_SCISSORS.title": "Rock, Paper, Scissors",
  "ROCK_PAPER_SCISSORS.pages.INITIAL.description": "A mysterious stranger grins at you. \"Care for a wager? Rock, paper, scissors -- winner takes fifty gold.\"",
  "ROCK_PAPER_SCISSORS.options.ROCK.title": "Rock",
  "ROCK_PAPER_SCISSORS.options.ROCK.description": "A solid, dependable choice.",
  "ROCK_PAPER_SCISSORS.options.PAPER.title": "Paper",
  "ROCK_PAPER_SCISSORS.options.PAPER.description": "Covers all bases.",
  "ROCK_PAPER_SCISSORS.options.SCISSORS.title": "Scissors",
  "ROCK_PAPER_SCISSORS.options.SCISSORS.description": "Sharp and decisive.",
  "ROCK_PAPER_SCISSORS.pages.WIN.description": "The stranger shakes their head with a laugh and hands over the gold.",
  "ROCK_PAPER_SCISSORS.pages.LOSE.description": "\"Better luck next time,\" the stranger says, already pocketing nothing.",
  "ROCK_PAPER_SCISSORS.pages.DRAW.description": "\"A tie! Let's go again.\""
}
```

Each entry is a key-value pair. The key is what the C# code passes to `L10NLookup`. The value is what the player sees.

## Why the text shows as key names

The game loads its localization data from the PCK file — the packaged asset bundle for the Godot project. Assets and data files that are part of the game need to be bundled into this file to be accessible at runtime.

When you run `dotnet publish`, you are only compiling the C# mod code. The localization JSON file exists in your project folder, but it has not been bundled into a PCK. The game cannot find it, so when it tries to look up `ROCK_PAPER_SCISSORS.options.ROCK`, it finds nothing and falls back to showing the key name itself.

This is expected. The `MissingLocPatch` in the mod framework prevents a crash — instead of an error, you just see the key. The event is fully playable; the text is just ugly.

## Two different reasons a key can show up

It is worth separating two causes that look identical on screen but are not the same problem:

1. **The text is not packaged yet.** The key is correct, but the PCK has not been exported. This is the situation above, and it is harmless — the fix is the PCK export step, covered later.
2. **The key name is wrong.** The first segment of the key does not match the event's class name. Here the lookup would fail *even after* the PCK is exported, because the game derives the key namespace from the class name (`RockPaperScissors` → `ROCK_PAPER_SCISSORS`). A key like `RPS.options.ROCK` is permanently broken, not just unpackaged.

Both render as the raw key on screen, which is why reading the exact key the game prints is useful — it tells you which problem you have. Lesson 7 explains the class-name-to-key-prefix rule in full.

## What PCK export involves

Bundling assets into a PCK requires working with the Godot editor and the ModSmith tooling. The exact steps depend on the version of ModSmith you are using and how Godot project export is configured.

This is an advanced build step that is not part of the current lessons. It will be covered when the curriculum gets to asset packaging.

For now: the localization file is in place and ready. When you get to the PCK export step, the text will appear correctly with no changes needed to the C# code or the JSON file.

## Vocabulary

**Localization** — The practice of separating human-readable text from code so it can be translated and maintained independently.

**Localization key** — A string used in code to reference a piece of text. `L10NLookup("ROCK_PAPER_SCISSORS.options.ROCK")` looks up this key.

**PCK file** — Godot's packaged asset format. Assets must be in a PCK for the game to find them at runtime.

**MissingLocPatch** — A mod framework patch that shows key names as fallback text when a localization entry is not found.

## Things to look up

- "Godot localization" — how the Godot engine handles translation files
- "ModSmith PCK export" — how to bundle mod assets into a loadable file
- "JSON format" — if the syntax of `{ "key": "value" }` is unfamiliar
