# What You Can Build: A Modding Reference

This is a map of what a Slay the Spire 2 mod can add to the game, and the conventions
every piece of content follows. It is a reference, not a tutorial — skim it to learn
what is *possible*, then come back to look things up while you build.

Everything here is anchored to a real, working example in the `JacksMod` starter
project (`src/StarterContent/`). When a section says "see `CoinFlip`," that file exists
and compiles. The starter content is, deliberately, one small example of every kind of
thing you can make.

> The lessons build one of these — an **event** (Rock Paper Scissors) — in depth.
> This reference is the wider landscape that event sits inside.

---

## The big picture

A mod is a C# class library that the game loads at startup. Your code runs *inside* the
game and adds content to it. The flow is always the same four steps, no matter what you
are adding:

1. **Extend a base class** that the framework provides (`ModSmithCardModel`,
   `ModSmithRelicModel`, …).
2. **Override the hooks** the framework calls — `OnPlay`, `AfterSideTurnStart`,
   `GenerateInitialOptions`, and so on. This is where your behavior lives.
3. **Register** the content so the game knows it exists (`Registry.RegisterCard<…>()`).
4. **Localize** the player-facing text in a JSON file, using keys derived from your
   class name.

If you understand those four steps for one content type, you understand them for all of
them. A card, a relic, and an enemy are the same shape — only the base class and the
hooks differ.

The framework that makes this possible is **ModSmith**, layered on top of the game's own
`sts2.dll` and the **Harmony** patching library. Your mod depends on ModSmith (declared
in `JacksMod.json`).

---

## What you can add

Every content type is a class that extends a `ModSmith…Model` base class. Here is the
full set in the starter project:

| Content type | Base class | Starter example | What it is |
|---|---|---|---|
| **Card** | `ModSmithCardModel` | `cards/CoinFlip.cs` | A playable card with a cost, type, rarity, and an `OnPlay` effect |
| **Relic** | `ModSmithRelicModel` | `relics/GoldArmor.cs` | A passive item that reacts to combat events |
| **Power** | `ModSmithPowerModel` | `powers/MadeOfGold.cs` | A buff/debuff applied to a creature that reacts to game events |
| **Potion** | `ModSmithPotionModel` | `potions/DropOfGold.cs`, `potions/GoldPaint.cs` | A one-shot consumable with an `OnUse` effect |
| **Event** | `ModSmithEventModel` | `events/TheGoldCoinRoom.cs` | A "?" room with text and choices |
| **Ancient event** | `ModSmithAncientEventModel` | `ancients/GoldGuy.cs` | A special event that offers a choice of relics |

These six cover the bulk of what mods add. The same pattern extends to other content the
game supports (enemies, encounters, character classes); they are not in the starter
project, but they follow the identical extend-override-register-localize shape.

---

## Step 1: Extend a base class

You declare a class and inherit from the matching base. The base class gives you a large
amount of behavior for free; you only write what is different.

```csharp
// A card — see cards/CoinFlip.cs
class CoinFlip : ModSmithCardModel
{
  public CoinFlip()
    : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }
  // ...
}

// A relic — see relics/GoldArmor.cs
public sealed class GoldArmor : ModSmithRelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;
  // ...
}
```

The constructor and overridable properties are how you declare the content's basic
identity — a card's cost and rarity, a relic's rarity, a potion's usage rules.

---

## Step 2: Override the hooks

The framework decides *when* things happen; your overrides decide *what* happens. You do
not call these methods — the game calls them, at the right moment. This is the most
important idea in the whole framework (the lessons call it "inversion of control").

Common hooks, by content type:

| Hook | Where | Fires when |
|---|---|---|
| `OnPlay(...)` | Card | The card is played — see `CoinFlip` |
| `OnUpgrade()` | Card | The card is upgraded — see `CoinFlip` |
| `OnUse(...)` | Potion | The potion is drunk — see `DropOfGold` |
| `AfterSideTurnStart(...)` | Relic | A combat side begins its turn — see `GoldArmor` |
| `AfterDamageReceived(...)` | Power | The owner takes damage — see `MadeOfGold` |
| `GenerateInitialOptions()` | Event | The event room is entered — see `TheGoldCoinRoom` |

Example — a relic that reacts every turn:

```csharp
// relics/GoldArmor.cs — the framework calls this at the start of each side's turn
public override async Task AfterSideTurnStart(
    CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
{
  if (side == Owner.Creature.Side && Owner.Gold >= DynamicVars.Gold.IntValue)
  {
    Flash();
    await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner);
    await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Unpowered, null);
  }
}
```

Most effects are driven through **command** helpers — `PlayerCmd.GainGold`,
`PlayerCmd.LoseGold`, `CreatureCmd.GainBlock`, `PowerCmd.Apply<T>`. These are
asynchronous (`await`), because the game animates them and waits for the animation to
finish.

---

## Step 3: Register the content

A class that is not registered does not exist as far as the game is concerned.
Registration happens once at startup. See `StarterContent.cs`:

```csharp
public static void RegisterStarterContent()
{
  Registry.RegisterPotion<DropOfGold>();
  Registry.RegisterCard<CoinFlip, ColorlessCardPool>();   // cards say which pool they join
  Registry.RegisterRelic<GoldArmor>();
  Registry.RegisterPotion<GoldPaint>();
  Registry.RegisterPower<MadeOfGold>();
  Registry.RegisterEvent<TheGoldCoinRoom>();
  Registry.RegisterEvent<RockPaperScissors>();
  Registry.RegisterAncientEvent<GoldGuy, Hive>();          // ancient events name their act
}
```

Note that some registrations take a second type argument — a card names the **pool** it
belongs to, an ancient event names where it appears. The registration call is where you
tell the game *how this content enters a run*.

---

## Dynamic values: `CanonicalVars`

Numbers that appear in player-facing text — "Gain **10** gold," "Gain **10** Block" —
are declared as **dynamic vars** rather than hard-coded into the text. This keeps the
number in one place (the C# code) and lets the localized text reference it by name.

```csharp
// cards/CoinFlip.cs
protected override IEnumerable<DynamicVar> CanonicalVars => [
  new GoldVar(BASE_GOLD),
];
```

```csharp
// relics/GoldArmor.cs — a relic can declare several
protected override IEnumerable<DynamicVar> CanonicalVars => [
  new GoldVar(Price),
  new BlockVar(Block, ValueProp.Unpowered),
];
```

You then read the live value in code as `DynamicVars.Gold.IntValue`, and reference it in
localized text as `{Gold}`. Common vars: `GoldVar`, `BlockVar`, `PowerVar<T>`. The
`CanonicalVars` list is also how upgrades work — `OnUpgrade` calls
`DynamicVars.Gold.UpgradeValueBy(n)`.

---

## Localization: the naming convention that bites everyone

All player-facing text lives in JSON files under `JacksMod/localization/eng/`, one file
per content category:

```
localization/eng/cards.json
localization/eng/relics.json
localization/eng/potions.json
localization/eng/powers.json
localization/eng/events.json
localization/eng/ancients.json
```

Each file maps **keys** to text:

```json
{
  "THE_GOLD_COIN_ROOM.pages.INITIAL.options.TAKE.title": "Pocket the coin.",
  "THE_GOLD_COIN_ROOM.pages.INITIAL.options.TAKE.description": "Gain [blue]{Gold}[/blue] [gold]gold[/gold]."
}
```

**The rule that causes the most confusion:** the first segment of every key must be your
class name converted to **SCREAMING_SNAKE_CASE**.

| Class name | Required key prefix |
|---|---|
| `TheGoldCoinRoom` | `THE_GOLD_COIN_ROOM` |
| `RockPaperScissors` | `ROCK_PAPER_SCISSORS` |
| `CoinFlip` | `COIN_FLIP` |

The framework derives the content's identity from the class name and looks up text under
that prefix. If your class is `RockPaperScissors` but your keys say `RPS.options.ROCK`,
the lookup fails and the game shows the raw key name on screen instead of the text. This
is not a packaging problem — it is permanently broken until the prefix matches the class.

Notes:

- Everything after the first segment (`.pages.INITIAL.options.TAKE.title`) is a structure
  *you* choose. Only the first segment is enforced.
- Text can embed dynamic vars (`{Gold}`) and rich-text tags (`[gold]…[/gold]`,
  `[blue]…[/blue]`).
- A missing key in a *rendering* path usually does not crash — ModSmith's `MissingLocPatch`
  shows the key as fallback text, and the on-screen key is exactly the string the game tried
  to find. **But this safety net does not cover every path** — see the troubleshooting note
  below.

### Troubleshooting: `NullReferenceException` ("Object reference not set to an instance of an object") from an event

This is the failure mode that wastes the most time, because the exception names neither the
key nor the class — just a bare null dereference, often pointing into your event's option
setup (`GenerateInitialOptions` / the method behind an `EventOption` callback).

**Most likely cause:** your localization key prefix does not match the class name. The
framework derives the content's identity from the class name (`RockPaperScissors` →
`ROCK_PAPER_SCISSORS`) and looks up *that* prefix. If your keys say `RPS.*`, the
framework's own lookup returns null and throws before `MissingLocPatch`'s on-screen
fallback ever runs — so unlike a missing key in a render path, you get a crash with no
fallback text and no hint about which key is wrong.

**How to confirm:** build with debug symbols so the stack trace carries line numbers
(`<DebugType>embedded</DebugType>` in your `.csproj`), reproduce, and check whether the
throwing frame is in your event's option construction. If so, it is almost always this.

**Fix:** make the first segment of *every* key for that content match the class name in
SCREAMING_SNAKE_CASE — in both the `.cs` lookups and the `localization/eng/*.json` file —
or rename the class so its SCREAMING_SNAKE_CASE form matches the prefix you already use.
The two must agree.

---

## Building and packaging

From the mod project directory:

```
dotnet publish
```

This compiles your C# into a DLL and copies it (plus the manifest) into the game's
`mods/` folder. The build is configured in `JacksMod.csproj`, which also references the
three things a mod links against: `sts2.dll` (the game), `0Harmony.dll` (patching), and
`ModSmith.dll` (the modding framework).

**Code vs. assets.** `dotnet publish` packages your *code*. Player-facing *assets* —
including the localization text — are bundled separately into a **PCK** file (Godot's
packaged-asset format) via a MegaDot export step. Until that export runs, your
localization JSON is present in the project but not loaded by the game, so text shows as
key names even when the keys are correct. The manifest (`JacksMod.json`) records whether
a mod ships a PCK:

```json
{
  "id": "JacksMod",
  "has_pck": true,
  "has_dll": true,
  "dependencies": [{ "id": "ModSmith", "minVersion": "v0.0.1" }]
}
```

> The exact PCK export workflow is an advanced build step. The lessons get you to a
> fully playable mod before that point — placeholder text and all.

---

## What this means for you

Look back at the four steps. They are the same for a card, a relic, a potion, an event,
and — when you get there — a custom **enemy**:

1. Extend the framework's base class for that content type.
2. Override the hooks where you want custom behavior.
3. Register it at startup.
4. Localize its text with a class-name-derived key prefix.

Once that pattern clicks, "what can I build?" stops being a question about the framework
and becomes a question about your imagination. The framework is not a wall of features
to memorize — it is one repeated shape. Learn the shape once, and every new kind of
content is a variation on it.

The curriculum's Lessons 5–7 build exactly this intuition by growing one event, and
Lesson 7 connects it back to this reference. When you want to add something new, start
here, find the closest starter example, and copy its shape.
