# PureQL.CSharp.Model.Serialization

`System.Text.Json` serialization support for **PureQL** query models — serialize and deserialize any `Query` and its constituent expressions to and from JSON.

[![.NET build & test](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/PureQL.CSharp.Model.Serialization/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/PureQL.CSharp.Model.Serialization)](https://www.nuget.org/packages/PureQL.CSharp.Model.Serialization)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

`PureQL.CSharp.Model.Serialization` provides a complete set of `System.Text.Json` converters for all PureQL model types. The single entry point is `PureQLConverters` — a record that implements `IEnumerable<JsonConverter>` and yields every converter needed to round-trip a `Query` through JSON.

Covered model categories:

| Category | Examples |
|---|---|
| Query structure | `Query`, `FromExpression`, `Join`, `SelectExpression`, `Pagination` |
| Boolean logic | `AndOperator`, `OrOperator`, `NotOperator`, `BooleanOperator` |
| Equalities | `BooleanEquality`, `NumberEquality`, `StringEquality`, `DateEquality`, `UuidEquality`, … |
| Array equalities | `BooleanArrayEquality`, `NumberArrayEquality`, … |
| Comparisons | `NumberComparison`, `StringComparison`, `DateComparison`, `DateTimeComparison`, … |
| Arithmetic | `Add`, `Subtract`, `Multiply`, `Divide` |
| Aggregates | `Min`, `Max`, `Average`, `Sum` for Number, Date, DateTime, Time, String |
| Scalars | `BooleanScalar`, `NumberScalar`, `StringScalar`, `NullScalar`, array variants, … |
| Parameters | `BooleanParameter`, `NumberParameter`, `NullParameter`, array variants, … |
| Returnings | `BooleanReturning`, `NumberReturning`, `StringReturning`, array variants, … |
| Fields | `Field`, `BooleanField`, `NumberField`, `StringField`, `UuidField`, … |
| Types | `BooleanType`, `NumberType`, `StringType`, `DateType`, `NullType`, array variants, … |

## Dependencies

- [`PureQL.CSharp.Model`](https://github.com/kudima03/PureQL.CSharp.Model/tree/0.1.0-preview.11.0.0) — core query model types: `Query`, `FromExpression`, `Join`, `SelectExpression`, `Pagination`, typed scalars, fields, parameters, returnings, and the full PureQL type system

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
using PureQL.CSharp.Model.Serialization;

JsonSerializerOptions options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
};

foreach (JsonConverter converter in new PureQLConverters())
{
    options.Converters.Add(converter);
}

// Serialize
string json = JsonSerializer.Serialize(query, options);

// Deserialize
Query restored = JsonSerializer.Deserialize<Query>(json, options)!;
```
