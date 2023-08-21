## Style Guide

The following style guide is mostly to help keep myself consistent.

### Classes

* Prefer extension methods when implementing new instance methods (e.g. `StringExtensions`).
* When completely reimplementing a class or interface, use the name of the original class (e.g. `ReadOnlyCollection`). This allows for drop-in replacement wherever the class is used.
* When partially implementing a class or adding new static methods, use name of the original class with the `Ex` suffix (e.g. `TaskEx`). This allows it to be easily used alongside the existing class (this convention was adopted from `Microsoft.Bcl.Async`).

### Namespaces

It's tempting to place polyfill classes inside of their respective BCL namespaces (e.g. putting `IReadOnlyCollection<T>` in the `System.Collections.Generic` namespace), but this should be avoided.

While conflicts can be mitigated through the use of preprocessor directives to conditionally include the class in the DLL depending on the target framework, this introduces complications when the library is included as a transitive dependency. If different referenced projects target different framework versions and, as a result, reference different versions of the library, we can encounter runtime errors where referenced types are not present.

Tools like [ILMerge](https://github.com/tom-englert/ILMerge.Fody) can help by embedding the library in the assembly, eliminating the possibilty of version conflicts. However, this places a significant burden on dependent projects.

* Prefix all BCL namespaces with `Gsemac.Polyfills` (e.g. `Gsemac.Polyfills.System.Collections.Generic`).