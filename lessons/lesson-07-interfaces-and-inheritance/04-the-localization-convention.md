# 04 — The Localization Convention, Explained at Last

Back in Lesson 4 you were told: localization keys must start with your class name in
SCREAMING_SNAKE_CASE, so `RockPaperScissors` needs `ROCK_PAPER_SCISSORS.options.ROCK`, and
`RPS.options.ROCK` is permanently broken. You took it on faith. Now you have the concepts to
understand *why*, and it is a direct consequence of the inheritance and inversion-of-control
ideas from the last chapter.

## The bug, restated

If your event class is `RockPaperScissors` but your localization keys look like:

```json
{
  "RPS.options.ROCK.title": "Rock"
}
```

the game shows `RPS.options.ROCK` on the button instead of "Rock." Renaming the keys to:

```json
{
  "ROCK_PAPER_SCISSORS.options.ROCK.title": "Rock"
}
```

fixes it. This is not a packaging problem and not bad luck — it is the framework following a
rule. Here is the rule.

## Every event has an identity, derived from its class name

When the framework loads your event, it needs a stable name for it — an **identity** — to
organize everything that belongs to it: its title, its option text, its page descriptions.
It does not make you declare that identity. It *derives* it, automatically, from your class
name, by converting PascalCase to SCREAMING_SNAKE_CASE:

| Your class name | Derived identity |
|-----------------|------------------|
| `RockPaperScissors` | `ROCK_PAPER_SCISSORS` |
| `TheGoldCoinRoom` | `THE_GOLD_COIN_ROOM` |
| `CoinFlip` | `COIN_FLIP` |

The conversion is mechanical: find the word boundaries (each capital letter starts a new
word), upper-case everything, join with underscores. `RockPaperScissors` → `ROCK` + `PAPER`
+ `SCISSORS` → `ROCK_PAPER_SCISSORS`.

This is inheritance and inversion of control at work. The base `ModSmithEventModel` contains
the logic that computes this identity from the class's name — your subclass inherits it. You
do not call that logic; the framework runs it for you when it loads your event. You never
see it happen, which is exactly why the rule felt like a mystery: the code enforcing it lives
in the base class, not in your file.

> You have already glimpsed this. `CoinFlip` writes `$"{base.Id.Entry}.headsBanter"` — it
> reads its own framework-assigned identity (`base.Id.Entry`) and builds a key from it,
> instead of hard-coding a prefix. That is the same identity the event system derives from
> the class name. The framework knows what your content is called, because it named it after
> your class.

## Why the lookup fails with the wrong prefix

When the game needs the text for an option, it asks: "what is this event's identity, and
what is under that identity for this option?" For your event the identity is
`ROCK_PAPER_SCISSORS`. It then looks up the key you gave the `EventOption`:

```csharp
new EventOption(this, () => Play(Choice.Rock), "ROCK_PAPER_SCISSORS.options.ROCK")
```

and finds the matching entry in the localization dictionary (Lesson 6's idea — keys to
text). Match found → "Rock" is displayed.

If your key said `RPS.options.ROCK`, the lookup still runs, but there is no entry with that
key in the data the game loaded for this event's identity. No match → the
`MissingLocPatch` falls back to showing the raw key. The framework is not "wrong"; it looked
exactly where it should and found nothing, because you filed the text under a name that does
not match the event's derived identity. The class is named `RockPaperScissors`; its mailbox
is `ROCK_PAPER_SCISSORS`; you left the letter at `RPS`; it never gets read.

## What is enforced and what is free

Only the **first segment** is the rule. Everything after it is structure *you* design:

```
ROCK_PAPER_SCISSORS . options . ROCK . title
└──── enforced ────┘ └────────── your choice ──────────┘
   = class name
   (SCREAMING_SNAKE)
```

`ROCK_PAPER_SCISSORS` must match the class. But `.options.ROCK.title` versus
`.pages.WIN.description` versus any other arrangement is a convention you pick for your own
readability — the framework just needs the full string you pass to `L10NLookup` or
`EventOption` to exist as a key in the file. Look at `TheGoldCoinRoom`'s keys
(`THE_GOLD_COIN_ROOM.pages.INITIAL.options.TAKE.title`) versus this event's
(`ROCK_PAPER_SCISSORS.options.ROCK.title`) — different sub-structures, same enforced first
segment, both correct.

## The rule for every content type

This is not special to events. The same class-name-to-identity derivation drives cards,
relics, powers, potions, and ancients — each in its own localization file. Rename the class,
and you must rename the keys to match, in lock-step.

| Content class | Loc file | Required key prefix |
|---------------|----------|---------------------|
| `CoinFlip` | `cards.json` | `COIN_FLIP` |
| `GoldArmor` | `relics.json` | `GOLD_ARMOR` |
| `MadeOfGold` | `powers.json` | `MADE_OF_GOLD` |
| `TheGoldCoinRoom` | `events.json` | `THE_GOLD_COIN_ROOM` |
| `RockPaperScissors` | `events.json` | `ROCK_PAPER_SCISSORS` |

[`MODDING.md`](../../MODDING.md) records this rule in its localization section, because it is
the single convention that trips up the most new modders. Now you know not just the rule but
the reason: the framework derives an identity from your class name and files all your text
under it, so your keys must use that same derived name.

## A debugging takeaway

The raw key on screen is a *gift*, not just an annoyance. It tells you the exact string the
game tried to find:

- See `ROCK_PAPER_SCISSORS.options.ROCK` on the button? The key is right; the text is just
  not packaged into the PCK yet. Harmless, expected before asset export.
- See `RPS.options.ROCK`? The prefix is wrong; this would fail forever, packaged or not. Fix
  the key to match the class name.

Two problems that look identical on screen, told apart by reading the key. This is your first
taste of debugging by reading what the program actually says — which is exactly where Lesson
8 goes next.

## Vocabulary

**Content identity** — The stable name the framework derives from a content class's name
(`RockPaperScissors` → `ROCK_PAPER_SCISSORS`) and uses to organize its localized text.

**PascalCase → SCREAMING_SNAKE_CASE** — The mechanical conversion: split on capitals,
upper-case, join with underscores.

**Enforced prefix** — The first key segment, which must equal the content identity. The rest
of the key is your convention.

**Fallback (MissingLocPatch)** — Showing the raw key when no matching text is found, instead
of crashing.

## Things to look up

- "naming convention case styles" — PascalCase, camelCase, snake_case, SCREAMING_SNAKE_CASE
- "convention over configuration" — the design philosophy of deriving behavior from names
- "reflection in C#" — how a framework can inspect a class's name at runtime to do this
