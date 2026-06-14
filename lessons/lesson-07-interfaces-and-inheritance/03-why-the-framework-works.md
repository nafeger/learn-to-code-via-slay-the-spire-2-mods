# 03 — Why the Framework Works the Way It Does

You just built an event that depends on the `IOpponent` contract and calls methods it does
not implement, letting a concrete class plugged in elsewhere decide the behavior. Hold that
thought — because **the mod framework treats your event exactly the way your event treats
the opponent.** Once you see that, the whole framework stops being magic.

## Your event has been a subclass the whole time

Every event you have written starts with a colon:

```csharp
public sealed class RockPaperScissors : ModSmithEventModel
```

That is inheritance, the tool from chapter 01. `RockPaperScissors` **is a**
`ModSmithEventModel`. It inherits a large amount of machinery from that base class — and the
methods you have been writing are **overrides** of `virtual` methods the base declares:

```csharp
protected override IReadOnlyList<EventOption> GenerateInitialOptions() { /* yours */ }
protected override IEnumerable<DynamicVar> CanonicalVars => [ /* yours */ ];
```

`override`. The same keyword you just used on `AdaptiveOpponent.Pick`. `ModSmithEventModel`
declares these as `virtual` (replaceable), and your subclass replaces them. Everything else
— how an event is shown on screen, how options are rendered, how `SetEventFinished` ends the
encounter, how the page transitions animate — is inherited, written once by the framework
authors, and reused by every event mod in existence. You wrote the few methods that are
*different* about your event and inherited the hundreds that are the *same*.

That is exactly what `AdaptiveOpponent` did to `RandomOpponent`: inherit the common parts,
override the few that differ. You have been on both sides of the same relationship.

## What you get for free

Look at the methods you *call* but never wrote:

- `SetEventFinished(text)` — ends the event
- `SetEventState(text, options)` — replaces the page
- `L10NLookup(key)` — looks up localized text
- `Owner` — the player the event is happening to
- `Rng` — the event's random source
- `DynamicVars` — the dynamic values you declared

None of these are in your file. They are all inherited from `ModSmithEventModel` (and its
own base classes, on up the chain). Inheritance is why a 30-line event class can do so much:
the 30 lines are the difference; the base class is the rest.

## Inversion of control: the framework calls you

Here is the deepest idea, and the one most worth internalizing.

You never call `GenerateInitialOptions` yourself. You never call `OnPlay` on a card. You
*write* them — and the framework calls them, at the moment that makes sense: when the player
walks into the event room, when the card is played. Your code is not the thing in charge
that calls into a library. Your code is the thing the framework *reaches into and calls*.

This is **inversion of control**, sometimes called the Hollywood Principle: "don't call us,
we'll call you." It is the same relationship you just built:

| In your code | In the framework |
|--------------|------------------|
| The event holds an `IOpponent` and calls `Pick()` when *it* decides | The framework holds your event and calls `GenerateInitialOptions()` when *it* decides |
| You wrote `RandomOpponent.Pick`; the event calls it without knowing the concrete type | You wrote `RockPaperScissors.GenerateInitialOptions`; the framework calls it without knowing your concrete type |
| Swap the opponent → event behaves differently, event code unchanged | Register a new event → game behaves differently, framework code unchanged |

The framework authors wrote code that calls `GenerateInitialOptions` on "some
`ModSmithEventModel`," exactly the way your event calls `Pick` on "some `IOpponent`." They
did not know your class would exist. They did not need to. They programmed to the base type;
you supplied the concrete one. That is how a framework lets thousands of modders extend a
game its authors shipped years ago without ever editing the game's code.

## Registration: announcing your concrete type

There is one gap. Your event calls `new AdaptiveOpponent()` directly — it names the concrete
class. But the framework cannot write `new RockPaperScissors()`; it has never heard of your
class. So you have to *tell* it your type exists. That is what registration is:

```csharp
Registry.RegisterEvent<RockPaperScissors>();
```

`RegisterEvent<RockPaperScissors>()` hands the framework your concrete type. Now, when the
game builds its pool of possible events, yours is in it, and the framework can create and
call your event through the `ModSmithEventModel` base type it already knows how to drive.
Registration is the bridge from "a class you wrote" to "a type the framework will call."

This is why unregistered content does nothing, and why your helper classes — `Scoreboard`,
`RandomOpponent`, `AdaptiveOpponent`, `IOpponent` — are *not* registered. The framework only
needs to know about the types *it* will instantiate and call (the event). The opponent is
something *your* event instantiates and calls; the framework never touches it, so it never
needs to hear about it. Registration tracks one boundary: where the framework reaches into
your code.

## The shape behind every content type

Now reread the four steps from [`MODDING.md`](../../MODDING.md) with this lens:

1. **Extend a base class** — inheritance; you get the machinery, you write the difference.
2. **Override the hooks** — `virtual`/`override`; the framework's replaceable methods,
   replaced by you.
3. **Register** — hand your concrete type across the boundary so the framework can call it.
4. **Localize** — supply text under keys derived from your class (next chapter).

A card, a relic, a potion, an event, an enemy: all the same four steps, differing only in
*which* base class and *which* hooks. You do not learn the framework as a thousand features.
You learn one relationship — the same one you built between your event and your opponent —
and apply it everywhere.

## Vocabulary

**Base class machinery** — The inherited implementation a subclass gets for free.

**Inversion of control (Hollywood Principle)** — The framework calls your code at the right
moment; you do not drive it. "Don't call us, we'll call you."

**Hook** — A `virtual` method the framework calls at a defined moment, for you to override.

**Registration** — Handing your concrete type to the framework so it can create and call it
through a base type it understands.

**Boundary / extension point** — The seam where framework code reaches into your code.
Registration marks it; the overrides live on its far side.

## Things to look up

- "inversion of control" — the principle, with examples beyond games
- "framework vs library" — a library you call; a framework calls you. This is the difference
- "template method pattern" — the classic name for "base class with overridable steps"
