# Contributing to Gsemac.Common

## Style Guide

The following style guide is mostly to help keep myself consistent.

### Classes

#### Configuration Classes

* If the class provides required configuration details, use the `Configuration` suffix.
* If the class provides optional configuration details, use the `Options` suffix.
* Options classes should have a static `Default` property that returns an instance configured with default options.
* Options classes should always appear last in the parameter list.
* Prefer options classes over enums.

#### Factory Classes

* If the `Create` method does not take any arguments, inherit from `IFactory<T>`.
* If there is a need for multiple `Create` methods, use methods with the `From` prefix instead (`FromStream`, `FromFile`, `FromBitmap`, etc.).

### Exceptions

* Treat `null` strings the same as empty strings where possible rather than throwing an exception.

### Naming Conventions

Naming conventions are chosen to be consistent with those [already present in .NET](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions).

* Prefer `FileName` over `Filename`.
* Prefer `UserName` over `Username`.
* Prefer `WhiteSpace` over `Whitespace`.