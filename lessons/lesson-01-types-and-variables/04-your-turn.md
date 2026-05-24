# 04 — Your Turn: Add a Constant

This is your first exercise where you change working code and see the result in the game.

## The task

In `CoinFlip.cs`, the gold reward is set like this:

```csharp
protected override IEnumerable<DynamicVar> CanonicalVars => [
  new GoldVar(10),
];
```

And the upgrade bonus is baked into `OnUpgrade`:

```csharp
protected override void OnUpgrade()
{
  base.DynamicVars.Gold.UpgradeValueBy(2);
}
```

The values `10` and `2` are **magic numbers** — numbers with meaning that aren't explained. If you come back to this code in a month, you'd have to reason about why it's `10` and not `15`.

Your job: replace those magic numbers with named constants.

## Step 1: Define the constants

At the top of the `CoinFlip` class (before the constructor), add:

```csharp
private const int BASE_GOLD = 10;
private const int UPGRADE_BONUS = 2;
```

## Step 2: Use the constants

Replace the `10` and `2` with your constant names:

```csharp
protected override IEnumerable<DynamicVar> CanonicalVars => [
  new GoldVar(BASE_GOLD),
];

protected override void OnUpgrade()
{
  base.DynamicVars.Gold.UpgradeValueBy(UPGRADE_BONUS);
}
```

## Step 3: Build and verify

```bash
dotnet publish
```

The build should succeed. Then launch the game, load your mod, and find the CoinFlip card. It should behave exactly the same as before — you changed the structure, not the behavior.

## Step 4: Change a value

Now try this: change `BASE_GOLD` to `25`. Build again, launch the game, and play CoinFlip. The heads reward should now be 25 gold instead of 10.

Change it back to `10` when you're done.

## What you just practiced

- Adding `const` declarations
- Naming magic numbers so code explains itself
- Verifying your change compiles and runs correctly
- Making a behavioral change and observing it in game

## Bonus challenge (solo)

Look at `GoldArmor.cs` — the relic that spends gold to gain block each turn. It also has magic numbers. Can you replace them with named constants the same way?

## Commit your work

```bash
git add src/StarterContent/cards/CoinFlip.cs
git commit -m "refactor: extract CoinFlip magic numbers to named constants"
```

---

## You're done with Lesson 1

Check off the success criteria in [README.md](README.md).

**Things to look up:**
- "C# magic numbers antipattern"
- "C# const vs static readonly"

---

When you're ready: **[Lesson 2 → Control Flow](../lesson-02-control-flow/README.md)**
