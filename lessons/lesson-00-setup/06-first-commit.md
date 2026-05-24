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

## A note on remotes and upstreams

Run this and look at the output:

```bash
git remote -v
```

You have at least two repos in play now:

- **Your fork of Sts2-ModSmith** — this is where the mod template and framework live. Your `origin` points here.
- **Your mod project repo** — the one you created in step 01. This is where your actual mod code will eventually live.

When your fork diverges from the original, git tracks these as separate **remotes**. You can add the original repo as an `upstream` remote — that lets you pull in future official changes:

```bash
git remote add upstream https://github.com/cpimhoff/Sts2-ModSmith.git
git remote -v
```

Now you have:
- `origin` — your fork (you push here)
- `upstream` — the official repo (you pull from here when it updates)

This pattern — fork + upstream — is how most open source contribution workflows are structured. You'll see it constantly.

## Verify in game

Run `dotnet publish` again, launch the game with mods enabled, and check the mod list. Your mod's name should appear — no longer "ModTemplate" but whatever you named it.

---

## You're done with Lesson 0

Go back to [README.md](README.md) and check off every item in the success criteria. If anything isn't working, now is the time to fix it — every lesson after this builds on a working environment.

**Things to look up:**
- "git add vs git commit"
- "git push vs git commit difference"
- "how to write a good commit message"
- "git remote add upstream explained"

---

When you're ready: **[Lesson 1 → Types & Variables](../lesson-01-types-and-variables/README.md)**
