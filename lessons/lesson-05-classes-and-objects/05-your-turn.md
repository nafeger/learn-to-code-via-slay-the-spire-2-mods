# 05 — Your Turn

Three exercises. Do the first two; the third is optional.

---

## Exercise A: Add a "rounds played" count

Add a feature to `Scoreboard`: track the total number of rounds played, and include it in
`Summary()`.

**Step 1**: Add a private field `roundsPlayed`.

**Step 2**: Update it in `Record` so every recorded result increases it by one.

**Step 3**: Include it in the `Summary()` text, e.g. `"... (round 3)"`.

Think about *where* the increment goes. You want exactly one place that runs on every
result, regardless of who won. Which method is that? (This is the single-source-of-truth
idea — there should be one obvious home for "a round happened.")

---

## Exercise B: Reason about object lifetime

Answer these in a comment or a note. They check that you understand *why* the design works,
which matters more than the syntax.

1. The `scoreboard` is a `private readonly` field on the event. Suppose you moved it to be
   a local variable declared inside `Play` instead:

   ```csharp
   private async Task Play(Choice playerChoice)
   {
       Scoreboard scoreboard = new();   // moved here
       // ...
   }
   ```

   What would the score be at the start of every round? Why? What feature would break?

2. If the player encounters a *second* Rock Paper Scissors event later in the same run, the
   game creates a new `RockPaperScissors` object for it. Does the new event start with a
   fresh scoreboard or the old one's score? Explain using the class/object distinction.

3. `RpsOpponent.Pick()` takes no parameters and `Scoreboard.Record(outcome)` takes one. Why
   does `Record` need a parameter but `Pick` does not? (Hint: think about what data each one
   needs that it does not already hold.)

---

## Exercise C (optional): A `RoundResult` class

Right now `Play` juggles two separate values per round — `playerChoice` and
`computerChoice` — plus the `outcome`. Try designing a small class that bundles them:

```csharp
public class RoundResult
{
    // fields: player's choice, computer's choice, the outcome
    // a constructor that takes all three
    // a Describe() method returning text like "You threw Rock, opponent threw Scissors — you win!"
}
```

You do not have to wire it into the event. The exercise is the *design*: what fields does
it hold, what does its constructor take, and what does `Describe()` return? This is the
same "what objects exist and what does each one know" question from chapter 01, applied to a
new little object.

If you do wire it in, have `Play` create a `RoundResult` each round and use its `Describe()`
in the `SetEventState` text instead of the bare score — so the player sees what just
happened, not only the tally.

---

## Connecting forward

Your opponent is an object now, but an empty-headed one — it picks at random and forgets
every throw. That is about to change.

In Lesson 6, you give `RpsOpponent` a **list** of your recent throws. Storing a growing
sequence of values is what collections are for, and once the opponent can *remember* what
you have been throwing, it can start to *counter* it. The scoreboard you built this lesson
is what will make that adaptation visible: throw Rock three times in a row and watch your
win count stop climbing.
