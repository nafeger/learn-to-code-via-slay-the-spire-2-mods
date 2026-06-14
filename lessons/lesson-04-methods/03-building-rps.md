# 03 — Building Rock Paper Scissors

Before writing a single line of real code, spend time with the design. This is the habit that separates people who can program from people who can write programs that work.

## The problem, stated plainly

Rock Paper Scissors is:

1. The player picks Rock, Paper, or Scissors.
2. The computer picks one too.
3. Compare the two picks according to the rules.
4. If the player wins, they get gold. If they lose, nothing happens. If it is a draw, they play again.

That description already tells you what methods you need.

Programming is hard for at least two distinct reasons. The first is figuring out what you want to build — the problem itself. The second is figuring out how to model it in code: what types represent your data, what methods represent your actions, and how they talk to each other. These are separate skills, and good developers work on them separately. You write out the problem in plain words first, then translate those words into types and methods.

That is exactly what this lesson does. The plain-language description gave you `Choice`, `Outcome`, `PickComputerChoice`, `DetermineOutcome`, and `Play` before you wrote a single line of C#.

The type question — how to model a choice or an outcome — is why the earlier lessons talked about enums, structs, and the difference between a type that is just a number versus a type that carries meaning. You will revisit this more deeply in Lesson 5 when you start building your own classes. And when collections come up in Lesson 6, you will see how the same two-step process applies there: what does the collection represent, and which collection type fits?

## Step 1: Name the concepts

Before writing code, name the things you are working with. In Rock Paper Scissors:

- A **choice** is one of three options: Rock, Paper, or Scissors.
- An **outcome** is one of three results: Win, Lose, or Draw.

These are not strings or integers — they are their own type. C# has a way to define exactly this kind of thing: an **enum**.

An enum is a type you define as a fixed set of named values:

```csharp
private enum Choice { Rock, Paper, Scissors }
private enum Outcome { Win, Lose, Draw }
```

Now `Choice.Rock` is a real value in your program. It is not the string `"Rock"`. It is not the number `0`. It is `Choice.Rock`, and the compiler knows the difference. If you accidentally write `Choice.Rocket`, the compiler will stop you immediately.

Enums live inside the class, declared at the top before the methods. They are `private` here because nothing outside `RockPaperScissors` needs to know about them.

## Step 2: Design DetermineOutcome

This is the pure logic of the game. It takes two choices and returns an outcome. It does not know about gold, the event system, or the player. It is just rules.

Write the signature first:

```csharp
private Outcome DetermineOutcome(Choice player, Choice computer)
```

Two inputs, one output. Now fill in the logic.

The draw case is easy: if both choices are the same, it is a draw.

```csharp
if (player == computer) return Outcome.Draw;
```

### What does `==` actually compare?

This is worth pausing on, because the answer depends on what type you are comparing.

**For value types** (primitives like `int`, `bool`, `double`, and enums), `==` compares the values directly. `Choice.Rock == Choice.Rock` is `true`. Under the hood, `Choice` is just an integer, so this is integer comparison.

**For reference types** (most classes), `==` by default checks whether both variables point to the exact same object in memory — not whether the objects contain the same data. Two different `Player` objects with identical stats would not be `==` unless they were literally the same instance.

This is the same distinction as Java's `==` (reference) vs `.equals()` (value). In C#, the equivalent of Java's `.equals()` is `.Equals()`. The equivalent of Java's `.hashCode()` is `.GetHashCode()`. They come from the same root — every object in both languages has these methods because every class ultimately inherits from a base object class.

In C#, you can override `==` directly on a class (Java cannot do this with operators). You also need to override `GetHashCode()` whenever you override `Equals()`, for the same reason as Java: objects that are "equal" must hash to the same bucket, otherwise dictionaries and sets break.

**For `string`**: C# does something convenient that Java famously does not — `==` on strings compares their contents, not their references. `"hello" == "hello"` is `true` in C# even if they are different string objects. Java requires `"hello".equals("hello")` for that.

**Back to this code**: `Choice` is an enum, so it is a value type. `player == computer` is value comparison. No `Equals()` override needed. The compiler knows exactly what to do.

For everything else, you need to enumerate which combinations are wins for the player:

- Rock beats Scissors
- Paper beats Rock
- Scissors beats Paper

That is exactly three winning combinations. Everything else is a loss. A switch expression is a clean way to express this:

```csharp
return (player, computer) switch
{
    (Choice.Rock,     Choice.Scissors) => Outcome.Win,
    (Choice.Paper,    Choice.Rock)     => Outcome.Win,
    (Choice.Scissors, Choice.Paper)    => Outcome.Win,
    _                                  => Outcome.Lose,
};
```

The `(player, computer)` is a **tuple** — a temporary grouping of two values. You are matching against pairs. The `_` wildcard matches everything that did not match the three winning cases.

### Trace through three combinations by hand

Before moving on, trace through the logic with three examples. The goal is to confirm you understand what happens before you run it.

**Example 1: player = Rock, computer = Scissors**

1. Are they equal? `Rock == Scissors`? No. Skip the draw.
2. Does `(Rock, Scissors)` match the first arm? Yes. Return `Outcome.Win`.

Result: Win. Correct.

**Example 2: player = Paper, computer = Paper**

1. Are they equal? `Paper == Paper`? Yes. Return `Outcome.Draw`.

Result: Draw. Correct.

**Example 3: player = Rock, computer = Paper**

1. Are they equal? `Rock == Paper`? No. Skip the draw.
2. Does `(Rock, Paper)` match `(Rock, Scissors)`? No.
3. Does `(Rock, Paper)` match `(Paper, Rock)`? No.
4. Does `(Rock, Paper)` match `(Scissors, Paper)`? No.
5. Does it match `_`? Yes. Return `Outcome.Lose`.

Result: Lose. Correct — paper beats rock.

If you can trace through the logic like this, you understand it. You do not need to run the program to check.

## Step 3: Design PickComputerChoice

This method takes no inputs and returns a `Choice`. Its only job is to pick one of the three options.

The signature:

```csharp
private Choice PickComputerChoice()
```

The implementation uses `Rng.NextBool()`, which you have in the event context. `NextBool()` returns `true` or `false` with roughly equal probability.

```csharp
private Choice PickComputerChoice()
{
    if (Rng.NextBool()) return Choice.Rock;
    if (Rng.NextBool()) return Choice.Paper;
    return Choice.Scissors;
}
```

### The probability bias — and why it is fine

This method is not perfectly fair. Work through the probabilities:

- First `NextBool()`: 50% chance of `true` -> return `Rock`. 50% chance of `false` -> continue.
- Second `NextBool()`: of the 50% who reached here, 50% chance of `true` -> return `Paper`. 50% chance of `false` -> continue.
- Fall through: return `Scissors`.

So:
- Rock: 50%
- Paper: 25%
- Scissors: 25%

Rock is picked twice as often as the others. In a tournament that would matter. In an in-game event where the player encounters it once or twice, it is barely noticeable — and it has the virtue of being simple code that you can understand completely right now.

Lesson 9 covers proper uniform random selection. For now, this is good enough.

The important thing is that you can see the bias and name it. "Good enough for now, with a known limitation" is a legitimate engineering decision. What you want to avoid is not knowing about the bias — that would be a bug. Knowing about it and choosing to leave it is a tradeoff.

## Step 4: Compose them in Play

You have `DetermineOutcome` and `PickComputerChoice`. Now you need a method that:

1. Calls `PickComputerChoice` to get the computer's pick.
2. Calls `DetermineOutcome` to get the result.
3. Reacts to the result.

```csharp
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
```

`Play` does not contain any game logic. It delegates the hard parts to the other two methods, then handles the event-system consequences. This is a pattern you will see everywhere in well-written code: orchestrators that call workers.

Notice `async Task` instead of `void`. `PlayerCmd.GainGold` is an asynchronous operation — the game needs to animate the gold gaining and wait for it to finish before continuing. `async Task` is how C# handles that. You do not need to understand async deeply yet; just know that when a method uses `await`, its return type must be `Task` rather than `void`.

## The design, summarized

Before writing any event system code at all, you now have:

| Method | Input | Output | Job |
|--------|-------|--------|-----|
| `PickComputerChoice` | nothing | `Choice` | Pick randomly |
| `DetermineOutcome` | two `Choice` values | `Outcome` | Apply rules |
| `Play` | one `Choice` (player's) | nothing | Orchestrate |

Each method does one thing. You can reason about each one separately. You can test `DetermineOutcome` without running the whole game.

That is the value of breaking a problem into methods.

## Vocabulary

**Enum** — A type that defines a fixed set of named values. `Choice.Rock` is more readable and safer than `0` or `"Rock"`.

**Tuple** — A lightweight grouping of values used inline. `(player, computer)` in the switch expression is a tuple.

**Orchestrator** — A method whose job is to call other methods and coordinate their results, rather than computing things itself.

**Probability bias** — When a random selection does not give each option an equal chance.

**Good enough for now** — A deliberate decision to accept a known limitation because fixing it would add complexity not yet worth the cost.

## Things to look up

- "C# enum" — how to define and use enums
- "C# tuple" — the full syntax for creating and destructuring tuples
- "C# switch expression" — the modern syntax used in `DetermineOutcome`
- "C# async await" — the full explanation of asynchronous programming; not required now but useful later
