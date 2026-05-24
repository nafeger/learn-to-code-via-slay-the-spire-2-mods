# 03 — The Bug: Reading Compiler Errors

You just ran `dotnet build` and it failed. That's normal. Broken builds happen to every developer, every day. The skill isn't avoiding them — it's reading them.

## What you should have seen

The build output should contain errors that look roughly like this:

```
error CS1503: Argument 3: cannot convert from 'double' to 'MegaCrit.Sts2.Core.Nodes.Vfx.VfxColor'
error CS0121: The call is ambiguous between the following methods or properties: ...
```

Don't panic. Read them slowly.

## How to read a compiler error

A C# compiler error has a few parts:

```
src/StarterContent/cards/CoinFlip.cs(30,92): error CS1503: Argument 3: cannot convert from 'double' to '...'
```

- **File path** — which file the error is in (`CoinFlip.cs`)
- **Line and column** — exactly where in the file (`30,92` = line 30, character 92)
- **Error code** — a unique ID for this type of error (`CS1503`)
- **Message** — a description of what went wrong

## What do these errors mean?

Take a few minutes to work through these questions with your parent before reading on:

1. Open `src/StarterContent/cards/CoinFlip.cs` in VS Code. Go to line 30. What does the code there look like?
2. The error says it can't convert a `double` to a `VfxColor`. What do you think `double` means? What do you think `VfxColor` might be?
3. The second error says a call is "ambiguous." What does ambiguous mean in plain English? Why might that be a problem for a compiler?

## Vocabulary

- **compiler** — a program that translates your code into something the computer can run. It checks for errors before anything runs.
- **type** — a category of value. `double` is a number with a decimal point. `VfxColor` is probably some kind of color value specific to this game.
- **type mismatch** — when you try to use a value of one type where a different type is expected. Like trying to pass someone a phone number when they asked for a street address.
- **ambiguous** — when the compiler can't decide which of two things you meant, because both seem equally valid.

## Things to look up

- "CS1503 C# error"
- "CS0121 C# error"
- "what is a type in programming"

---

Next up: [04-the-fix.md](04-the-fix.md) — where these errors came from, and how to fix them.
