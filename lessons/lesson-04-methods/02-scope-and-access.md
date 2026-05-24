# 02 — Scope and Access

## Scope: where a variable lives

Every variable you declare has a **scope** — the region of code where it exists and can be used. Once execution leaves that region, the variable is gone.

The most important rule: a variable declared inside a method only exists inside that method.

```csharp
private int Double(int x)
{
    int result = x * 2;  // result exists here
    return result;
}

private void SomeOtherMethod()
{
    // result does NOT exist here
    // Console.WriteLine(result);  // this would be a compile error
}
```

`result` is a **local variable**. It is created when `Double` is called, used inside `Double`, and destroyed when `Double` returns.

This is not a limitation — it is a feature. Local variables cannot be accidentally read or changed by code in other methods. Each method gets its own clean workspace.

## Parameters are local too

Parameters work the same way. They exist only for the duration of the method call.

```csharp
private Outcome DetermineOutcome(Choice player, Choice computer)
{
    // player and computer exist here
    if (player == computer) return Outcome.Draw;
    // ...
}
```

`player` and `computer` are local to `DetermineOutcome`. The `Play` method has its own variables named `playerChoice` and `computerChoice`. They are different variables in different scopes — it does not matter that they hold the same values.

## Passed by value vs by reference

When you pass a parameter to a method, what exactly gets handed over?

For **value types** (like `int`, `bool`, `Choice`, `Outcome`, or any `struct`), C# copies the value. The method gets its own copy. If the method modifies the parameter, the original is unchanged:

```csharp
private void AddTen(int number)
{
    number += 10;  // modifies only the local copy
}

int gold = 50;
AddTen(gold);
// gold is still 50
```

For **reference types** (like classes — `Player`, `List<int>`, most objects), the variable holds a reference (essentially a pointer) to an object in memory. C# copies the reference, not the object. If the method modifies the object through the reference, the caller sees the change:

```csharp
private void AddCard(List<int> hand, int cost)
{
    hand.Add(cost);  // modifies the actual list object
}

List<int> myHand = new List<int> { 1, 2, 3 };
AddCard(myHand, 0);
// myHand now contains { 1, 2, 3, 0 }
```

In the Rock Paper Scissors event, `Choice` and `Outcome` are `enum` types, which are value types. Passing `playerChoice` to `DetermineOutcome` copies the value — the method can't accidentally modify the caller's copy.

The rule of thumb: **primitives and enums are safe to pass freely**. Objects and lists can be modified by the methods you pass them to, so be aware of that when you see reference types as parameters.

## Block scope

Scope can be even narrower than a method. A variable declared inside an `if` block only exists inside that block:

```csharp
private void Example(bool condition)
{
    if (condition)
    {
        int x = 10;  // x only exists inside this if block
        Console.WriteLine(x);
    }
    // Console.WriteLine(x);  // compile error — x is out of scope
}
```

You will see this matter most with `for` loops: the loop variable (`int i`) only exists inside the loop.

## Private vs public

Every method (and field, and class) has an **access modifier** that controls who can call it.

The two you need right now:

**`private`** — Only code inside the same class can call or use it.

**`public`** — Any code anywhere can call or use it.

Here is a concrete example. The `RockPaperScissors` class has:

```csharp
public sealed class RockPaperScissors : ModSmithEventModel
{
    private Choice PickComputerChoice() { ... }
    private Outcome DetermineOutcome(Choice player, Choice computer) { ... }
    private async Task Play(Choice playerChoice) { ... }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() { ... }
}
```

`PickComputerChoice`, `DetermineOutcome`, and `Play` are all `private`. They are internal helpers. No other class needs to call them. Keeping them private means if you ever change how they work — say, you improve `PickComputerChoice` in Lesson 9 — nothing outside this class breaks, because nothing outside this class can use them.

`GenerateInitialOptions` is `protected override`, which you will learn more about in Lesson 5. For now: it has to be accessible to the parent class `ModSmithEventModel`, which is why it is not private.

## Why private is the right default

When you write a new method, ask: does anything outside this class need to call this?

The answer is almost always no. Start with `private`. If you later discover something external needs access, you can change it to `public`. Going the other direction — making something `public` and then finding out you need to hide it — is harder, because other code may already be depending on it.

**Private by default** is a real principle that experienced developers follow. It is called minimizing surface area or the principle of least privilege.

## The sealed keyword

The class declaration in the mod is:

```csharp
public sealed class RockPaperScissors : ModSmithEventModel
```

`sealed` means this class cannot be used as a base class. No one can write `class MyThing : RockPaperScissors`. It is a signal that says: this class is complete as-is, it is not designed to be extended.

For mod events, sealed is appropriate because you are not trying to build an inheritance hierarchy. You just want one self-contained event.

## The this keyword

Inside any method, the keyword `this` refers to the current instance of the class — the specific object the method is running on.

You can see it in `GenerateInitialOptions`:

```csharp
new EventOption(this, () => Play(Choice.Rock), "RPS.options.ROCK")
```

`this` is passed as the first argument to `EventOption`. It tells the option: the event that owns you is this one.

You will use `this` explicitly when there is a naming conflict or when you need to pass the current object somewhere. In most other cases, it is implied. When you write `PickComputerChoice()` inside another method of the same class, you are implicitly calling `this.PickComputerChoice()`.

## How the private helpers call each other

Because `PickComputerChoice`, `DetermineOutcome`, and `Play` are all private methods on the same class, they can call each other freely:

```csharp
private async Task Play(Choice playerChoice)
{
    Choice computerChoice = PickComputerChoice();       // calls another private method
    Outcome outcome = DetermineOutcome(playerChoice, computerChoice);  // and another

    // ... handle outcome
}
```

`Play` delegates to `PickComputerChoice` and `DetermineOutcome` rather than doing all the work itself. Each method stays focused on one job. `Play` orchestrates; the others compute.

## Vocabulary

**Scope** — The region of code where a variable exists and can be referenced.

**Local variable** — A variable declared inside a method or block. It exists only for the duration of that method or block.

**Access modifier** — A keyword (`private`, `public`, `protected`, etc.) that controls what code can see a member.

**`private`** — Accessible only within the declaring class.

**`public`** — Accessible from any code.

**`sealed`** — Applied to a class; prevents other classes from inheriting from it.

**`this`** — A reference to the current instance of the class.

**Surface area** — The set of things a class exposes publicly. Smaller surface area means fewer things other code can depend on.

## Things to look up

- "C# access modifiers" — the full list includes `protected`, `internal`, `protected internal`, and `private protected`
- "C# sealed class" — when and why to use it
- "encapsulation C#" — the broader principle that private methods support
- "C# this keyword" — the full set of things `this` can do
