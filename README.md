# PureQL.CSharp.Model.Serialization

`System.Text.Json` serialization support for **PureQL** query models — serialize and deserialize any `Query` and its constituent expressions to and from JSON.

[![.NET build & test](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/PureQL.CSharp.Model.Serialization)](https://www.nuget.org/packages/PureQL.CSharp.Model.Serialization)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

`PureQL.CSharp.Model.Serialization` provides `System.Text.Json` converters for every type of [`PureQL.CSharp.Model`](https://github.com/kudima03/PureQL.CSharp.Model), which mirrors PureQL specification [`0.1.0-preview.3.0.0`](https://github.com/kudima03/PureQL-Specification/releases/tag/0.1.0-preview.3.0.0). There are two entry points: `PureQLConverters` — a record that implements `IEnumerable<JsonConverter>` and yields one converter per model type, so a whole `PureQLQuery` or any part of it (an expression, a select item, a literal) can be converted — and `PureQLTypeInfoResolver`, an `IJsonTypeInfoResolver` over the same converters for applications that serialize without reflection (Native AOT, trimming, source generation).

## How JSON maps to the model

The model's unions are flat C# unions of records, and each record has one JSON shape, so variants are chosen the way the schema chooses them:

| JSON | Chosen by |
|---|---|
| Root query (`PureQLQuery`) | presence of a set operation (`unionAll`, `union`, `intersect`, `except`: a combined query), else of `groupBy` (grouped), else of `aggregate` (total), else plain; then of `pagination` (`…WithPagination`, which requires `orderBy`) |
| Subquery (`Query`, also a set operand and a recursive `seed`) | as a root query, then of `orderBy` without `pagination` (`…WithoutPaginationWithOrderBy`, which requires `distinctBy`) |
| `subqueries` item (`Subquery`) | presence of `rows` (inline rows), else of `recursive`, else a query |
| Recursive `next` (`NextQuery`) | presence of `groupBy` (grouped), else plain |
| `from`, `join` | presence of `subquery` (otherwise `entity`) |
| `orderBy` item of a query | presence of `column` (a `select` alias), otherwise `expression` |
| Window (`over`) | presence of `frame`, else of `range`, whose distances are chosen by the type the one sort key declares (`integer`, `decimal`, `date`, `datetime`), else a partition alone |
| A side of a frame or a range, `skip`, `take`, `maxDepth` | the JSON token: a bare number, the string `"all"` (an empty `All`), or an object (a parameter, a literal, `{ days }` / `{ months }` / `{ seconds }` by presence) |
| Operator nodes | `operator` and the declared `type`, which name exactly one record per union (`add` declared `decimal?` in `DecimalNullableRow` is `AddDecimalNullableRow`) |
| `equal`, `notEqual`, comparisons | also the type family of `left`: the `type.name` it declares, `integer` being of the decimal family |
| `in`, date and time parts, `asString`, `asDate`, `asTime`, `asDatetime`, `asDecimal` | also the type family of `operand` (`year` of a `date` is `YearDateIntegerRow`, of a `datetime` `YearDatetimeIntegerRow`); `integer` is of the decimal family except for `asDecimal`, which has an `integer` case |
| `round`, `truncate` | also whether `digits` is present (`RoundWithDigitsIntegerRow` keeps an `integer` operand, `RoundWithoutDigitsIntegerRow` rounds a `decimal` to a whole number) |
| Leaves | shape — `source` + `field` (field), `param` (parameter), `key` (group key), `subquery` (list column), `value` (literal) — then `type.name` and `nullable` |

Select items, group keys, inline-row cells and `orderBy` items are records over `Expr{Ctx}` (or `Cell`), whose case is chosen by the `expression` itself. A union reads ahead on a copy of the `Utf8JsonReader` only until its case is known, and the node is then read once, so documents are read in a single pass without buffering a `JsonDocument`.

Reading is strict: an unknown or duplicate property, a missing required one, a `nullable: false`, a default that the schema never writes (`over: "group"`, `direction: "asc"`, `mode: "halfAwayFromZero"`, `distinct: false`), a value that is not in the one spelling of its type, a list shorter than the schema's `minItems` (or longer than its `maxItems`: a range window has one sort key), an operator declared nullable that none of its operands (nor a `predicate`, nor a missing `default` of `previous` / `next`) makes nullable, an operator declared `decimal` whose number operands are all integers (`add`, `if`, `greatest`, … of integers is an `integer`), a non-null `coalesce` without exactly one non-null operand, or a value the model's CLR type cannot hold exactly is a `JsonException` whose message carries the JSON path. So is a value that breaks a rule of the schema that the model's types do not express, on read and on write:

- names (entities, fields, aliases, parameters, subqueries, columns) follow the schema's `NAME`: not empty, no leading or trailing space or tab, no line break;
- bare numbers (`skip`, `take`, `maxDepth`, the sides of a frame or a range) start from 1, as do a literal `buckets` and capture `group`; a literal `offset` starts from 2;
- a literal `fraction` is from 0 to 1, and a decimal distance of a range is more than 0;
- a literal regex `pattern` is in the specification's dialect, token by token (the schema's `REGEX`);
- a literal `zone` is one of the time zone names of the specification (`tools/time_zones.txt`);
- `pagination`, a frame and a range have at least one property, and a frame or a range is not `"all"` on both sides.

A query whose clauses the schema's dependencies reject (`pagination` without `orderBy`, a subquery's `orderBy` without `pagination` or `distinctBy`) has no model variant and is rejected too. Every invalid query of the specification is rejected. What the specification leaves to the interpreter is not checked: name resolution, unique aliases, a computed or bound zone, balanced parentheses of a pattern. The exact declared type and the `coalesce` rule are checked on read only: a model built in code is written as it is.

Writing omits schema defaults — `nullable: false`, `over` when it is the default (`group`; aggregates outside a group never write it), `direction: "asc"`, `mode` when it is half away from zero, a `distinct` that is `false` — and absent optional clauses, and writes properties in the specification's order. A value the model leaves missing (a `null` string or list, a `default` union) is a `JsonException`. A nullable literal is written as `"value": null`, a `decimal` as a string without exponent or trailing zeros, a `datetime` always in UTC with `Z` and a `uuid` in lower case.

Limits of the model's CLR types: `date` and `datetime` start at year 0001 (`DateOnly`, `DateTimeOffset`), and `time` / `datetime` have a precision of one microsecond, as the specification: a value finer than that, or a `decimal` of more than 28 digits, is a `JsonException` on write. A `DateTimeOffset` with an offset is written in UTC, keeping its instant.

The readers and writers are generated from the model ahead of time, use no reflection, and the library is trim- and AOT-compatible. CI publishes a Native AOT application that round-trips every fixture of the specification through `PureQLTypeInfoResolver`, so the usage below is checked in a trimmed binary, not only by the analyzers.

## Dependencies

- [`PureQL.CSharp.Model`](https://github.com/kudima03/PureQL.CSharp.Model) `0.1.0-preview.15.0.0`

## Target Frameworks

- .NET 11

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

### Native AOT and trimming

The overloads above resolve type metadata by reflection, which a trimmed or Native AOT application does not have. Use `PureQLTypeInfoResolver` instead of the converters and the `JsonTypeInfo` overloads of `JsonSerializer`:

```csharp
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using PureQL.CSharp.Model;
using PureQL.CSharp.Model.Serialization;

JsonSerializerOptions options = new JsonSerializerOptions
{
    MaxDepth = 256,
    TypeInfoResolver = new PureQLTypeInfoResolver(),
};
JsonTypeInfo<PureQLQuery> queryInfo = options.GetTypeInfo<PureQLQuery>();

PureQLQuery query = JsonSerializer.Deserialize(json, queryInfo)!;
string written = JsonSerializer.Serialize(query, queryInfo);
```

The resolver answers for model types only and returns `null` for any other, so a query that is part of a larger document combines with the application's source-generated context. Put the resolver first, so that model types are not generated as plain objects. Keep the model's types out of the context: the source generator walks every type a context reaches, and for the thousands of records and unions of `PureQL.CSharp.Model` `0.1.0-preview.15.0.0` that takes many minutes of build time (it did not finish in ten minutes for a context of one record holding a `PureQLQuery`, even with a `[JsonConverter]` on the property). Hold the query as `object`, which is written through its runtime type, and read it back with the resolver:

```csharp
[JsonSerializable(typeof(Envelope))]
internal sealed partial class DocumentContext : JsonSerializerContext;

// A PureQLQuery, written through its runtime type; read back as a JsonElement.
internal sealed record Envelope(string Name, object Query);

JsonSerializerOptions options = new JsonSerializerOptions
{
    MaxDepth = 256,
    TypeInfoResolver = JsonTypeInfoResolver.Combine(
        new PureQLTypeInfoResolver(),
        DocumentContext.Default
    ),
};
Envelope envelope = JsonSerializer.Deserialize(json, options.GetTypeInfo<Envelope>())!;
PureQLQuery query = ((JsonElement)envelope.Query).Deserialize(
    options.GetTypeInfo<PureQLQuery>()
);
```

## Generating the serializers

The readers and writers in `src/PureQL.CSharp.Model.Serialization/Generated` are generated from the referenced model. After a model bump, from `./src`:

```bash
dotnet run --project PureQL.CSharp.Model.Serialization.Generator
```

CI fails if the committed files differ from the generated ones.
