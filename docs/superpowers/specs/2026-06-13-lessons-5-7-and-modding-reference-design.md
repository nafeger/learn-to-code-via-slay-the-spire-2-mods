# Lessons 5–7 + Modding Reference — Design

**Date:** 2026-06-13
**Status:** Approved (brainstorm)
**Author:** Nate Feger (with Claude)

## Summary

Fill out the three "Coming soon" lessons — 5 (Classes & Objects), 6 (Collections),
and 7 (Interfaces & Inheritance) — by growing the Rock Paper Scissors event from
Lesson 4 into a best-of-3 match against an opponent that **biases its pick using the
human's last couple of throws**. Add a root-level **modding capabilities reference**
that maps the full StS2 modding surface. Fix a localization-key bug that Lesson 4
currently teaches.

The guiding goal (from the user): *learn about all that is possible with modding Slay
the Spire 2.* The RPS opponent is the vehicle; the reference is the map.

## Motivation

- Lessons 5, 6, 7 are stubs (`> Coming soon.`).
- The RPS work Jack started on `origin/jf/learning` is incomplete: `StarterContent.cs`
  registers `RockPaperScissors`, but **no `RockPaperScissors.cs` is committed** (his
  commit message: "didn't properly commit it"). The branch does not compile as-is.
- Lesson 4 teaches localization keys prefixed `RPS.*`, but ModSmith derives the key
  namespace from the **class name in SCREAMING_SNAKE_CASE** — so `RockPaperScissors`
  must use `ROCK_PAPER_SCISSORS.*` (mirroring `TheGoldCoinRoom` → `THE_GOLD_COIN_ROOM`).
  This is the exact gotcha the user hit. Lesson 4 also mis-explains the placeholder
  text as "PCK not bundled yet" when the deeper cause is the key-name mismatch.

## The narrative spine

**Before (end of Lesson 4):** one class, all methods, computer picks randomly
(`PickComputerChoice()` is biased coin flips), single throw with a draw re-roll loop.

**After (end of Lesson 7):** a best-of-3 match with a visible W/L/D scoreboard against
an opponent that remembers recent throws and biases its pick to counter the player —
and a learner who understands why the mod framework is shaped the way it is.

The through-line: **the scoreboard's history (L5/L6) becomes the adaptive pick's input
(L6).** You collect the record to show off your score, and that same record is what
lets the computer get smarter. No AI, no new probability math — just reuse the weighted
distribution idea from Lesson 2, fed by recent-throw counts.

## Deliverables

### 1. Modding capabilities reference — `MODDING.md` (repo root)

A standalone, durable "what can a modder make, and what are the conventions" map.
Every entry anchored to a real example in `jacksmod-real-/JacksMod/src/StarterContent/`.

Sections:

| Section | Content | Anchored to |
|---|---|---|
| Content types | Cards, Relics, Powers, Potions, Ancients, Events — what each is + its `ModSmith*Model` base class | `CoinFlip`, `GoldArmor`, `MadeOfGold`, `GoldPaint`/`DropOfGold`, `GoldGuy`, `TheGoldCoinRoom` |
| Registration | Each artifact must be registered; `Registry.Register*<>()` patterns | `StarterContent.cs` |
| Overriding behavior | Hooks the framework calls: `OnPlay`, `OnUpgrade`, `AfterSideTurnStart`, `GenerateInitialOptions`, `CanonicalVars` | the starter classes |
| DynamicVars | `GoldVar`, `BlockVar`, value substitution into text | `CoinFlip`, `GoldArmor` |
| Localization conventions | **SCREAMING_SNAKE_CASE-from-class-name** rule (the gotcha), per-type JSON files, key structure | `localization/eng/*.json` |
| Build & assets | `dotnet publish`, ModSmith, `has_pck`, MegaDot/PCK export | `JacksMod.csproj`, manifest |
| What this means for you | The meta-point: card, relic, **enemy** — same shape (extend a base, override hooks, register, localize). On-ramp to "more artifacts and enemies." | — |

Relationship to Lesson 7: Lesson 7 is the *narrative* ("why the framework works this
way"); `MODDING.md` is the *lookup* version. They cross-link.

### 2. Lesson 5 — Classes & Objects · "What IS an opponent?"

Extract the static `PickComputerChoice()` into a real `RpsOpponent` **class**: fields
(its memory), a constructor, instance methods (`Pick()`, `Remember(throw)`). The event
*has an* opponent object that lives across the whole match and accumulates state.

- **Teaches:** class vs object, fields, constructor, `this`, instance state vs static.
  ("A card is an object" → "the opponent is an object.")
- **In-game payoff:** RPS becomes **best-of-3** with a running **W/L/D score line** (the
  scoreboard). The opponent is now an object — still picking randomly at this stage.

### 3. Lesson 6 — Collections · bias the pick from recent throws

Give the opponent a `List<Choice>` of the player's recent throws and tally them (a
`Dictionary<Choice,int>` or counters). The pick weights toward the counter of the
player's most-frequent recent move, over a fixed-size "recent window."

- **Teaches:** lists, dictionaries, iterating to compute a tally, fixed-size windows.
- **In-game payoff:** the computer visibly **adapts** — spam Rock and it starts
  throwing Paper. This is the user's "use the last couple throws to influence the
  distribution of our pick."

### 4. Lesson 7 — Interfaces & Inheritance · "Why the framework works the way it does"

Two strands that meet:

1. **Your own code:** make opponent behavior swappable — a base `RpsOpponent` (random)
   and a derived `override` adaptive opponent (or an `IOpponent` interface). One line
   swaps difficulty.
2. **The framework:** `ModSmithEventModel`, `override GenerateInitialOptions`,
   `Registry.RegisterEvent<>`, and **the loc-key naming convention** — the framework
   reflects over your class name to build the key namespace and to call your overrides.
   This is *why* `RPS.*` failed and `ROCK_PAPER_SCISSORS.*` works.

- **In-game payoff:** swap opponent difficulty in one line; loc text renders correctly.
- **Closing tease:** a custom card, relic, or **enemy** is the same pattern — extend a
  framework base, override its hooks. Links to `MODDING.md`. This is Jack's next goal.

### 5. Cleanup (prerequisite, part of this work)

- **Fix Lesson 4 loc keys** `RPS.*` → `ROCK_PAPER_SCISSORS.*` across `04-the-event.md`
  and `05-localization.md`, and correct the misleading "it's just PCK" explanation —
  point forward to Lesson 7 for the real *why* (key derived from class name).
- **Resolve the dangling registration:** ensure a committed, compiling single-round RPS
  baseline (`RockPaperScissors.cs` + matching `ROCK_PAPER_SCISSORS.*` loc entries)
  exists as the "before" state Lesson 5 builds on.

## Lesson structure (follows existing convention)

Each lesson is a folder with a `README.md` (overview + success criteria + file list)
and numbered chapter files (`01-*.md`, `02-*.md`, …), ending with a "what you built"
in-game payoff and a Vocabulary + "Things to look up" section — matching Lessons 1–4.

## Scope / YAGNI

- **No AI / ML.** Bias = weighted distribution (reuse Lesson 2), not learning.
- **No cross-run persistence.** Scoreboard/history lives within a single event visit.
- **No new probability math.** Lesson 9 still owns uniform-RNG correctness.
- **Reference is example-anchored, not exhaustive API docs.** It maps what exists in
  `StarterContent` + the conventions, not every class in `sts2.dll`.

## Open questions

- Exact "recent window" size for the bias (last 2? last 3?) — a Lesson 6 detail, decided
  during planning.
- Whether L7 uses an `interface` (`IOpponent`) or `abstract`/`virtual` inheritance as the
  primary teaching vehicle — both are in scope for the lesson; pick the clearer one when
  writing it.

## References

- Jack's branch: `origin/jf/learning` in `jacksmod-real-`
- Starter content: `jacksmod-real-/JacksMod/src/StarterContent/`
- Lesson 4 (the "before"): `lessons/lesson-04-methods/04-the-event.md`
