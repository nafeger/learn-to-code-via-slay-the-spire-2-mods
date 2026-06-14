# 02 — Fields, Constructors, and `this`

Three mechanics make a class work: **fields** (what it remembers), **constructors** (how
it gets set up), and **`this`** (how an object refers to itself). This chapter covers all
three with small examples before you build the real thing.

## Fields: an object's memory

A field is a variable that belongs to an object instead of to a method. It is declared
inside the class but outside any method, and it lives for as long as the object lives.

```csharp
public class Scoreboard
{
    private int playerWins;     // a field
    private int computerWins;   // a field
    private int draws;          // a field
}
```

Each object built from this class gets its **own** copy of these three fields. Two
scoreboards have two independent sets of counts. That is the difference between a field
and a local variable: a local variable belongs to one run of one method and disappears
when that method returns; a field belongs to the object and persists.

You have already met something field-like: `WIN_GOLD` in `RockPaperScissors` and
`BASE_GOLD` in `CoinFlip` are fields (specifically `const` fields). The `DynamicVars` you
have been using are fields too. Now you will declare your own non-constant fields — ones
whose values *change* over the object's life.

### `private` by default — same rule as methods

Notice the fields are `private`. The same logic from Lesson 4 applies, even more
strongly: an object's fields are its private business. Code outside the class should go
through methods, not poke at the data directly. If `playerWins` were public, any code
anywhere could set it to `-999` and corrupt the scoreboard. Keeping fields private means
the only way to change the score is to call `Record(...)` — and `Record` controls exactly
how the counts can change.

This is encapsulation in practice: the data is hidden, the behavior is the public door.

## Methods that use fields

A method inside the class can read and write the object's fields directly — it does not
need them passed in as parameters, because they belong to the same object.

```csharp
public class Scoreboard
{
    private int playerWins;
    private int computerWins;
    private int draws;

    public void RecordPlayerWin()
    {
        playerWins++;     // changes THIS object's field
    }
}
```

`RecordPlayerWin` takes no parameters, yet it changes data — because the data lives on the
object, and the method is part of that object. Compare this to `DetermineOutcome`, which
took everything as parameters and changed nothing. Both are valid; they are different
tools. Methods that operate on an object's own remembered state are what classes are for.

## Constructors: setup when an object is born

A **constructor** is a special method that runs once, automatically, when you write
`new`. Its job is to put a freshly created object into a valid starting state.

A constructor has no return type and is named exactly like the class:

```csharp
public class Scoreboard
{
    private int playerWins;

    public Scoreboard()    // constructor — note: no return type, same name as the class
    {
        playerWins = 0;    // start at a known state
    }
}
```

You have already *called* constructors. `new GoldVar(BASE_GOLD)` calls `GoldVar`'s
constructor with one argument. `CoinFlip`'s own constructor is right there in the code:

```csharp
public CoinFlip()
    : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
}
```

That `: base(...)` part calls the *parent* class's constructor — you will understand
exactly what that means in Lesson 7. For now, notice the shape: `public CoinFlip()` is a
constructor, named after its class, with no return type.

For numeric fields, C# already initializes them to `0`, so a constructor that only sets
things to zero is optional. You write a constructor when there is real setup to do — a
value that must be provided, or work that must happen before the object is usable.

## `this`: the object referring to itself

Inside a method, `this` means "the object this method is running on." Most of the time you
do not need to write it — `playerWins++` already means `this.playerWins++`. But `this`
becomes useful when a parameter name collides with a field name:

```csharp
public class Scoreboard
{
    private int winsNeeded;

    public Scoreboard(int winsNeeded)
    {
        this.winsNeeded = winsNeeded;
        // this.winsNeeded  -> the field
        //      winsNeeded  -> the parameter
    }
}
```

Without `this`, the line `winsNeeded = winsNeeded` would just assign the parameter to
itself and the field would stay `0`. `this.winsNeeded` disambiguates: "the field, not the
parameter."

You saw `this` in Lesson 4 already, passed as the first argument to `EventOption`:

```csharp
new EventOption(this, () => Play(Choice.Rock), "ROCK_PAPER_SCISSORS.options.ROCK")
```

There, `this` means "the current event object" — the event handing a reference to itself
to the `EventOption` so the option knows which event it belongs to. Same keyword, same
meaning: the object you are currently inside of.

## Putting the three together

```csharp
public class Counter
{
    private int count;          // field: state

    public Counter(int start)   // constructor: setup
    {
        this.count = start;     // this: the field, set from the parameter
    }

    public void Increment()     // behavior that reads/writes the field
    {
        count++;
    }

    public int Value()
    {
        return count;
    }
}
```

Usage:

```csharp
Counter c = new Counter(5);
c.Increment();
c.Increment();
int v = c.Value();   // v is 7
```

`c` is one object. It remembers `count` across the two `Increment()` calls because `count`
is a field, not a local. That memory across calls is the entire point.

## Vocabulary

**Field** — A variable that belongs to an object and persists for the object's lifetime.

**Constructor** — A special, return-type-less method named after the class that runs once
when the object is created with `new`.

**`this`** — A reference to the current object, used inside its own methods.

**Initialization** — Setting an object's fields to valid starting values, usually in the
constructor.

## Things to look up

- "C# fields vs properties" — properties are a tidier way to expose fields; you will meet
  them naturally later
- "C# constructor" — including constructors that take parameters
- "C# this keyword" — the full set of uses
