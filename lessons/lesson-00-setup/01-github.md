# 01 — GitHub: Your Code's Home

Every professional developer uses version control. It's how you save your work, share it with others, track your history, and collaborate without stepping on each other's toes. The most widely used platform for this is **GitHub**, which is built on a tool called **git**.

You don't need to fully understand git yet. For now, just follow the steps and pay attention to the vocabulary — you'll look these terms up again later and they'll make more sense once you've used them.

## Vocabulary

- **repository (repo)** — a folder that git is tracking. Think of it as a project with a history.
- **fork** — your own personal copy of someone else's repo on GitHub. You can change it without affecting the original.
- **clone** — downloading a repo from GitHub to your local machine so you can work on it.
- **commit** — saving a snapshot of your changes with a message describing what you did.
- **push** — sending your local commits up to GitHub.

## Step 1 — Create a GitHub account

Go to [github.com](https://github.com) and create a free account if you don't have one.

Pick a username you're happy with — it'll show up publicly on your work.

## Step 2 — Fork the mod template

The mod template lives at:

**[https://github.com/nafeger/Sts2-ModSmith](https://github.com/nafeger/Sts2-ModSmith)**

> This is a fork of the official ModSmith repository. You'll find out why you're using this one instead of the original in a later step.

1. Go to that URL
2. Click the **Fork** button in the top right
3. Under "Owner", select your own account
4. Click **Create fork**

You now have your own copy of the repo at `https://github.com/YOUR-USERNAME/Sts2-ModSmith`.

## Step 3 — Clone it to your machine

Open a terminal and run:

```bash
git clone https://github.com/YOUR-USERNAME/Sts2-ModSmith.git
cd Sts2-ModSmith
```

Replace `YOUR-USERNAME` with your actual GitHub username.

You should now have the project files on your machine. Take a look around — don't worry about understanding everything yet.

## Step 4 — Verify your remote

Run:

```bash
git remote -v
```

You should see `origin` pointing to your fork. That means when you push changes, they go to your copy on GitHub, not the original.

---

**Things to look up if you're curious:**
- "what is git version control"
- "git fork vs clone difference"
- "what is a git commit"

---

Next up: [02-setup.md](02-setup.md) — install the tools and try to build.
