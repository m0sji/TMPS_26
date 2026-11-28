# FragranceComposer

Console application for the TMPS laboratory works at UTM, FCIM, group FAF-243. It models a small perfume house. A perfumer composes fragrances from ingredients, layers them and releases them as products in the Luxury, Classic and Travel lines. The perfume house then produces these products in batches, and every production order goes through approval, maceration, bottling and shipping. Each lab adds a new group of design patterns to the same domain.

## Requirements

.NET 10 SDK. Check the installed version with:

```bash
dotnet --version
```

## Build

From the repository root:

```bash
dotnet build
```

## Run

```bash
dotnet run --project FragranceComposer
```

The program runs the demo scenarios from `Program.cs` and prints the result of every pattern to the console. Sections 1 to 7 come from the earlier labs. Sections 8, 9 and 10 show the State, Chain of Responsibility and Observer patterns on production orders.

## Interactive mode

```bash
dotnet run --project FragranceComposer -- --interactive
```

The interactive mode lets the user create production orders and change how the program behaves while it is running. The `--` separates the arguments of `dotnet run` from the arguments of the application. Commands are typed one per line and are not case sensitive.

| Command | What it does |
|---|---|
| `new <luxury\|classic\|travel> <aquatic\|gourmand> <quantity>` | Creates a production order in the Draft state. The line chooses the product line factory and the recipe chooses the fragrance built by the director. The audit log and the stock observer are subscribed to every new order |
| `approve <order id>` | Sends a Draft order through the approval chain and prints the decision |
| `macerate <order id>` | Starts the maceration of an Approved order |
| `bottle <order id>` | Bottles a Macerating order and takes the bottles from stock |
| `ship <order id>` | Ships a Bottled order |
| `cancel <order id>` | Cancels a Draft or Approved order |
| `subscribe <order id> <audit\|stock\|customer>` | Subscribes an observer to an order |
| `unsubscribe <order id> <audit\|stock\|customer>` | Unsubscribes an observer from an order |
| `toggle <safety\|stock>` | Turns the safety check or the stock check of the approval chain on or off. The change is used by orders created after it |
| `list` | Prints all orders with their state, the bottle stock and which checks are on |
| `exit` | Closes the program |

Example session:

```
> new classic gourmand 30
PO-1: 30 x Vanilla Night [Classic line]: Glass bottle (50 ml), Cardboard box, 802.65 EUR [Draft]
   subscribed: audit, stock

> approve po-1
PO-1 approve -> Draft
   safety check: Jasmine is 25.0%, the limit is 20.0%

> toggle safety
safety check OFF, stock check ON (used by orders created from now on)
```

A forbidden action, for example shipping an order that is still a Draft, prints `REJECTED` with the reason, and the order keeps its state.

## Test

```bash
dotnet test
```

This runs all xUnit tests from `FragranceComposer.Tests`. The tests do not need any console input. To see every test by name:

```bash
dotnet test --logger "console;verbosity=normal"
```

## Project structure

| Folder | Content |
|---|---|
| `FragranceComposer/` | the console application |
| `FragranceComposer/Client/` | the interactive mode |
| `FragranceComposer/States/` | the states of a production order |
| `FragranceComposer/Approvals/` | the approval chain |
| `FragranceComposer/Observers/` | the observers of a production order |
| `FragranceComposer.Tests/` | xUnit tests, one test class per pattern |
| `REPORT.md` | report for the current laboratory work |

Each laboratory work has its own branch: `lab1`, `lab2`, `lab3`, `lab4`.
