# 04 — Your Turn

Two exercises. Do both.

---

## Exercise A: SumCards

Create a new file called `src/Scratch.cs` in your `JacksMod` project and write a method that takes a list of card costs and returns the total. (You can delete this file after — it's a scratchpad.)

```csharp
namespace JacksMod;  // replace with your mod's name

public static class Scratch
{
    public static int SumCards(List<int> costs)
    {
        // your code here
    }
}
```

Test it by calling it from your mod's `Initialize` method temporarily, or just trace through it by hand.

Expected result with `{ 1, 2, 0, 3, 1 }` → `7`.

**Step 1**: Write it using a `for` loop with an index.

**Step 2**: Rewrite it using `foreach`.

Both versions should produce the same result. Compare them. Which one is easier to read? There is no wrong answer — the point is to notice that you have two valid options and think about the tradeoff.

Bonus: try to do it in one line using LINQ's `.Sum()`. Don't worry if you haven't seen LINQ yet — search "LINQ Sum C#" and see if you can figure it out. You won't break anything.

---

## Exercise B: Trace DoubleIt

The `DoubleIt` method multiplies `DynamicVars.Gold.BaseValue` by 2 each time the player doubles. The starting gold is 20.

Assume the player doubles 4 times successfully, then busts on the 5th flip.

Write out the value of `DynamicVars.Gold.BaseValue` after each successful double:

| Double # | Gold after |
|----------|-----------|
| Start | 20 |
| 1st double | ? |
| 2nd double | ? |
| 3rd double | ? |
| 4th double | ? |
| 5th flip (bust) | player gets nothing |

Now answer these questions in a comment or a note:

1. If the player busted on the very first flip instead, what would `DynamicVars.Gold.BaseValue` be?
2. `DynamicVars.Gold.BaseValue *= 2` only runs on a successful double. If the player busts, does the gold value matter? Why or why not? Look at the `DoubleIt` source again.
3. What type of loop would you use if you wanted to simulate this without the event system — `for`, `while`, or `foreach`? Why?

---

## Connecting forward

You now have every tool you need to build Rock Paper Scissors.

- You can represent the three choices as a `List<string>`
- You can pick a random one from the list
- You can compare the player's pick to the computer's pick with `if/else` (Lesson 2)
- You can loop through rounds with a `for` loop

Lesson 4 is the build.
