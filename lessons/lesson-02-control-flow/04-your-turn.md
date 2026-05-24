# 04 — Your Turn: A Third Option

This exercise adds a third option to `TheGoldCoinRoom` event. Instead of just taking gold or doubling it, the player can now examine the coin — which pays out a fraction of the gold depending on how much is available.

## The goal

Add an "Examine it" option that works like this:

- If gold is less than 30: gain all of it and finish the event
- If gold is 30 or more but less than 60: gain half of it and finish
- If gold is 60 or more: gain a third of it and finish

This is straightforward `if`/`else if`/`else` with integer division. You'll write the method, register the option, and see it in the game.

## Step 1: Read the existing code

Open `src/StarterContent/events/TheGoldCoinRoom.cs` in your `JacksMod` project.

Before touching anything, read through it. Notice:

- `TakeGold` is `private async Task` — it uses `await` inside, so it needs `async`
- `DoubleIt` is `private Task` — it does not use `await`, so it returns `Task.CompletedTask` directly
- Both methods call either `SetEventFinished` or `SetEventState` at the end
- The current gold amount is `DynamicVars.Gold.IntValue`
- The player reference comes from `Owner is Player player`

Your new method follows the same patterns.

### What is `Task`?

A `Task` represents work that will happen — and may not be done yet. Some game commands (like giving gold) need to run as animations in sequence, so the engine uses `Task` to say "start this, and don't move on until it finishes."

When a method contains `await`, you mark it `async Task` and the compiler handles the sequencing. When a method never awaits anything, you write `private Task` and return `Task.CompletedTask` to signal "nothing to wait for." Either way, the event system treats them the same.

You don't need to fully understand this yet — the important thing is to match the pattern: use `await` when calling commands like `PlayerCmd.GainGold`, and mark your method `async Task` whenever you do.

## Step 2: Write the ExamineIt method

Add this method to the class, below `DoubleIt`:

```csharp
private async Task ExamineIt()
{
    if (Owner is Player player)
    {
        int gold = DynamicVars.Gold.IntValue;

        int payout;
        if (gold < 30)
        {
            payout = gold;
        }
        else if (gold < 60)
        {
            payout = gold / 2;
        }
        else
        {
            payout = gold / 3;
        }

        await PlayerCmd.GainGold(payout, player);
    }
    SetEventFinished(L10NLookup("THE_GOLD_COIN_ROOM.pages.TAKEN.description"));
}
```

A few things to notice:

- `payout` is declared before the `if`/`else if`/`else` chain and assigned inside it. Every branch assigns a value, so by the time you reach `GainGold`, `payout` is always set.
- Integer division in C# truncates (rounds toward zero). `7 / 2` is `3`, not `3.5`. That's intentional here — gold amounts are always whole numbers.
- The conditions are `gold < 30` and `gold < 60`, not `gold >= 30 && gold < 60`. The second condition only runs if the first was false, so you already know `gold >= 30`. You don't need to repeat it.
- The method reuses the `TAKEN` description key as a placeholder. If you want to add a real localization key for "Examined" later, that's a fine extension.

## Step 3: Register the option

In `GenerateInitialOptions`, add a third `EventOption`:

```csharp
protected override IReadOnlyList<EventOption> GenerateInitialOptions()
{
    return [
        new EventOption(this, TakeGold, "THE_GOLD_COIN_ROOM.pages.INITIAL.options.TAKE"),
        new EventOption(this, DoubleIt, "THE_GOLD_COIN_ROOM.pages.INITIAL.options.DOUBLE_IT"),
        new EventOption(this, ExamineIt, "THE_GOLD_COIN_ROOM.pages.INITIAL.options.TAKE"),
    ];
}
```

The third option reuses the `TAKE` localization key as a placeholder (since there isn't a real "Examine it" key yet). The text will say "Take" in the game, but the behavior will be the new method. That's fine for now.

## Step 4: Build and verify

```bash
dotnet publish
```

Launch the game, load your mod, and find a Gold Coin Room event. You should see three options. Select the third one with different gold amounts and verify:

- With 20 gold: you gain 20 gold
- With 45 gold: you gain 22 gold (45 / 2 = 22, truncated)
- With 90 gold: you gain 30 gold (90 / 3 = 30)

## Step 5: Commit your work

```bash
git add src/StarterContent/events/TheGoldCoinRoom.cs
git commit -m "feat: add ExamineIt option to TheGoldCoinRoom"
```

## What you just practiced

- Writing `if`/`else if`/`else` with comparison operators
- Ordering conditions correctly (most restrictive first)
- Using integer division to produce a whole-number result
- Matching the method signature pattern used in the existing code (`private async Task`)
- Reading an unfamiliar file before modifying it

## Bonus challenge

The `ExamineIt` method always reuses the `TAKEN` description. Look at how `DoubleIt` passes different strings to `SetEventState`. Can you modify `ExamineIt` to set a different final message depending on which payout tier was used?

You'll need to restructure slightly: instead of assigning just `payout` in each branch, also assign a `string description` and pass it to `SetEventFinished`.

---

## You're done with Lesson 2

Check off the success criteria in [README.md](README.md).

Next lesson covers loops — after that, Lesson 4 is where you'll put conditions, loops, and methods together to build Rock Paper Scissors as a full playable event.

**[Lesson 3 → Loops](../lesson-03-loops-and-recursion/README.md)**
