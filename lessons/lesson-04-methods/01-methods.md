# 01 — Methods

## What a method is

A method is a named block of code that you can run by calling its name.

You have already called methods without thinking about it. `Console.WriteLine("hello")` is a method call. `Rng.NextBool()` is a method call. `PlayerCmd.GainGold(...)` is a method call.

Now you are going to write your own.

The reason methods exist is simple: without them, every piece of logic you want to reuse has to be copied and pasted. Copy-paste code is a maintenance problem. If you later find a bug in that logic, you have to find and fix every copy. A method lets you write the logic once, give it a name, and call it anywhere.

Methods also let you name a concept. Instead of reading fifteen lines of arithmetic and asking "what is this doing?", you can read `DetermineOutcome(playerChoice, computerChoice)` and immediately know. The name communicates intent.

## Anatomy of a method

Here is a method that adds two integers and returns the result:

```csharp
private int Add(int a, int b)
{
    return a + b;
}
```

Breaking it down:

```
private   int    Add   (int a, int b)
   ^        ^     ^         ^
   |        |     |         |
access   return  name   parameters
modifier  type
```

**Access modifier** — `private` or `public`. Controls who can call this method. More on this in the next chapter.

**Return type** — The type of value the method hands back when it finishes. `int` means it returns an integer. If the method does not return anything, you write `void` here.

**Name** — What you call it. Method names in C# use PascalCase (first letter of every word capitalized).

**Parameters** — The inputs the method needs to do its job. Each parameter has a type and a name, separated by commas, enclosed in parentheses. If the method needs no inputs, the parentheses are empty: `()`.

**Body** — The code inside `{ }`. This is what runs when the method is called.

## void: when a method does not return a value

Some methods do work without handing anything back. They might print something, change state, or fire off a command. You use the return type `void` to say "this method returns nothing."

```csharp
private void PrintGreeting(string name)
{
    Console.WriteLine("Hello, " + name + "!");
}
```

You call it and it runs. There is no return value to capture.

```csharp
PrintGreeting("Jack");
// prints: Hello, Jack!
```

You can still use the `return` keyword inside a `void` method — but without a value. It just exits the method early:

```csharp
private void PrintPositive(int number)
{
    if (number <= 0) return;  // exit early, nothing to print
    Console.WriteLine(number);
}
```

## Returning a value

When the return type is not `void`, the method must reach a `return` statement with a value matching that type before it ends. The compiler will not let you forget.

```csharp
private int Double(int x)
{
    return x * 2;
}
```

To use the return value, you capture it:

```csharp
int result = Double(5);
Console.WriteLine(result);  // prints: 10
```

Or you use it directly in an expression:

```csharp
Console.WriteLine(Double(5) + 1);  // prints: 11
```

## Multiple parameters

A method can take as many parameters as it needs. They are listed in order, separated by commas.

```csharp
private string Describe(string item, int quantity)
{
    return quantity + "x " + item;
}

string label = Describe("Gold", 3);
// label is "3x Gold"
```

The caller must provide arguments in the same order as the parameters are defined. The names do not have to match — what matters is position and type.

## Calling a method

To call a method, write its name followed by parentheses. If the method needs arguments, put them inside the parentheses:

```csharp
// No arguments
bool flip = Rng.NextBool();

// One argument
PrintGreeting("Jack");

// Two arguments
Outcome result = DetermineOutcome(playerChoice, computerChoice);
```

When you call a method, execution jumps into the method body, runs the code there, and then returns to the line where you called it — bringing the return value with it if there is one.

## Examples from the mod

Here is `PickComputerChoice` from the Rock Paper Scissors event:

```csharp
private Choice PickComputerChoice()
{
    if (Rng.NextBool()) return Choice.Rock;
    if (Rng.NextBool()) return Choice.Paper;
    return Choice.Scissors;
}
```

- Return type: `Choice` (an enum — a type that represents one of several named options, covered in chapter 03)
- Parameters: none — it needs no input, it just picks something
- Body: uses `Rng.NextBool()` twice to choose

Because it is its own method, `Play` can just call `PickComputerChoice()` and get back a value without caring about the details:

```csharp
Choice computerChoice = PickComputerChoice();
```

Here is `DetermineOutcome`:

```csharp
private Outcome DetermineOutcome(Choice player, Choice computer)
{
    if (player == computer) return Outcome.Draw;

    return (player, computer) switch
    {
        (Choice.Rock,     Choice.Scissors) => Outcome.Win,
        (Choice.Paper,    Choice.Rock)     => Outcome.Win,
        (Choice.Scissors, Choice.Paper)    => Outcome.Win,
        _                                  => Outcome.Lose,
    };
}
```

- Return type: `Outcome`
- Parameters: two `Choice` values — `player` and `computer`
- Body: handles the draw case first, then uses a switch expression to check all winning combinations

Notice that `DetermineOutcome` knows nothing about gold, the event system, or the player object. It only compares two choices and returns an outcome. That is by design. Pure, focused methods are easier to read, test, and change.

## Vocabulary

**Method** — A named, reusable block of code.

**Return type** — The type of value a method sends back to its caller. `void` means nothing is returned.

**Parameter** — A named input declared in the method signature.

**Argument** — The actual value passed to a method when you call it.

**Method signature** — The access modifier, return type, name, and parameter list together.

**Call stack** — The chain of method calls currently executing. When `Play` calls `DetermineOutcome`, both are on the call stack at that moment.

## Things to look up

- "C# method syntax" — any C# reference will show you the full grammar
- "C# return statement" — what happens when you return early vs at the end
- "C# named arguments" — a way to pass arguments by parameter name instead of position
- "C# optional parameters" — how to give a parameter a default value so the caller can omit it
