# 06 — Your First Commit: Make It Yours

The mod template has placeholder values in it — it says `"your-name"` where your name should go. Let's fix that, and use it as an excuse to make your first real git commit.

## Update the manifest

Open `ModTemplate.json` in VS Code — it's in the root of the `ModTemplate` folder, not inside `src/`. It looks something like this:

```json
{
  "id": "ModTemplate",
  "name": "ModTemplate",
  "author": "your-name",
  "description": "Mod for Slay the Spire 2",
  "version": "v0.0.0",
  ...
}
```

Update it:
- Change `"author"` to your name
- Change `"name"` to whatever you want to call your mod
- Change `"id"` to match (no spaces — use dashes or CamelCase)
- Update `"description"` to something that describes your mod

Save the file.

## See what changed

In your terminal:

```bash
git status
```

This shows you which files have been modified. You should see `ModTemplate.json` listed.

```bash
git diff
```

This shows you exactly what changed — lines removed in red, lines added in green.

## Stage and commit

```bash
git add ModTemplate.json
git commit -m "chore: update manifest with my name and mod details"
```

The `-m` flag lets you write your commit message inline. A good commit message says *what* changed and *why*, briefly.

## Push to GitHub

```bash
git push
```

Now go to your GitHub repo in a browser. You should see your commit in the history.

## A note on what you're working in

Right now you're working inside your fork of `Sts2-ModSmith` — modifying the template that came with the framework. That's intentional for now. The starter content (CoinFlip, TheGoldCoinRoom, etc.) is your reference material for the next few lessons.

In Lesson 4 you'll create your own standalone mod project from scratch. That's when you'll make a new GitHub repo and push your own work to it. For now, this fork is your workshop.

## Verify in game

Run `dotnet publish` again, launch the game with mods enabled, and check the mod list. Your mod's name should appear — no longer "ModTemplate" but whatever you named it.

---

## You're done with Lesson 0

Go back to [README.md](README.md) and check off every item in the success criteria. If anything isn't working, now is the time to fix it — every lesson after this builds on a working environment.

**Things to look up:**
- "git add vs git commit"
- "git push vs git commit difference"
- "how to write a good commit message"

---

When you're ready: **[Lesson 1 → Types & Variables](../lesson-01-types-and-variables/README.md)**
