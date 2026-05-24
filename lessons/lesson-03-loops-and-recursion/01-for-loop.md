# 01 — The For Loop

## The basic idea

A `for` loop runs a block of code a specific number of times. It has three parts packed into one line.

```csharp
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}
```

Output: `0 1 2 3 4 5 6 7 8 9`

## Anatomy

```
for ( initialization ; condition ; increment )
      ^^^^^^^^^^^^     ^^^^^^^^^   ^^^^^^^^^
      runs once        checked     runs after
      at the start     before      each iteration
                       each run
```

Break it down:

- **Initialization** — `int i = 0` declares a counter variable and sets its starting value. This runs exactly once, before anything else.
- **Condition** — `i < 10` is checked before every iteration. If it is `true`, the loop body runs. If it is `false`, the loop stops.
- **Increment** — `i++` runs after each loop body execution. It is shorthand for `i = i + 1`.

The loop body is everything between the curly braces.

## Off-by-one errors

This is one of the most common bugs in all of programming.

```csharp
// Prints 0 through 9 — ten numbers
for (int i = 0; i < 10; i++) { ... }

// Prints 0 through 10 — eleven numbers
for (int i = 0; i <= 10; i++) { ... }

// Prints 1 through 10 — ten numbers, starting at 1
for (int i = 1; i <= 10; i++) { ... }
```

The relationship between your starting value, your condition operator (`<` vs `<=`), and what you actually want is worth pausing on. Trace through the first example mentally:

- i = 0, condition: 0 < 10 = true, run body
- i = 1, condition: 1 < 10 = true, run body
- ...
- i = 9, condition: 9 < 10 = true, run body
- i = 10, condition: 10 < 10 = **false**, stop

If you used `i <= 10` instead, i = 10 passes the check and runs one extra time.

## Counting backward

Flip the initialization and condition, and decrement instead of increment:

```csharp
for (int i = 10; i >= 1; i--)
{
    Console.WriteLine(i);
}
```

Output: `10 9 8 7 6 5 4 3 2 1`

`i--` is shorthand for `i = i - 1`.

## A practical example: summing a list

You have a list of card costs. You want the total mana cost of your hand.

```csharp
List<int> cardCosts = new List<int> { 1, 2, 0, 3, 1 };
int total = 0;

for (int i = 0; i < cardCosts.Count; i++)
{
    total += cardCosts[i];  // total = total + cardCosts[i]
}

Console.WriteLine(total);  // 7
```

A few things worth noting:

- `cardCosts.Count` is the number of items in the list. Using it in the condition means you never have to hardcode the list size.
- `cardCosts[i]` accesses item at index `i`. Indexes start at 0, so the last valid index is `Count - 1`. This is why `i < cardCosts.Count` (not `<=`) is correct — accessing index `Count` would crash.
- `total += cardCosts[i]` is shorthand for `total = total + cardCosts[i]`.

## Tracing a loop manually

When a loop does something unexpected, trace it by hand. Write a small table:

| i | cardCosts[i] | total before | total after |
|---|---|---|---|
| 0 | 1 | 0 | 1 |
| 1 | 2 | 1 | 3 |
| 2 | 0 | 3 | 3 |
| 3 | 3 | 3 | 6 |
| 4 | 1 | 6 | 7 |

This is tedious but it finds bugs fast.

## Vocabulary

| Term | Meaning |
|------|---------|
| iteration | one execution of the loop body |
| loop variable / counter | the variable (often `i`) that tracks progress |
| increment | increase by one (`i++`) |
| decrement | decrease by one (`i--`) |
| off-by-one error | a bug where a loop runs one too many or one too few times |
| index | the position of an item in a list or array; starts at 0 |
| `List<T>.Count` | the number of items in a list |

## Things to look up

- `i++` vs `++i` — they behave differently when used as expressions (search "prefix vs postfix increment C#")
- `+=` `-=` `*=` `/=` — all the compound assignment operators
- `break` and `continue` — ways to exit a loop early or skip an iteration
- What happens when you access an index out of bounds (search "IndexOutOfRangeException C#")
