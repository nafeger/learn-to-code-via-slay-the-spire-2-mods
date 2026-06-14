# 05 — Your Turn, and What's Next

Two exercises, then a map of where the framework can take you.

---

## Exercise A: A third opponent

You have `RandomOpponent` (easy) and `AdaptiveOpponent` (hard). Add a third strategy of your
own, as a class implementing `IOpponent` (or inheriting `RandomOpponent`). Ideas:

- **`MirrorOpponent`** — throws whatever you threw *last* round. (Inherit `RandomOpponent`,
  override `Remember` to store just the last throw, and `Pick` to return it — or its
  counter.)
- **`StubbornOpponent`** — picks one throw at construction and always plays it. (How would
  you make it *unbeatable to ignore* but easy to counter once spotted?)

Wire it into the event with a one-line change to the field initializer. The fact that this
is a one-line change — that the event needs no other edit — is the whole point of programming
to the `IOpponent` interface. If you find yourself editing `Play` to add your opponent, stop:
the design is telling you the interface should have covered it.

---

## Exercise B: Explain it to someone

The real test of Lesson 7 is whether you can explain it without notes. Write a few sentences,
or say them out loud, answering:

1. Your event is `class RockPaperScissors : ModSmithEventModel`. In plain words, what does
   that colon give you, and what do you still have to write yourself?

2. You never call `GenerateInitialOptions` — the framework does. What is that relationship
   called, and where did you build the *same* relationship in your own code this lesson?

3. A friend's custom relic shows `MY_RELIC` on screen instead of its name. Their class is
   `MyShinyRelic`. What is wrong, and what should the key prefix be? How do you *know*
   without running it?

If you can answer all three, you understand inheritance, inversion of control, and the
localization rule — the three ideas this lesson set out to teach.

---

## What's next: the whole modding surface is open now

You set out to learn what is possible with Slay the Spire 2 modding. Here is the honest
answer: **you now know the one pattern that everything is built from.** Every kind of content
is the same four moves you have done by hand —

1. Extend a `ModSmith…Model` base class.
2. Override its hooks.
3. Register your type.
4. Localize with a class-name-derived key prefix.

The companion reference, [`MODDING.md`](../../MODDING.md), is your map. It lists every content
type with a working starter example to copy. Read it now — it will make far more sense than
it would have before this lesson, because you understand *why* each entry has the shape it
does.

Concrete next builds, roughly easiest to hardest:

- **A custom card.** Copy `CoinFlip`. Override `OnPlay` with your effect, declare your
  `CanonicalVars`, register it into a pool, localize under `YOUR_CARD_NAME`. Same four moves,
  new base class (`ModSmithCardModel`) and new hook (`OnPlay`).
- **A custom relic.** Copy `GoldArmor`. Pick a combat hook to override (`AfterSideTurnStart`,
  and there are many others), and you have a passive item that reacts to the fight.
- **A custom potion or power.** Copy `DropOfGold` or `MadeOfGold`. Same pattern again.
- **A custom enemy.** This is where you said you wanted to go. An enemy is a bigger version
  of the same idea — a class extending a creature/encounter base, overriding hooks for its
  turn behavior and intents, registered into the game's encounter tables, localized under its
  derived name. More hooks, more moving parts, but not a new *kind* of thing to learn. It is
  the pattern you already know, scaled up.

That is the answer to "what is possible": as much as you are willing to build, because the
framework is not a wall of separate features to memorize. It is one shape — extend, override,
register, localize — repeated. You have done it, by hand, for an event with a scoreboard and
an adaptive opponent. Everything else is a variation on what you have already done.

---

## Where the curriculum goes

- **Lesson 8: Debugging** — You got a taste in chapter 04: reading what the program actually
  says (the raw loc key) to tell two problems apart. Lesson 8 makes that a discipline, with
  real tools — breakpoints, stepping, inspecting state — so you can break things on purpose
  and fix them with confidence.
- **Lesson 9: Probability & Randomness** — The "good enough for now" RNG bias you have
  accepted since Lesson 4 finally gets done properly, with the math of expected value behind
  the game's systems.
- **Lesson 10: Capstone** — Design and ship a piece of content from scratch. By then, the
  four moves will be second nature.

## Vocabulary

**Strategy (as a class)** — A behavior packaged as its own class behind a shared interface,
swappable without changing the code that uses it. Your opponents are strategies.

**Extension point** — A place a framework invites you to plug in: a base class to extend, a
hook to override. The whole framework is a set of these.

**The four moves** — Extend, override, register, localize. The repeating shape of all Slay
the Spire 2 content.

## Things to look up

- "strategy pattern" — the formal name for swappable behaviors behind an interface
- "open closed principle" — open to extension (new opponents), closed to modification (the
  event does not change). You just lived it
- The starter content in `JacksMod/src/StarterContent/` — read every file now; you can
