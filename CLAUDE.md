# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet format --verify-no-changes          # check code style (CI enforces this)
dotnet format                              # auto-fix code style
dotnet test --no-build --verbosity normal  # run tests
dotnet pack --configuration Release -p:PackageVersion=<version> --output .
```

## Architecture

This is a **JSON serialization library** for PureQL query models. It depends on `PureQL.CSharp.Model` for all model types and adds `System.Text.Json` support on top.

**Entry point:** `PureQLConverters` — a `sealed record` that implements `IEnumerable<JsonConverter>`. Iterating it yields every converter required to serialize and deserialize a `Query`. Consumers register all converters into a `JsonSerializerOptions` instance in a single loop.

**Converter organization:** converters live in subdirectories that mirror the model's logical groupings — `Aggregates/`, `Arithmetics/`, `ArrayEqualities/`, `ArrayParameters/`, `ArrayReturnings/`, `ArrayScalars/`, `BooleanOperations/`, `Comparisons/`, `Equalities/`, `Fields/`, `Parameters/`, `Returnings/`, `Scalars/`, `Types/`. Each type-specific subdirectory contains a `*JsonModel` record (the intermediate DTO), a typed `*Converter`, and a discriminated-union converter where needed.

**Top-level converters** (directly in the project root): `QueryConverter`, `SelectExpressionConverter`, `FromExpressionConverter`, `JoinConverter`, `PaginationConverter`.

**Test project:** `PureQL.CSharp.Model.Serialization.Tests` targets `net10.0` and uses xunit. Tests verify round-trip serialization for each converter type.

**Multi-targeting:** net7.0, net8.0, net9.0, net10.0. `IsAotCompatible` is explicitly set to `false`.

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
