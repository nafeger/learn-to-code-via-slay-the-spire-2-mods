# 01 — Interfaces and Inheritance

Two tools, one goal: letting different classes be used interchangeably. They approach it
from different directions. This chapter explains both with small examples; the next two put
them to work.

## The problem they solve

By the end of Lesson 6, `RpsOpponent.Pick` had two behaviors crammed together: a random
fallback and an adaptive counter, chosen with an `if`. Suppose you want a clean "easy mode"
(always random) and "hard mode" (adaptive), and you want the event to not care which one it
is playing against.

What you want is: **one kind of thing — "an opponent" — with more than one implementation,**
and code that talks to "an opponent" without knowing or caring which implementation it got.
That is what interfaces and inheritance give you.

## Interfaces: a contract

An **interface** is a list of methods a class promises to provide, with no implementation —
just the signatures. It is a contract: "anything that is an `IOpponent` can be asked to
`Pick()` and to `Remember(...)`."

```csharp
public interface IOpponent
{
    Choice Pick();
    void Remember(Choice playerThrow);
}
```

No bodies, just signatures. The interface says *what* an opponent can do, not *how*. By
convention, C# interface names start with a capital `I` — `IOpponent`, and in the mod code
you have already seen `IEnumerable`, `IReadOnlyList`, `IHoverTip`. That leading `I` is your
signal "this is an interface — a contract, not a concrete class."

A class **implements** an interface by listing it after a colon and providing every method
it names:

```csharp
public class RandomOpponent : IOpponent
{
    public Choice Pick() { /* ... */ }
    public void Remember(Choice playerThrow) { /* ... */ }
}
```

If `RandomOpponent` left out `Pick` or `Remember`, it would not compile — the contract is
enforced. That is the value: any code holding an `IOpponent` *knows* it can call those two
methods, whatever the concrete class behind it is.

## Polymorphism: one variable, many types

Here is the move that makes interfaces powerful. A variable typed as the interface can hold
*any* implementation:

```csharp
IOpponent opponent = new RandomOpponent();
// ...later, or in a different build:
IOpponent opponent = new AdaptiveOpponent();
```

Code that uses `opponent.Pick()` does not change between those two lines. It calls `Pick()`
on "an opponent"; which `Pick` actually runs depends on the real object behind the
interface. This is **polymorphism** — "many forms": one interface, many concrete behaviors,
selected by which object you created.

You have *been* the beneficiary of this. `GenerateInitialOptions` returns
`IReadOnlyList<EventOption>` — an interface. The framework does not care whether you handed
it a `List`, an array, or something else; it only needs the contract `IReadOnlyList`
guarantees. That is why your `[ ... ]` literal just works.

## Inheritance: building on a base class

**Inheritance** is the other tool. Instead of a contract with no bodies, a class can extend
another class and inherit its actual implemented behavior, then add to or change it.

```csharp
public class RandomOpponent : IOpponent
{
    public virtual Choice Pick() => RandomChoice();
    public virtual void Remember(Choice playerThrow) { }   // random opponent ignores history

    protected Choice RandomChoice() { /* ... */ }
}

public class AdaptiveOpponent : RandomOpponent   // inherits everything RandomOpponent has
{
    public override Choice Pick() { /* use memory instead */ }
    public override void Remember(Choice playerThrow) { /* actually remember */ }
}
```

`AdaptiveOpponent : RandomOpponent` means "an AdaptiveOpponent is a RandomOpponent, plus
changes." It inherits `RandomChoice()` for free and does not rewrite it. Where it wants
different behavior, it **overrides**.

### `virtual` and `override`

These two keywords are a pair:

- **`virtual`** on a base-class method means "a subclass is allowed to replace this."
- **`override`** on a subclass method means "I am replacing the virtual method I inherited."

```csharp
public virtual Choice Pick() => RandomChoice();   // base: replaceable
public override Choice Pick() { /* ... */ }        // subclass: the replacement
```

When you call `Pick()` on an `AdaptiveOpponent` — even through a `RandomOpponent` or
`IOpponent` variable — the **overridden** version runs. The most specific implementation
wins. That is the engine of polymorphism with inheritance.

You have written `override` in every lesson without dwelling on it:
`protected override Task OnPlay(...)`, `protected override IReadOnlyList<EventOption>
GenerateInitialOptions()`. Each one replaces a `virtual` method the base `ModSmith…Model`
provides. Lesson 3 of this lesson set comes back to exactly that.

### `protected`: the third access level

`RandomChoice()` above is `protected`. You know `private` (only this class) and `public`
(anyone). `protected` is the middle: *this class and classes that inherit from it.*
`AdaptiveOpponent` can call the inherited `RandomChoice()` because it is `protected`; code
outside the family still cannot. It is the natural visibility for "a helper meant for
subclasses."

## Interface vs inheritance: which when?

| Use an **interface** when… | Use **inheritance** when… |
|----------------------------|----------------------------|
| You only need to agree on *what* methods exist | You want to *share real implementation* |
| Unrelated classes should be interchangeable | One class is a *specialized version* of another |
| "can do" — a thing that *can be picked from* | "is a" — an Adaptive opponent *is a* kind of opponent |

They combine, as above: `IOpponent` is the contract; `RandomOpponent` and `AdaptiveOpponent`
both honor it; `AdaptiveOpponent` additionally *inherits* from `RandomOpponent` to reuse its
random logic. Contract on the outside, shared code on the inside.

> **A note for Java folks:** this maps almost exactly. C# `interface` ≈ Java `interface`,
> `: BaseClass` ≈ `extends`, `: IOpponent` ≈ `implements`, and `override` is the explicit,
> required form of Java's `@Override`. The big difference: C# makes you mark base methods
> `virtual` before they can be overridden — in Java methods are virtual by default. C# is
> "opt in to being overridable"; Java is "opt out."

## Vocabulary

**Interface** — A contract: a set of method signatures, no bodies, that implementing classes
must provide. Named with a leading `I`.

**Implement** — To provide the methods an interface requires (`class X : IThing`).

**Inheritance** — Building a class on a base class with `: BaseClass`, gaining its members.

**`virtual` / `override`** — Mark a base method replaceable, and replace it in a subclass.

**Polymorphism** — One interface or base type standing in for many concrete types, with the
real object deciding which behavior runs.

**`protected`** — Visible to a class and its subclasses, but not to outside code.

## Things to look up

- "C# interface vs abstract class" — a related third option between the two tools here
- "C# virtual override new" — the keywords and a subtle one (`new`) to avoid for now
- "Liskov substitution principle" — the rule that makes "is a" inheritance trustworthy
