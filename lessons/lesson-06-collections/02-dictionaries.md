# 02 — Dictionaries and the Tally Pattern

A list answers "what is the sequence?" A **dictionary** answers "what value goes with this
key?" They are the two collections you will use most, and they solve different problems.

## `Dictionary<TKey, TValue>`

A dictionary maps **keys** to **values**. You look something up by its key and get its value
back, fast, without scanning the whole thing.

```csharp
Dictionary<Choice, int> counts = new();   // maps a Choice to an int
```

Read `Dictionary<Choice, int>` as "dictionary from Choice to int" — the keys are `Choice`
values, the values are counts. Two type parameters this time: key type, then value type.

**Store** a value under a key (and overwrite if the key already exists):

```csharp
counts[Choice.Rock] = 3;
counts[Choice.Paper] = 1;
```

**Read** a value by its key:

```csharp
int rockCount = counts[Choice.Rock];   // 3
```

**Check** whether a key is present before reading it:

```csharp
if (counts.ContainsKey(Choice.Scissors))
{
    int s = counts[Choice.Scissors];
}
```

Reading a key that is not in the dictionary crashes, the same way an out-of-range list
index does. `ContainsKey` is how you ask first.

## You already know a dictionary: localization

You have been using a dictionary since Lesson 4 without the name. The localization file is
exactly this:

```json
{
  "ROCK_PAPER_SCISSORS.options.ROCK.title": "Rock",
  "ROCK_PAPER_SCISSORS.options.ROCK.description": "A solid, dependable choice."
}
```

That JSON is a dictionary from string keys to string values. When the game runs
`L10NLookup("ROCK_PAPER_SCISSORS.options.ROCK.title")`, it is doing a dictionary lookup:
take this key, return its value. And the failure mode you learned about — a missing key
showing the raw key name — is exactly "the key was not in the dictionary." Same structure,
same rules. You understand dictionaries already; this chapter just gives you your own.

## The tally pattern

The single most common thing people do with a dictionary is **count occurrences**: given a
list of things, how many times does each one appear? This is the *tally pattern*, and you
will use it constantly.

The shape: walk the list, and for each item, bump its count in a dictionary.

```csharp
Dictionary<Choice, int> Tally(List<Choice> throws)
{
    Dictionary<Choice, int> counts = new();

    foreach (Choice c in throws)
    {
        if (!counts.ContainsKey(c))   // first time we have seen this choice
        {
            counts[c] = 0;            // start its count at zero
        }
        counts[c] = counts[c] + 1;    // bump it
    }

    return counts;
}
```

Walk through it with `throws = [Rock, Paper, Rock]`:

| Step | item | `ContainsKey`? | action | `counts` after |
|------|------|----------------|--------|----------------|
| 1 | Rock | no | set to 0, then +1 | `{Rock: 1}` |
| 2 | Paper | no | set to 0, then +1 | `{Rock: 1, Paper: 1}` |
| 3 | Rock | yes | +1 | `{Rock: 2, Paper: 1}` |

At the end, `counts[Choice.Rock]` is `2` and `counts[Choice.Paper]` is `1`. You have turned
a sequence into a count-per-item. That `if (!ContainsKey) set to 0` guard is the heart of
the pattern — without it, the first `counts[c] + 1` would crash, because `counts[c]` does
not exist yet.

## Finding the largest count

Once you have the tally, "which choice appears most?" is a walk over the dictionary,
tracking the best so far:

```csharp
Choice MostFrequent(List<Choice> throws)
{
    Dictionary<Choice, int> counts = Tally(throws);

    Choice best = throws[0];              // the key with the most counts so far...
    int bestCount = 0;                    // ...and how many counts that is
    foreach (var pair in counts)          // each pair has a .Key and a .Value
    {
        if (pair.Value > bestCount)       // this choice appears more often than our best
        {
            best = pair.Key;
            bestCount = pair.Value;       // remember the new high count
        }
    }
    return best;
}
```

`foreach (var pair in counts)` walks the dictionary's entries. Each `pair` has a `.Key` (a
`Choice`) and a `.Value` (its count). The loop keeps the key with the highest count seen so
far — the classic "max" pattern from Lesson 3, now over a dictionary. Notice you track *two*
things together: the best key (`best`) and its count (`bestCount`). Carrying the count in its
own variable, instead of looking it up again as `counts[best]` each comparison, is the
clearer way to write a max scan — one variable, updated in lockstep with the key it describes.

`var` here means "let the compiler figure out the type of `pair`." It is exactly the type
the loop produces (a key/value pair); writing `var` saves you naming a verbose type and
reads fine because the right-hand side makes the type obvious.

This `MostFrequent` is the brain of the adaptive opponent. Give it your recent throws and it
tells the opponent what you favor — and from there, countering you is one step away.

## Lists vs dictionaries: which when?

| You want… | Use |
|-----------|-----|
| An ordered sequence; "what came in what order" | `List<T>` |
| Look something up by a key; "what value goes with X" | `Dictionary<TKey, TValue>` |
| To count how often each thing appears | `Dictionary` (the tally pattern) over a `List` |

You will use them together all the time, exactly as here: a list records the raw sequence,
a dictionary summarizes it.

## Vocabulary

**Dictionary (`Dictionary<TKey, TValue>`)** — A collection mapping unique keys to values,
with fast lookup by key.

**Key / Value** — The lookup handle and the thing it retrieves. In the loc file, the key is
the string, the value is the displayed text.

**Tally pattern** — Counting occurrences by walking a sequence and bumping a per-item count
in a dictionary.

**`ContainsKey`** — Asks whether a key exists, before reading it.

**`var`** — Lets the compiler infer a variable's type from its initializer.

## Things to look up

- "C# Dictionary" — `TryGetValue`, which combines the check-and-read in one step
- "C# var keyword" — when type inference helps readability and when it hurts it
- "C# KeyValuePair" — the type each dictionary `pair` actually is
