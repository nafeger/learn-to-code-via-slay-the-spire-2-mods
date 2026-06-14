# 05 — Your Turn

Three exercises. Do A and B; C is optional.

---

## Exercise A: Tune the memory and the bias

Two numbers control how the opponent feels. Change each, rebuild, and play a few rounds to
feel the difference.

1. **`MEMORY_SIZE`** in `RpsOpponent`. Try `1` (reacts only to your *last* throw) and `6`
   (reacts to a longer pattern). Which feels smarter? Which is easier to fool? Write down
   what you notice.

2. **The counter probability.** The line `Rng.Chaotic.NextBool() || Rng.Chaotic.NextBool()`
   counters ~75% of the time. Change it to a single `Rng.Chaotic.NextBool()` (50%) and then
   to three flips `||`'d together (87.5%). At which setting does the opponent feel
   *unfairly* good?

There is no single right answer — the exercise is to connect a number in the code to a
feeling in play, and to notice that "good game balance" is a tuning decision, not a formula.

---

## Exercise B: Trace the tally by hand

Given this sequence of player throws into a `MEMORY_SIZE = 3` opponent, write out the
`recentThrows` window after each throw, then the result of `MostFrequent` at the end.

Throws in order: **Paper, Rock, Rock, Scissors, Rock**

| Throw | `recentThrows` window after |
|-------|-----------------------------|
| Paper | ? |
| Rock | ? |
| Rock | ? |
| Scissors | ? |
| Rock | ? |

1. What does the window hold after all five throws?
2. Run the tally pattern on that final window. What count does each `Choice` have?
3. What does `MostFrequent` return, and therefore what will the opponent most likely throw
   next?

Do this on paper before you trust the code. If you can trace it, you understand it.

---

## Exercise C (optional): Count wins by player choice

Add a `Dictionary<Choice, int>` to the `Scoreboard` (or a new small class) that tracks how
many rounds the player **won** with each throw. After a match, you could answer "Rock won me
the most rounds."

**Step 1**: Add a field `private readonly Dictionary<Choice, int> winsByChoice = new();`

**Step 2**: You will need the player's choice at record time. That means `Record` needs to
*also* take the player's `Choice` — change its signature to
`Record(Outcome outcome, Choice playerChoice)` and update the one call in `Play`.

**Step 3**: When the outcome is a win, bump `winsByChoice[playerChoice]` using the tally
pattern (the `if (!ContainsKey) = 0` guard, then `+ 1`).

**Step 4**: Add a method that returns the choice with the most wins, reusing the
`MostFrequent`-style max walk.

This combines everything from the lesson: a dictionary, the tally pattern, the max walk, and
changing a method's signature to give it the data it needs.

---

## Connecting forward

You now have two kinds of opponent behavior tangled together in one class: the *random*
fallback and the *adaptive* countering. Right now they live in the same `Pick` method,
chosen with an `if`.

What if you wanted to offer an easy opponent (always random) and a hard one (adaptive) and
switch between them cleanly — without `if`s scattered through the event? That is a job for
**interfaces and inheritance**, and it is exactly how the mod framework itself lets *you*
plug custom content into the game. Lesson 7 separates the two opponents into swappable
classes, and then uses that same idea to finally explain why `ModSmithEventModel`,
`override`, and the localization key rule all work the way they do.
