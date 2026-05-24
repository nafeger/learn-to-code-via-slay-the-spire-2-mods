# 04 — The Fix: Open Source in the Real World

Here's something worth knowing: the official ModSmith template has bugs. The game was updated and two things changed in the API — code that used to work stopped compiling.

@nafeger found these bugs while setting up this curriculum, figured out what was wrong, and filed a fix with the ModSmith project.

## What happened

**Bug 1: `TalkCmd.Play` signature changed**

The game updated its API. A method called `TalkCmd.Play` used to take a duration as a `double` (like `1.5` for 1.5 seconds). The game changed it to take a `VfxColor` and a `VfxDuration` enum instead. The template wasn't updated to match.

**Bug 2: `IsPoweredAttack` defined twice**

ModSmith used to include a helper method called `IsPoweredAttack` because the game didn't have it yet. Then the game added it natively. Now it exists in two places with the same name, and the compiler can't decide which one to use — hence "ambiguous."

## What @nafeger did about it

1. Used a decompiler to inspect the game's DLL and find the correct new API signatures
2. Fixed both files in the template
3. Opened a pull request (PR) against the official ModSmith repo

You can see that PR here:
👉 [https://github.com/cpimhoff/Sts2-ModSmith/pull/5](https://github.com/cpimhoff/Sts2-ModSmith/pull/5)

A **pull request** is how you propose a fix to someone else's open source project. You make the change in your fork and ask the maintainer to "pull" it into the official version. The fix is pending review — it hasn't been merged yet.

## Why any of this is possible: distributed version control

Git is a **distributed** version control system. That word matters.

In a centralized system, there's one server and everyone connects to it. If that server has bad code, you're stuck.

In a distributed system, every clone is a complete copy of the repo — full history and all. There's no single source of truth that everyone must obey. `cpimhoff/Sts2-ModSmith` and `nafeger/Sts2-ModSmith` are both complete, valid copies of the same repo at different points in time. Your local clone is too.

This means you can point your local repo at *any* compatible remote and pull from it. Remotes are just URLs — they're not special. You can change where `origin` points, add new remotes, pull from one and push to another. The code on your machine doesn't care which server it came from.

That's what makes open source work at scale. Someone finds a bug, fixes it in their copy, and anyone can pull that fix immediately — without waiting for the original maintainer to merge it.

## Switch to the fixed version and rebuild

You already have the repo cloned. Just tell git to pull from the version that has the fix:

```bash
git remote set-url origin https://github.com/nafeger/Sts2-ModSmith.git
git pull
```

The first command changes where `origin` points. The second pulls down the updated code. Your local history stays intact — git figures out what changed and applies it.

Now build again from the `ModTemplate` folder:

```bash
dotnet build
```

This time it should succeed.

## Install the fixed template and scaffold your own mod

The ModSmith template is a `dotnet new` template — a blueprint for creating new mod projects. Now that you have the fixed version locally, install it:

```bash
cd ~/code/Sts2-ModSmith
dotnet new install ./ModTemplate --force
```

Then scaffold your own mod project. Go back to your `~/code` folder and create it there:

```bash
cd ~/code
dotnet new modsmith-mod -n JacksMod --StarterContent
```

Replace `JacksMod` with whatever you want to call your mod. The `--StarterContent` flag includes example code (CoinFlip, GoldArmor, etc.) that you'll read and modify in the lessons ahead.

Build it to confirm it works:

```bash
cd JacksMod
dotnet build
```

This is your mod. It lives at `~/code/JacksMod`, separate from the ModSmith framework. The ModSmith clone in `~/code/Sts2-ModSmith` is now just reference material — you won't edit it directly again.

## Create a GitHub repo for your mod

Your mod needs its own home on GitHub.

1. Go to [github.com/new](https://github.com/new)
2. Name it `JacksMod` (or whatever you chose above)
3. Set it to **Public**
4. Do **not** check "Add a README file" — your project already has one
5. Click **Create repository**

GitHub will show you instructions for pushing an existing repo. Run those — they'll look like:

```bash
git init
git add .
git commit -m "chore: initial mod scaffold"
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/JacksMod.git
git push -u origin main
```

Now your mod is on GitHub and everything you build from here lives there.

## Vocabulary

- **API (Application Programming Interface)** — the set of methods and types a library exposes for you to use. When the game updates its API, code written against the old version can break.
- **pull request (PR)** — a proposal to merge changes from one branch or fork into another. Standard practice in open source.
- **open source** — software whose source code is publicly available. Anyone can read it, report bugs, or contribute fixes.
- **distributed version control** — a system where every clone is a complete copy of the repo, not just a connection to a central server. Git is distributed; older systems like SVN were centralized.
- **remote** — a named URL that git knows about. `origin` is just the conventional name for the default one.
- **template** — a blueprint for generating new projects. `dotnet new modsmith-mod` uses the ModSmith template to scaffold a working mod skeleton.

## Things to look up

- "what is an API in programming"
- "what is open source software"
- "how does a github pull request work"
- "git distributed vs centralized version control"
- "dotnet new template explained"

---

Next up: [05-vscode.md](05-vscode.md) — set up your editor and debugger.
