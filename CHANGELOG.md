# Changelog

All notable changes to PureQL.CSharp.Model.Serialization are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
Versioning mirrors the PureQL specification with a `-csharp.N` suffix where needed.

---

## [0.1.0-preview.6.0.0] - 2026-11-09

Moves serialization to `PureQL.CSharp.Model 0.1.0-preview.14.0.0`, the model rewritten
with C# 15 unions on .NET 11. The JSON is unchanged: every document accepted before is
accepted now, except those the new model's types reject (see Changed).

### Changed

- **Targets .NET 11 only**, as the model does; the SDK is pinned by `global.json`.
- **One pass over the JSON** — values are read straight from the `Utf8JsonReader`
  instead of a buffered `JsonDocument`. A union looks ahead on a copy of the reader for
  what chooses its case and stops as soon as it is known; the node is then read once.
  Over the specification's valid fixtures, reading is about twice as fast and allocates
  about twenty times less, and writing is about twice as fast.
- **Generated readers and writers** — `PureQL.CSharp.Model.Serialization.Generator`
  generates them from the model by reflection (CI fails if they are out of date),
  replacing the registrations of a one-off script and the descriptor engine.
- **Flat dispatch** — a union reads its case through one `switch` over the node's
  signature: the operator or leaf kind, the declared type and, for `equal`, `notEqual`,
  `in` and the comparisons, the family of the operand's type. The probe unions, the
  operator families and the exact unions are gone with the model's.
- **Query variants** follow the schema's dependencies: a query with `pagination` is the
  variant that requires `orderBy`, and a subquery with `orderBy` but no `pagination` the
  one that requires `distinctBy`. Such documents without the required clause, which the
  specification never accepted, are now rejected. So are `min` / `max` / `sum` selectors
  of another type than the one the operator declares, which the model now types
  exactly.
- Properties are written in the order of the specification (the model's).
- A missing required value of the model (a `null` string or list, a default union) is
  a `JsonException` on write.
- In a source-generated context that holds a model type, the .NET 11 source generator
  reports SYSLIB1227 for each of the model's unions, whose cases it cannot tell apart;
  `PureQLTypeInfoResolver`, placed first, resolves them, so the warning can be
  suppressed on the context (see the README).

### Removed

- Package validation, until the first union-based release becomes the baseline.

## [0.1.0-preview.5.1.0] - 2026-10-26

### Added

- `PureQLTypeInfoResolver`, an `IJsonTypeInfoResolver` that serves the `JsonTypeInfo` of
  every model type from its converter and returns `null` for any other type, so a query
  can be serialized in a Native AOT or trimmed application with the `JsonTypeInfo`
  overloads of `JsonSerializer`, alone or combined with a source-generated context. CI
  now publishes a Native AOT application and round-trips every valid fixture through it.

### Fixed

- An operator declared nullable is now rejected unless the node gives a cause for it:
  a nullable operand or a `predicate`, and every operand nullable for `coalesce`. The
  `average`, `min` and `max` of a total query stay nullable whatever their operands are.
  Such documents were never valid under the specification, but were accepted before.

---

## [0.1.0-preview.5.0.1] - 2026-10-22

### Fixed

- Lists shorter than the specification's `minItems` are now rejected on read: operator
  operands need at least two items and query lists at least one. Documents with such
  lists were never valid under the specification, but were accepted before.

---

## [0.1.0-preview.5.0.0] - 2026-10-14

Moves serialization to `PureQL.CSharp.Model 0.1.0-preview.13.0.0`, which mirrors
PureQL specification `0.1.0-preview.2.0.1`. The JSON mapping changes, so documents
written by the previous release may not be accepted.

### Added

- Total queries (`aggregate`) and the converters for their expressions.

### Changed

- Operator nodes are chosen by `operator` and the type they declare; select items and
  group keys take the type of their `expression`.
- Renamed properties: `param_name` is now `param`, a join's `type` is now `kind`, the
  operand `value` of floor, ceiling, round and `in` is now `operand`, and `distinct`
  is now `distinctBy`. Group keys are referenced by alias.
- Every value has a single spelling on read and write: decimal literals as strings,
  time and datetime to the microsecond, datetime in UTC, lower case uuid, no repeated
  list literal value, and no `nullable: false`, `over: "group"` or `direction: "asc"`.
- Row, projection and group expression converters follow the exact unions of the
  specification, including `round` by declared type and ordered `uuid` and `boolean`.

---

## [0.1.0-preview.4.0.0] - 2026-10-09

Rewrites serialization for `PureQL.CSharp.Model 0.1.0-preview.12.0.0`, which mirrors
PureQL specification `0.1.0-preview.1.0.0`. All previous converters are replaced.

### Changed

- **`PureQLConverters`** now yields one converter per model type (788 types:
  queries, sources, joins, pagination, order items, select items, group keys, every
  `*Row` / `*Projection` / `*Group` expression, fields, parameters, keys, literals and
  lists).
- Variants are chosen as the schema chooses them: queries by `groupBy`, `from` /
  `join` by `subquery`, operator nodes by `operator`, leaves by shape and declared
  type, and `equal` / `notEqual` / `in` / comparisons / `orderBy` keys by the type
  family of their operand (`probe.*`).
- Reading is strict: unknown, duplicate or missing properties and values outside the
  literal formats raise `JsonException` with the JSON path.
- Writing omits schema defaults (`nullable: false`, default `over`, `direction: "asc"`,
  `distinct: false`), writes nullable literals as `"value": null` and every `datetime`
  with an offset.
- The converters no longer use reflection; the package is marked trim- and
  AOT-compatible on net8.0 and later.
- The assembly version is now derived from the release tag's major version, so
  `0.x` releases carry `AssemblyVersion` `0.0.0.0` (previous releases carried
  `1.0.0.0`) and `FileVersion` matches the release.

### Removed

- The public enum `ComparisonOperatorJsonModel` and all previous internal converters.

---

## [0.1.0-preview.2.0.1]

Migrates serialization support to `PureQL.CSharp.Model 0.1.0-preview.11.0.1`.

### Fixed

- **`*FieldConverter`** — all eight field converters now serialize the `type`
  field using the scalar type name (`"number"`, `"string"`, `"boolean"`,
  `"null"`, `"date"`, `"time"`, `"datetime"`, `"uuid"`) as required by the
  PureQL specification, instead of the array type name (`"numberArray"` etc.).
  This corrects spec-invalid JSON emitted by the previous release and fixes
  rejection of spec-valid JSON during deserialization.

---

## [0.1.0-preview.2.0.0]

Migrates serialization support to `PureQL.CSharp.Model 0.1.0-preview.11.0.0`
(spec `0.1.0-preview.0.2.0` through `0.1.0-preview.0.5.0`).

### Added — spec 0.1.0-preview.0.2.0

New converters for the per-row predicate family (`each*`):

- **`EachEqualityConverter`** and typed converters
  (`EachBooleanEqualityConverter`, `EachNumberEqualityConverter`,
  `EachStringEqualityConverter`, `EachDateEqualityConverter`,
  `EachTimeEqualityConverter`, `EachDateTimeEqualityConverter`,
  `EachUuidEqualityConverter`) — JSON discriminator `"operator": "eachEqual"`.
- **`EachComparisonConverter`** and typed converters
  (`EachNumberComparisonConverter`, `EachStringComparisonConverter`,
  `EachDateComparisonConverter`, `EachDateTimeComparisonConverter`,
  `EachTimeComparisonConverter`) — discriminators `eachGreaterThan`,
  `eachLessThan`, `eachGreaterThanOrEqual`, `eachLessThanOrEqual`.
- **`EachAndOperatorConverter`**, **`EachOrOperatorConverter`**,
  **`EachNotOperatorConverter`** — discriminators `eachAnd`, `eachOr`,
  `eachNot`.
- **`BooleanOrArrayReturningConverter`** — handles
  `OneOf<BooleanReturning, BooleanArrayReturning>` used by
  `Join.On` and `Query.Where`.
- **`NumberReturningOrArrayConverter`**, **`StringReturningOrArrayConverter`**,
  **`DateReturningOrArrayConverter`**, **`TimeReturningOrArrayConverter`**,
  **`DateTimeReturningOrArrayConverter`**, **`UuidReturningOrArrayConverter`** —
  handle `OneOf<XReturning, XArrayReturning>` for each-equality right-hand
  sides.

### Changed — spec 0.1.0-preview.0.2.0

- **`BooleanArrayReturningConverter`** extended to dispatch
  `EachComparison`, `EachEquality`, `EachAndOperator`, `EachOrOperator`,
  and `EachNotOperator`.
- **`JoinConverter`** updated: `on` is now deserialized as
  `OneOf<BooleanReturning, BooleanArrayReturning>` and the resulting
  `Join` is constructed from whichever variant is present.
- **`QueryConverter`** updated: `where` is now
  `OneOf<BooleanReturning, BooleanArrayReturning>?`.

### Added — spec 0.1.0-preview.0.3.0

New converters for per-row arithmetic:

- **`EachArithmeticConverter`** and typed converters
  (`EachAddConverter`, `EachSubtractConverter`, `EachMultiplyConverter`,
  `EachDivideConverter`) — discriminators `eachAdd`, `eachSubtract`,
  `eachMultiply`, `eachDivide`.
- **`EachDateAddDaysConverter`**, **`EachDateDiffDaysConverter`** —
  discriminators `eachDateAddDays`, `eachDateDiffDays`.
- **`EachDateTimeAddSecondsConverter`**, **`EachDateTimeDiffSecondsConverter`** —
  discriminators `eachDatetimeAddSeconds`, `eachDatetimeDiffSeconds`.
- **`EachTimeAddSecondsConverter`**, **`EachTimeDiffSecondsConverter`** —
  discriminators `eachTimeAddSeconds`, `eachTimeDiffSeconds`.

### Changed — spec 0.1.0-preview.0.3.0

- **`NumberArrayReturningConverter`** extended to dispatch `EachArithmetic`,
  `EachDateDiffDays`, `EachDateTimeDiffSeconds`, `EachTimeDiffSeconds`.
- **`DateArrayReturningConverter`** extended to dispatch `EachDateAddDays`.
- **`TimeArrayReturningConverter`** extended to dispatch `EachTimeAddSeconds`.
- **`DateTimeArrayReturningConverter`** extended to dispatch
  `EachDateTimeAddSeconds`.

### Added — spec 0.1.0-preview.0.5.0

- **`OrderByItemConverter`** — serializes `OrderByItem` with an optional
  `SortDirection` (`"asc"` | `"desc"`, default omitted when `Asc`).

### Fixed

- **`NumberReturningConverter`** now dispatches `Arithmetic`,
  `NumberAggregate`, and `Count`, covering the full `numericReturning`
  definition.
- **`DateReturningConverter`** now dispatches `DateAggregate`.
- **`TimeReturningConverter`** now dispatches `TimeAggregate`.
- **`DateTimeReturningConverter`** now dispatches `DateTimeAggregate`.
- **`StringReturningConverter`** now dispatches `StringAggregate`.
- **`NullFieldConverter`** added; `FieldConverter` extended to include
  `NullField` as the 8th union variant.
- Removed dead `BooleanReturningOrArrayConverter` from
  `EachEqualityRightConverters.cs`; the same type was already handled by
  `BooleanOrArrayReturningConverter` registered earlier in `PureQLConverters`.
