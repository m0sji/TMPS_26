# FragranceComposer

Console application for the TMPS laboratory works (UTM, FCIM, FAF-243). It models a small perfume house: a perfumer composes fragrances from ingredients, layers them, and releases them as products in the Luxury, Classic and Travel lines. Each lab adds a new group of design patterns to the same domain.

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

The program runs the demo scenarios from `Program.cs` and prints the result of every pattern to the console.

## Test

```bash
dotnet test
```

This runs all xUnit tests from `FragranceComposer.Tests`. To see every test by name:

```bash
dotnet test --logger "console;verbosity=normal"
```

## Project structure

| Folder | Content |
|---|---|
| `FragranceComposer/` | the console application |
| `FragranceComposer.Tests/` | xUnit tests for the Composite, Proxy and Decorator patterns |
| `REPORT.md` | report for the current laboratory work |

Each laboratory work has its own branch: `lab1`, `lab2`, `lab3`.
