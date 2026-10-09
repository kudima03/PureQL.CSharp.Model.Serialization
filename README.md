# PureQL.CSharp.Model.Serialization

`System.Text.Json` serialization support for **PureQL** query models — serialize and deserialize any `Query` and its constituent expressions to and from JSON.

[![.NET build & test](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/PureQL.CSharp.Model.Serialization)](https://www.nuget.org/packages/PureQL.CSharp.Model.Serialization)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

`PureQL.CSharp.Model.Serialization` provides `System.Text.Json` converters for every type of [`PureQL.CSharp.Model`](https://github.com/kudima03/PureQL.CSharp.Model), which mirrors PureQL specification [`0.1.0-preview.1.0.0`](https://github.com/kudima03/PureQL-Specification/releases/tag/0.1.0-preview.1.0.0). The single entry point is `PureQLConverters` — a record that implements `IEnumerable<JsonConverter>` and yields one converter per model type, so a whole `PureQLQuery` or any part of it (an expression, a select item, a literal) can be converted.

## How JSON maps to the model

Variants are chosen the way the schema chooses them:

| JSON | Chosen by |
|---|---|
| Root query, subquery (`PureQLQuery`, `Query`) | presence of `groupBy` |
| `from`, `join` | presence of `subquery` (otherwise `entity`) |
| Operator nodes | `operator`, which names exactly one record per value union (`add` in `DecimalNullableRow` is `AddDecimalNullableRow`); `round` with `digits` is `RoundDecimalDigits*` |
| Leaves | shape — `source` + `field` (field), `param_name` (parameter), `key` (group key), `subquery` (list column), `value` (literal) — then `type.name` and `nullable` |
| `equal`, `notEqual`, `in`, comparisons, `orderBy` keys | the type family of the left operand (`value` for `in`, the key itself for `orderBy`), read like `probe.*`: a leaf's `type.name`, a fixed-type operator, or `if.then`, `coalesce.values[0]`, an aggregate's `selector` |

Reading is strict: an unknown or duplicate property, a missing required one, or a value the model's CLR type cannot hold exactly is a `JsonException` whose message carries the JSON path. Constraints that no model type expresses are not checked: empty arrays (`minItems`), the `NAME` pattern of names and aliases, `skip` / `take` / `key` minimums, and the non-null operand that a non-null `coalesce` requires.

Writing omits schema defaults — `nullable: false`, `over` when it is the default (`group`; aggregates whose `over` has a single allowed value never write it), `direction: "asc"`, `distinct: false` — and absent optional clauses. A nullable literal is written as `"value": null`, a `datetime` always with an offset (`Z` for UTC, otherwise `±hh:mm`).

Limits of the model's CLR types: `date` and `datetime` start at year 0001 (`DateOnly`, `DateTimeOffset`), and `time` / `datetime` have a precision of 100 ns (fraction digits beyond the seventh must be `0`). A `datetime` offset beyond ±14:00, which `DateTimeOffset` cannot carry, keeps its instant and is written in UTC — the specification treats the offset as notation only.

The converters use no reflection and the library is trim- and AOT-compatible (analyzers enabled for net8.0 and later).

## Dependencies

- [`PureQL.CSharp.Model`](https://github.com/kudima03/PureQL.CSharp.Model) `0.1.0-preview.12.0.0`

## Target Frameworks

- .NET 7
- .NET 8
- .NET 9
- .NET 10

## Installation

```shell
dotnet add package PureQL.CSharp.Model.Serialization
```

## Usage

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using PureQL.CSharp.Model;
using PureQL.CSharp.Model.Serialization;

JsonSerializerOptions options = new JsonSerializerOptions
{
    // Deeply nested expressions can exceed the default depth of 64.
    MaxDepth = 256,
};

foreach (JsonConverter converter in new PureQLConverters())
{
    options.Converters.Add(converter);
}

PureQLQuery query = JsonSerializer.Deserialize<PureQLQuery>(json, options)!;
string written = JsonSerializer.Serialize(query, options);
```
