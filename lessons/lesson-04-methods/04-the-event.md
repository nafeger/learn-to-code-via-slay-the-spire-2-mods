# 04 — The Event

You have designed the methods. Now you write and wire up the event.

## The complete file

Create `src/StarterContent/events/RockPaperScissors.cs` inside your `JacksMod` project:

```csharp
using ModSmith.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace JacksMod;  // replace JacksMod with whatever you named your mod

public sealed class RockPaperScissors : ModSmithEventModel
{
    private enum Choice { Rock, Paper, Scissors }
    private enum Outcome { Win, Lose, Draw }

    private const int WIN_GOLD = 50;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new GoldVar(WIN_GOLD),
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return [
            new EventOption(this, () => Play(Choice.Rock),     "ROCK_PAPER_SCISSORS.options.ROCK"),
            new EventOption(this, () => Play(Choice.Paper),    "ROCK_PAPER_SCISSORS.options.PAPER"),
            new EventOption(this, () => Play(Choice.Scissors), "ROCK_PAPER_SCISSORS.options.SCISSORS"),
        ];
    }

    private async Task Play(Choice playerChoice)
    {
        Choice computerChoice = PickComputerChoice();
        Outcome outcome = DetermineOutcome(playerChoice, computerChoice);

        if (Owner is Player player)
        {
            if (outcome == Outcome.Win)
            {
                await PlayerCmd.GainGold(WIN_GOLD, player);
                SetEventFinished(L10NLookup("ROCK_PAPER_SCISSORS.pages.WIN.description"));
            }
            else if (outcome == Outcome.Lose)
            {
                SetEventFinished(L10NLookup("ROCK_PAPER_SCISSORS.pages.LOSE.description"));
            }
            else
            {
                SetEventState(L10NLookup("ROCK_PAPER_SCISSORS.pages.DRAW.description"), GenerateInitialOptions());
            }
        }
    }

    private Choice PickComputerChoice()
    {
        // Not perfectly uniform — Rock 50%, Paper/Scissors 25% each.
        // Lesson 9 covers proper uniform random selection.
        if (Rng.NextBool()) return Choice.Rock;
        if (Rng.NextBool()) return Choice.Paper;
        return Choice.Scissors;
    }

    private Outcome DetermineOutcome(Choice player, Choice computer)
    {
        if (player == computer) return Outcome.Draw;

        return (player, computer) switch
        {
            (Choice.Rock,     Choice.Scissors) => Outcome.Win,
            (Choice.Paper,    Choice.Rock)     => Outcome.Win,
            (Choice.Scissors, Choice.Paper)    => Outcome.Win,
            _                                  => Outcome.Lose,
        };
    }
}
```

## Walking through the parts you have not seen before

### CanonicalVars

```csharp
protected override IEnumerable<DynamicVar> CanonicalVars => [
    new GoldVar(WIN_GOLD),
];
```

`CanonicalVars` is how you tell the event system about dynamic values that will appear in localization text. `GoldVar` makes the gold amount available as a substitution token. You saw this pattern in `TheGoldCoinRoom`. It is boilerplate that you can copy for now; Lesson 5 will explain inheritance and `override` properly.

### WIN_GOLD

```csharp
private const int WIN_GOLD = 50;
```

`const` means this value never changes — it is a compile-time constant. Using a named constant instead of writing `50` everywhere is good practice: if you want to change the reward, you change it in one place and everything that references `WIN_GOLD` updates automatically. It also communicates intent: `WIN_GOLD` is more meaningful than a bare `50`.

### GenerateInitialOptions and lambdas

```csharp
protected override IReadOnlyList<EventOption> GenerateInitialOptions()
{
    return [
        new EventOption(this, () => Play(Choice.Rock),     "ROCK_PAPER_SCISSORS.options.ROCK"),
        new EventOption(this, () => Play(Choice.Paper),    "ROCK_PAPER_SCISSORS.options.PAPER"),
        new EventOption(this, () => Play(Choice.Scissors), "ROCK_PAPER_SCISSORS.options.SCISSORS"),
    ];
}
```

`EventOption` takes three arguments: the event it belongs to (`this`), a callback to run when the option is chosen, and a localization key for the button text.

> **Why `ROCK_PAPER_SCISSORS` and not `RPS`?** The first segment of every localization key has to match your class name, converted to SCREAMING_SNAKE_CASE — so `RockPaperScissors` becomes `ROCK_PAPER_SCISSORS`, exactly the way `TheGoldCoinRoom` becomes `THE_GOLD_COIN_ROOM`. If you used `RPS.options.ROCK`, the game would never find the text. This is a rule the framework enforces, and Lesson 7 explains exactly why it works that way. For now, just match the prefix to the class name.

The callbacks are **lambdas**: `() => Play(Choice.Rock)`. A lambda is an anonymous function — a small block of code without a name, defined inline. The `()` is an empty parameter list, and `=> Play(Choice.Rock)` is the body: call `Play` with `Choice.Rock`.

Why not just write `Play(Choice.Rock)` directly? Because `EventOption` needs to store the action and run it later, when the player clicks. If you wrote `Play(Choice.Rock)` without `() =>`, it would run immediately at construction time — before the player clicks anything. The lambda wraps it in a function that is called later.

You do not need to fully master lambdas now. The pattern `() => SomeMethod(someArgument)` is the one you will use most, and it is readable: "when triggered, call `SomeMethod` with `someArgument`."

### SetEventFinished and SetEventState

These are methods inherited from `ModSmithEventModel`:

- `SetEventFinished(description)` — Shows a description and ends the event with no further options.
- `SetEventState(description, options)` — Shows a description and replaces the current options with a new list.

On a draw, `SetEventState` is called with the initial options list — so the player can pick again. That is how the draw loop works without any explicit loop code.

## Register the event

Open `StarterContent.cs` and add one line:

```csharp
public static void RegisterStarterContent()
{
    Registry.RegisterPotion<DropOfGold>();
    Registry.RegisterCard<CoinFlip, ColorlessCardPool>();
    Registry.RegisterRelic<GoldArmor>();
    Registry.RegisterPotion<GoldPaint>();
    Registry.RegisterPower<MadeOfGold>();
    Registry.RegisterEvent<TheGoldCoinRoom>();
    Registry.RegisterEvent<RockPaperScissors>();   // add this line
    Registry.RegisterAncientEvent<GoldGuy, Hive>();
}
```

`Registry.RegisterEvent<RockPaperScissors>()` tells the mod framework that this event exists and should be included in the game's event pool.

## Build the mod

From the mod directory:

```
dotnet publish
```

If the build succeeds, the mod is ready to load. If you get a compile error, read the error message carefully — it will name the file, the line number, and describe what is wrong.

## Encounter the event in game

Events are drawn from a pool when you enter an event room (the "?" rooms on the map). `RockPaperScissors` is now in that pool, so you may encounter it on any run.

If you want to see it immediately:

- Check if ModSmith or the game has a developer mode or debug menu that lets you force a specific event.
- Otherwise, play a few runs and keep an eye out for it. Event rooms with multiple events in the pool can take some time before this one comes up.

## About the button text

When you encounter the event, the button text will show the localization key name instead of the real text. For example, the Rock button will say something like `ROCK_PAPER_SCISSORS.options.ROCK` rather than `Rock`.

Seeing the key on screen is actually useful: it tells you the exact string the game tried to look up and failed to find. If that key ever reads `RPS.options.ROCK`, you have a naming bug — the prefix does not match the class name. If it reads `ROCK_PAPER_SCISSORS.options.ROCK`, the key is correct and the only thing missing is the packaged text.

The game reads display text from localization data bundled into the game's PCK file. The localization JSON file you will create in chapter 05 exists in the project, but it will not be loaded by the game until the mod is packaged with PCK export — an advanced build step covered later. So even with correct keys, you will see the key names until the text is packaged. That is expected.

The event is fully functional. The text being placeholder keys does not affect the logic. You can play through it, win gold, lose, and draw — it all works.

## What you just built

You built a game inside a game. Rock Paper Scissors is a complete, runnable interactive experience: it takes player input, applies rules, generates a random opponent, branches on the result, and integrates with the game's gold system. And it is organized into methods that each do exactly one thing.

## Vocabulary

**const** — A compile-time constant. Its value is set once and never changes.

**Lambda** — An anonymous function defined inline. `() => Play(Choice.Rock)` is a lambda.

**Callback** — A function passed to another function to be called later.

**`SetEventFinished`** — Ends the event and shows a final description.

**`SetEventState`** — Updates the event with new description text and a new set of options.

**PCK file** — The packaged asset bundle for a Godot game. Localization data lives here.

## Things to look up

- "C# lambda expressions" — the full syntax and capabilities
- "C# const vs readonly" — when to use each
- "Godot PCK export" — for when you are ready to bundle assets
