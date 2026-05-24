# 03 — Recursion

## What recursion is

A recursive method is a method that calls itself.

```csharp
void CountUp(int n, int max)
{
    if (n > max) return;       // base case
    Console.WriteLine(n);
    CountUp(n + 1, max);       // recursive case
}
```

Call it with `CountUp(1, 10)` and it prints 1 through 10, same as a loop.

Every recursive method has two parts:

- **Base case** — the condition that stops the recursion. Without it, the method calls itself forever.
- **Recursive case** — the call to itself, with an argument that moves closer to the base case.

If you forget the base case, or write one that is never reached, you get a stack overflow (more on that below).

## Three ways to count from 1 to 10

These all produce the same output. They are all valid.

```csharp
// --- for loop ---
for (int i = 1; i <= 10; i++)
{
    Console.WriteLine(i);
}

// --- while loop ---
int i = 1;
while (i <= 10)
{
    Console.WriteLine(i);
    i++;
}

// --- recursion ---
void CountUp(int n, int max)
{
    if (n > max) return;       // base case: stop when we've passed max
    Console.WriteLine(n);
    CountUp(n + 1, max);       // recursive case: count the next number
}
CountUp(1, 10);
```

The loops manage state in a variable (`i`) that lives in the current scope. Recursion manages state in parameters that get passed down. The bookkeeping is different; the result is the same.

## The call stack

You saw the call stack in the debugger section of Lesson 0. Every time a method is called, a new frame is pushed onto the stack. When the method returns, that frame is popped off.

For `CountUp(1, 10)`, the call stack grows like this:

```
CountUp(1, 10)
  CountUp(2, 10)
    CountUp(3, 10)
      ...
        CountUp(10, 10)
          CountUp(11, 10)  <- n > max, returns immediately
        <- returns
      <- returns
    <- returns
  <- returns
```

Each call is a frame. Ten active calls means ten frames stacked up. When the base case fires, frames start popping off in reverse order.

This is why the call stack in a debugger looks like nested method calls — it literally is.

## Stack overflow

The call stack has a fixed size. If recursion goes too deep before hitting a base case, you run out of stack space. C# throws a `StackOverflowException` and the program crashes.

Practical limit in C# is roughly 10,000–20,000 nested calls depending on the environment. For most game logic, you will never get close. If you write a recursive function that could theoretically run thousands of levels deep, a loop is safer.

For short, well-bounded recursion — parsing a tree, walking a small hierarchy, simple math — recursion is clean and fine.

## TheGoldCoinRoom as a kind of loop

Look at `DoubleIt` from the mod:

```csharp
private Task DoubleIt()
{
    var doubled = Rng.NextBool();
    if (doubled)
    {
        DynamicVars.Gold.BaseValue *= 2;
        SetEventState(..., [
            new EventOption(this, TakeGold, ...),
            new EventOption(this, DoubleIt, ...),  // <-- offers DoubleIt again
        ]);
    }
    else
    {
        SetEventFinished(...);  // <-- base case: bust
    }
    return Task.CompletedTask;
}
```

`DoubleIt` doesn't call itself directly — it tells the event system "when the player picks this option, run `DoubleIt` again." The event state machine is doing the looping. But the structure maps directly onto recursion:

- **Recursive case**: gold doubled, options reset, `DoubleIt` offered again
- **Base case**: coin flip fails, event ends

If you wanted to simulate this without an event system, you could write it as a `while` loop:

```csharp
int gold = 20;
bool keepGoing = true;

while (keepGoing)
{
    bool doubled = FlipCoin();
    if (doubled)
    {
        gold *= 2;
        Console.WriteLine($"Doubled! Gold is now {gold}. Double again? (y/n)");
        keepGoing = Console.ReadLine() == "y";
    }
    else
    {
        Console.WriteLine("Bust. You get nothing.");
        keepGoing = false;
    }
}
```

Same logic. Different shape.

## When to use recursion vs a loop

Recursion shines when the problem is naturally hierarchical or self-similar: walking a tree, parsing nested data, divide-and-conquer algorithms.

Loops are the right tool for most sequential repetition in game logic. They're easier to trace, easier to debug, and don't risk stack overflow.

When you reach for recursion, ask: is this problem naturally tree-shaped? If yes, recursion might be cleaner. If it's just "do this N times," use a loop.

## Vocabulary

| Term | Meaning |
|------|---------|
| recursion | a method that calls itself |
| base case | the condition that stops recursion from continuing |
| recursive case | the branch where the method calls itself with a modified argument |
| call stack | the stack of active method frames; grows with each call, shrinks as methods return |
| stack frame | the chunk of memory holding one method's local variables and return address |
| stack overflow | crash caused by the call stack exceeding its maximum size |
| `StackOverflowException` | the C# exception thrown when the call stack overflows |

## Things to look up

- Fibonacci sequence implemented recursively vs iteratively — a classic side-by-side comparison
- Tail recursion — an optimization some languages apply to recursive calls at the end of a method; C# does not do this automatically
- Tree traversal — a real-world case where recursion is genuinely cleaner than a loop
- "The call stack" — search for a visual diagram; most debugger tutorials have one
