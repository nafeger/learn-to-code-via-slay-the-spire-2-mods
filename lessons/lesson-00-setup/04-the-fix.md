# 04 — The Fix: Open Source in the Real World

Here's something worth knowing: the official ModSmith template has bugs. The game was updated and two things changed in the API — code that used to work stopped compiling.

Your parent found these bugs while setting up this curriculum, figured out what was wrong, and filed a fix with the ModSmith project.

## What happened

**Bug 1: `TalkCmd.Play` signature changed**

The game updated its API. A method called `TalkCmd.Play` used to take a duration as a `double` (like `1.5` for 1.5 seconds). The game changed it to take a `VfxColor` and a `VfxDuration` enum instead. The template wasn't updated to match.

**Bug 2: `IsPoweredAttack` defined twice**

ModSmith used to include a helper method called `IsPoweredAttack` because the game didn't have it yet. Then the game added it natively. Now it exists in two places with the same name, and the compiler can't decide which one to use — hence "ambiguous."

## What your parent did about it

1. Used a decompiler to inspect the game's DLL and find the correct new API signatures
2. Fixed both files in the template
3. Opened a pull request (PR) against the official ModSmith repo

You can see that PR here:
👉 [https://github.com/cpimhoff/Sts2-ModSmith/pull/5](https://github.com/cpimhoff/Sts2-ModSmith/pull/5)

A **pull request** is how you propose a fix to someone else's open source project. You make the change in your fork and ask the maintainer to "pull" it into the official version. The fix is pending review — it hasn't been merged yet.

## Vocabulary

- **API (Application Programming Interface)** — the set of methods and types a library exposes for you to use. When the game updates its API, code written against the old version can break.
- **pull request (PR)** — a proposal to merge changes from one branch or fork into another. Standard practice in open source.
- **open source** — software whose source code is publicly available. Anyone can read it, report bugs, or contribute fixes.
- **fork** — remember from lesson 01? Your parent's fork at `nafeger/Sts2-ModSmith` has these fixes. The upstream `cpimhoff/Sts2-ModSmith` does not yet.

## Switch to the working fork

You already cloned your fork of `nafeger/Sts2-ModSmith` in step 01 — which means you already have the fixes. Run `dotnet build` again from the `ModTemplate` folder:

```bash
dotnet build
```

This time it should succeed.

## Things to look up

- "what is an API in programming"
- "what is open source software"
- "how does a github pull request work"

---

Next up: [05-vscode.md](05-vscode.md) — set up your editor and debugger.
