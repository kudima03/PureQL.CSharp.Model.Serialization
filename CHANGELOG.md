# Changelog

All notable changes to PureQL.CSharp.Model.Serialization are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
Versioning mirrors the PureQL specification with a `-csharp.N` suffix where needed.

---

## [Unreleased]

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
