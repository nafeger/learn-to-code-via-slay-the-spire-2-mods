# 01 — Lists

## The problem a list solves

You can hold one throw in a `Choice` variable:

```csharp
Choice lastThrow = Choice.Rock;
```

But the opponent needs to remember *several* recent throws, and you do not know in advance
how many — it grows as the match goes on. You cannot declare `throw1`, `throw2`, `throw3`,
… that way lies madness. You need one thing that holds a *sequence* of choices and can grow.

That is a **list**.

## `List<T>`

A `List<T>` is an ordered, growable sequence. The `T` is the type of thing it holds — you
fill it in when you declare the list:

```csharp
List<Choice> throws = new List<Choice>();   // a list of Choice values, currently empty
List<int> costs = new List<int>();          // a list of ints
List<string> names = new List<string>();    // a list of strings
```

`List<Choice>` is read "list of Choice." The angle brackets make `List` **generic** — it
works for any element type, and you pick the type per list. This is the same `<T>` shape you
have seen on `IEnumerable<DynamicVar>` and `IReadOnlyList<EventOption>` in the mod code.
Those are collection types too.

> The leading `I` on `IEnumerable` and `IReadOnlyList` marks them as **interfaces** — a
> *contract* a type promises to fulfil, as opposed to a concrete class like `List` you build
> with `new`. Interfaces (and how `List` relates to them) are the whole subject of Lesson 7;
> you do not need them yet. For now, read `IReadOnlyList<EventOption>` as "something that
> behaves like a read-only list of options," and keep moving. The distinction between a
> *contract*, a class that *implements* it, and an *instance* of that class is exactly what
> Lesson 7 untangles.

You can use the `new()` shorthand from Lesson 5 here as well:

```csharp
List<Choice> throws = new();
```

## The operations you need

**Add** — put something on the end:

```csharp
throws.Add(Choice.Rock);
throws.Add(Choice.Paper);
// throws now holds: Rock, Paper
```

**Count** — how many are in it:

```csharp
int howMany = throws.Count;   // 2
```

Note `Count` is a property, not a method — no parentheses. (A `List`'s size is data it
already knows, so you read it like a field.)

**Index** — read the item at a position. Positions start at **0**:

```csharp
Choice first = throws[0];   // Rock
Choice second = throws[1];  // Paper
```

The first item is at index `0`, the second at `1`, and the last at `Count - 1`. This zero-
based counting trips up everyone at first. If a list has 3 items, the valid indexes are 0,
1, and 2 — and `throws[3]` would crash with an out-of-range error, because there is no
fourth slot.

**Remove at a position** — drop the item at an index; everything after it shifts down:

```csharp
throws.RemoveAt(0);   // removes Rock; Paper shifts to index 0
```

You will use `RemoveAt(0)` in this lesson to drop the *oldest* throw when the memory gets
full.

## Iterating with `foreach`

You met `foreach` in Lesson 3. It is the natural way to visit every item in a list:

```csharp
foreach (Choice c in throws)
{
    // c is each item in turn: first Rock, then Paper
}
```

`foreach` reads as "for each `Choice c` in `throws`, run this block." You do not manage an
index; it walks the whole list in order. When you need the position too, a `for` loop with
`throws[i]` is the alternative — but for "do something with every item," `foreach` is
cleaner and harder to get wrong.

## The functional alternative

If you have seen JavaScript's `array.forEach(...)` or Ruby's `each`, you are looking for the
same thing in C#. There are two flavors.

A `List<T>` has its own `ForEach` method that takes a **lambda** (the `() =>` syntax from
Lesson 4 — here it takes the item as a parameter):

```csharp
throws.ForEach(c => DoSomethingWith(c));
```

That is the direct `forEach` analog. But C#'s real functional toolkit is **LINQ** — a set of
methods that *describe what you want* instead of *how to loop for it*. Instead of writing a
loop to count or find, you call a method:

```csharp
using System.Linq;   // LINQ lives here

int rockCount = throws.Count(c => c == Choice.Rock);   // how many Rocks?
Choice newest  = throws.Last();                        // the last item — no index math
bool anyPaper  = throws.Any(c => c == Choice.Paper);   // is there a Paper at all?
```

Each reads almost like English: "count the ones that are Rock," "the last one," "any Paper?"
LINQ is declarative — you say the *what*, and it runs the loop for you.

Which to use? A plain `foreach` is clearest when you are doing several things per item or the
logic is involved. LINQ shines for the one-liners — counting, filtering, finding, transforming
— where a loop would be more ceremony than substance. You will see LINQ again in this lesson's
"things to look up," and it is worth learning well once you are comfortable with loops. For
now, know that both styles exist and produce the same results; the loop is never *wrong*, just
sometimes wordier.

## A worked example: the most recent throw

Suppose you want the most recent throw in the list — the last one added. It is at index
`Count - 1`:

```csharp
Choice mostRecent = throws[throws.Count - 1];
```

If `throws` holds `Rock, Paper, Scissors` (count 3), then `Count - 1` is `2`, and
`throws[2]` is `Scissors` — the last one added. Trace it: the newest item is always at the
end, because `Add` appends to the end.

Guard against an empty list, though — `throws[Count - 1]` on an empty list is `throws[-1]`,
which crashes. Always check `Count > 0` before indexing into a list you are not sure has
anything in it:

```csharp
if (throws.Count > 0)
{
    Choice mostRecent = throws[throws.Count - 1];
}
```

The functional style sidesteps the index math entirely: `throws.Last()` gives the same value,
and `throws.LastOrDefault()` returns a default instead of crashing on an empty list — the
guard built in. The index version is shown here because it teaches what "last" *means*
(position `Count - 1`); reach for `.Last()` once that clicks.

## Lists in the mod code you already have

The event options you return are a list:

```csharp
return [
    new EventOption(this, () => Play(Choice.Rock),     "ROCK_PAPER_SCISSORS.options.ROCK"),
    new EventOption(this, () => Play(Choice.Paper),    "ROCK_PAPER_SCISSORS.options.PAPER"),
    new EventOption(this, () => Play(Choice.Scissors), "ROCK_PAPER_SCISSORS.options.SCISSORS"),
];
```

The `[ ... ]` is a **collection literal** — a compact way to build a list with known items
up front. The method's return type, `IReadOnlyList<EventOption>`, is a list type. You have
been producing lists since Lesson 4; now you will build one that grows during play.

## Vocabulary

**List (`List<T>`)** — An ordered, growable collection of items of type `T`.

**Generic type** — A type parameterized by another type, written with `<>`. `List<Choice>`
is the generic `List` specialized to hold `Choice` values.

**Index** — An item's position in a list, starting at `0`. The last index is `Count - 1`.

**Zero-based** — Counting positions from 0 rather than 1. The first item is at index 0.

**Collection literal** — The `[ a, b, c ]` syntax for building a collection inline.

## Things to look up

- "C# List<T>" — the full set of methods (`Insert`, `Contains`, `Clear`, and more)
- "C# generics" — what `<T>` means and why it matters
- "off by one error" — the classic bug that lives at list boundaries
- "C# LINQ" — the functional toolkit (`Where`, `Select`, `Count`, `Any`, `Last`, `Sum`)
- "C# List ForEach vs foreach" — the method, the statement, and when each fits
