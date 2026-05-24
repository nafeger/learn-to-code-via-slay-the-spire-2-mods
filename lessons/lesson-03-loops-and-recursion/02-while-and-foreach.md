# 02 — While Loops and Foreach

## The while loop

A `while` loop has only a condition. It keeps running as long as that condition is true.

```csharp
int i = 1;
while (i <= 10)
{
    Console.WriteLine(i);
    i++;
}
```

Output: `1 2 3 4 5 6 7 8 9 10`

The shape is:

```
while ( condition )
{
    // body — must eventually make condition false
}
```

The condition is checked before each iteration, including the first. If the condition is false from the start, the body never runs:

```csharp
int x = 100;
while (x < 10)
{
    Console.WriteLine("this never prints");
}
```

## Infinite loops

If nothing inside the loop body makes the condition false, the loop runs forever. This will lock up your program (or in a game, freeze the frame).

```csharp
// BUG: forgot to increment i
int i = 0;
while (i < 10)
{
    Console.WriteLine(i);
    // i never changes — runs forever
}
```

When you write a `while` loop, ask yourself: "what in this body moves me toward the condition being false?" If the answer is nothing, you have an infinite loop.

## The foreach loop

`foreach` is designed for iterating over a collection. You don't manage an index. You just get each item in turn.

```csharp
List<string> options = new List<string> { "Rock", "Paper", "Scissors" };

foreach (string option in options)
{
    Console.WriteLine(option);
}
```

Output:
```
Rock
Paper
Scissors
```

The shape is:

```
foreach ( ElementType variableName in collection )
{
    // variableName holds the current element
}
```

It works with `List<T>`, arrays, and anything that implements `IEnumerable<T>` — which you don't need to know yet, but you will in Lesson 7.

You cannot modify the collection while iterating it with `foreach`. If you need to add or remove items as you go, use a `for` loop with an index instead.

## When to use which

| Loop type | Use when |
|-----------|----------|
| `for` | You know the exact count before you start, or you need the index |
| `while` | You repeat until a condition changes, and you don't know how many times that will take |
| `foreach` | You have a collection and want each item; you don't need the index |

These rules are guidelines. You can technically use any loop for any situation. The question is which one makes your intent most readable. `foreach` over a list says "I want every item" clearly. A `while` that checks player health says "keep going until something changes" clearly.

## A practical example: checking player options

```csharp
List<string> validChoices = new List<string> { "Rock", "Paper", "Scissors" };
string playerInput = "Lizard";  // not valid
bool found = false;

foreach (string choice in validChoices)
{
    if (choice == playerInput)
    {
        found = true;
        break;  // stop as soon as we find a match
    }
}

if (!found)
{
    Console.WriteLine("Invalid choice.");
}
```

`break` exits the loop immediately. You'll use it often with `while` and `foreach`.

## do-while (bonus)

There is a third variant where the condition is checked after the body instead of before. The body always runs at least once.

```csharp
do
{
    Console.WriteLine("runs at least once");
} while (false);
```

It's less common. Good to know it exists.

## Vocabulary

| Term | Meaning |
|------|---------|
| loop condition | the boolean expression checked each iteration |
| infinite loop | a loop whose condition never becomes false |
| `foreach` | a loop that iterates over every element in a collection |
| collection | an object that holds multiple values: `List<T>`, arrays, etc. |
| `break` | exits the loop immediately |
| `continue` | skips to the next iteration |
| `IEnumerable<T>` | the interface that makes an object work with `foreach` |

## Things to look up

- `List<T>` methods: `Add`, `Remove`, `Contains`, `Count`
- `Array` vs `List<T>` — when would you use one vs the other?
- `break` vs `return` inside a loop — what is the difference?
- `do-while` syntax in C#
