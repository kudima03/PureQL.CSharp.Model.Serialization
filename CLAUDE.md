# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory. The projects need the .NET 11 SDK (for the model's C# 15 unions); where the system SDK is older, use `~/.dotnet/dotnet` or put it first on `PATH` with `DOTNET_ROOT=$HOME/.dotnet`. The tests need the specification submodule; after cloning run `git submodule update --init` (CI checks it out with `submodules: true`).

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet run --project PureQL.CSharp.Model.Serialization.Generator   # regenerate Generated/ from the model
dotnet format --verify-no-changes          # check code style (CI enforces this)
dotnet format                              # auto-fix code style
dotnet test --no-build --verbosity normal  # run tests
dotnet pack --configuration Release -p:Version=<version> --output .
```

CI additionally passes `-p:AssemblyVersion` (pinned to the major) and `-p:FileVersion`; see `.github/workflows/publish-nuget.yml`.

## Architecture

This is a **JSON serialization library** for PureQL query models. It depends on `PureQL.CSharp.Model` `0.1.0-preview.14.1.0` (generated from PureQL specification `0.1.0-preview.2.0.1`; see its CLAUDE.md for the naming rules): sealed records, one per schema node, and flat C# 15 `union`s of them (structs implementing `IUnion`, one constructor per case, `Value` the record held). It adds `System.Text.Json` support on top.

**Entry points:** `PureQLConverters` — a `sealed record` that implements `IEnumerable<JsonConverter>` and yields one `PureQLConverter<T>` per model type (records and unions). Consumers register all converters into a `JsonSerializerOptions` instance in a single loop. `PureQLTypeInfoResolver` — an `IJsonTypeInfoResolver` that answers for the same types with a `JsonTypeInfo` built by `JsonMetadataServices.CreateValueInfo` over the type's converter and `null` for any other type, so it chains with a source-generated context through `JsonTypeInfoResolver.Combine` (the .NET 11 source generator reports SYSLIB1227 for the model's unions in such a context; suppress it there). It is the entry point for Native AOT and trimmed applications.

**Engine (hand-written, project root):** one pass over the `Utf8JsonReader`, no `JsonDocument`. A converter creates a `ReadContext` and calls the generated reader of its type; every reader starts on the value's first token and ends on its last, as a `JsonConverter` does.
- `ReadContext` keeps the JSON path as a stack, spelled only in errors (`JsonErrors.At`: message `… (at $.select[0].expression)`). It reads records (`Begin` / `Next` / `End` over a `RecordShape`: properties by index, the implied `operator` and `type` checked, duplicate, unknown and missing properties rejected), lists (`Many` with the schema's `minItems`: 2 for operator operands, 1 for query lists), names, enums and type objects (one spelling: no `nullable: false`).
- `ReadContext.Dispatch` — what unions read ahead, on a copy of the reader, stopping as soon as the case is known: `Signature` (`Kinds` operator or leaf kind, `TypeIds` declared type, and for `equal` / `notEqual` / `in` / comparisons the `Families` family of the type `left` / `operand` declares; packed by `Signature.Of`), `Clauses` (query variants) and `Has` (`from` / `join` by `subquery`).
- `ModelSerializers` (hand-written part) reads the unions chosen by presence: `PureQLQuery` / `Query` by `groupBy`, then `aggregate`, then `pagination` (and, for a subquery, `orderBy`), as the schema's dependencies split the variants; `From` / `Join` by `subquery`; `PaginationSkip` / `PaginationTake` (`long | ParamInteger`).
- `Writes` writes records (omitting schema defaults and absent optional values; a missing required value is a `JsonException`); `ModelRegistry` holds the converters, the record writers by runtime type (a union writes its `Value`), and which records declare a nullable type (`Nullability`).
- An operator declared nullable must have a nullable operand or a `predicate` (every operand nullable for `coalesce`); the check is generated only where the operand types do not already guarantee it, and not for the `average` / `min` / `max` of a total query, which are always nullable.
- `Literals/LiteralValues` — literal formats checked against the schema patterns (one spelling per value), parsed and formatted on the stack; values the schema or the CLR type cannot hold exactly are rejected on read and on write.

**Generated (`PureQL.CSharp.Model.Serialization/Generated/*.g.cs`):** written by `PureQL.CSharp.Model.Serialization.Generator`, which reflects over the model it references (keep its `PureQL.CSharp.Model` version equal to the library's). Never edit these files; change the generator and rerun it. CI regenerates and fails on any difference. Per record: a `RecordShape`, a reader (a `switch` over property indexes; locals passed to the constructor, required ones first as the model declares them) and a writer (`operator` and `type` first for operators, `type` first for literals and last for other leaves, then properties in the model's order, which is the schema's). Per union: a reader that `switch`es over the `Signatures` constants of its cases (the generator throws if two cases share a signature) and a writer of its `Value`. Also `Names` (encoded property names) and `Register` (every converter). The generator derives the operator from a record's name (longest operator prefix), the declared type from its `Type`, the family of a comparison from its name, JSON names from the constructor parameters, and `minItems` from a table that `MinItemsTests` checks against the schema.

**Test project:** `PureQL.CSharp.Model.Serialization.Tests` targets `net11.0` and uses xunit. The fixtures come from the git submodule `src/Tests/PureQL-Specification`, which tracks the spec's `main` branch (`branch = main` in `.gitmodules`; Dependabot opens a PR when it moves) and is copied to `Specification/` in the test output: `samples/valid/`, `samples/invalid/` and the schema. Land a submodule bump together with any model bump it needs. Valid fixtures must round-trip to semantically equal JSON (`JsonSemantics`: sorted keys, numbers by value, everything else as text, as every value has one spelling); invalid ones must fail, except those listed in `InvalidQueryTests.AcceptedByModel` (constraints the model's types cannot express). `SignatureTests` checks the signatures unions dispatch on, including the operand family of comparisons. `ModelRoundTripTests` builds every variant of every model type by reflection (`ModelSamples`: one per union case; operands are leaves, nullable for a nullable operator and non-null otherwise, of the operator's declared type where the operand admits it) and checks it reads back as the same variant. `MinItemsTests` cuts every list of those variants below and to its `minItems` (checked against the schema) and expects a rejection and an acceptance. `SchemaConformanceTests` validates the JSON written for each of those variants against the schema definition its type mirrors (JsonSchema.Net). It also builds every record declared nullable over non-null operands only (`ModelSamples.OverNonNull`) and expects the reader to reject it exactly when the schema does.

**Native AOT check:** `PureQL.CSharp.Model.Serialization.AotTests` is a console application with `PublishAot` (in the solution, so it builds under the trim and AOT analyzers with warnings as errors, but it is not an xunit project). CI publishes it and runs the native binary, which resolves every model type through `PureQLTypeInfoResolver` and round-trips every valid fixture, on its own and inside a source-generated `Envelope`. Locally: `dotnet publish Tests/PureQL.CSharp.Model.Serialization.AotTests -c Release -o <dir>` (add `-p:CppCompilerAndLinker=gcc` where clang is not installed) and run the binary.

**Target framework:** net11.0 only, as the model. `IsAotCompatible` is `true`; no reflection or `JsonSerializer` calls inside converters.

**Package validation:** enabled against `PackageValidationBaselineVersion` `0.1.0-preview.6.0.1`, the latest release; intentional breaks go to `CompatibilitySuppressions.xml` next to the csproj.

**Mutation testing:** `src/stryker-config.json` sets `language-version: Preview`, which Stryker 5.0.0 needs to parse C# 15. CI does not run it.

**Publishing:** triggered by pushing a semver tag (pattern `*.*.*`). The tag becomes the `PackageVersion`. The workflow publishes to both GitHub Packages and NuGet.org.

## Code Style

Enforced via `.editorconfig` and `dotnet format --verify-no-changes` in CI:

- No `var` — always use explicit types
- No expression-bodied methods or constructors — use block bodies
- Expression-bodied properties and accessors are required
- File-scoped namespace declarations (`namespace Foo.Bar;`)
- Private fields prefixed with `_` in camelCase (e.g., `_options`)
- Braces always required, even for single-statement blocks
- Max line length: 90 characters
- Allman brace style — opening brace on its own line for all blocks
- `System.*` using directives sorted first, no blank line between using groups
- Parentheses required in arithmetic and relational binary operators for clarity

## Commit Messages

Do not mention Claude or AI assistance in commit messages.
