# Contributing to Gsemac.Common

## Style Guide

The following style guide is mostly to help keep myself consistent.

## Design

### Classes

#### Configuration Classes

* Options classes should have a static `Default` property that returns an instance configured with default options.
* Options classes should always appear last in the parameter list.
* Prefer options classes over enums.

#### Factory Classes

* If the `Create` method does not take any arguments, inherit from `IFactory<T>`.

### Exceptions

* Treat `null` strings the same as empty strings where possible rather than throwing an exception.

### Extension Methods

* If an extension method does more than just remapping arguments, consider adding it to a utility class (with the `Utilities` suffix). This enhances discoverability and allows the method to be used without importing other extension methods into the current scope.

## Naming Conventions

### Classes

#### Configuration Classes

* If the class provides required configuration details, use the `Configuration` suffix.
* If the class provides optional configuration details, use the `Options` suffix.

#### Factory Classes

* If there is a need for multiple `Create` methods, use methods with the `From` prefix instead (`FromStream`, `FromFile`, `FromBitmap`, etc.). For methods taking a `string` argument, use `Parse`.

### Capitalization Conventions

The following rules are consistent with .NET's [capitalization conventions](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions).

| Use           | Not           |
| ------------- | ------------- |
| `FileName`    | `Filename`    |
| `Id`          | `ID`          |
| `Ok`          | `OK`          |
| `UserName`    | `Username`    |
| `WhiteSpace`  | `Whitespace`  |