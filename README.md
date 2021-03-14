![NaturalSort.Extension logo](https://raw.githubusercontent.com/tompazourek/NaturalSort.Extension/master/assets/logo_32.png) NaturalSort.Extension
=====================

*Extension method for `StringComparison` or any `IComparer<string>` that adds support for natural sorting  (e.g. "abc1", "abc2", "abc10" instead of "abc1", "abc10", "abc2").*

[![Build status](https://img.shields.io/appveyor/ci/tompazourek/naturalsort-extension/master.svg)](https://ci.appveyor.com/project/tompazourek/naturalsort-extension)
[![Tests](https://img.shields.io/appveyor/tests/tompazourek/naturalsort-extension/master.svg)](https://ci.appveyor.com/project/tompazourek/naturalsort-extension/build/tests)
[![codecov](https://codecov.io/gh/tompazourek/NaturalSort.Extension/branch/master/graph/badge.svg?token=31JTU6543K)](https://codecov.io/gh/tompazourek/NaturalSort.Extension)
[![NuGet version](https://img.shields.io/nuget/v/NaturalSort.Extension.svg)](https://www.nuget.org/packages/NaturalSort.Extension/)
[![NuGet downloads](https://img.shields.io/nuget/dt/NaturalSort.Extension.svg)](https://www.nuget.org/packages/NaturalSort.Extension/)


The library is written in C# and released with an [MIT license](https://raw.githubusercontent.com/tompazourek/NaturalSort.Extension/master/LICENSE), so feel **free to fork** or **use commercially**.

**Any feedback is appreciated, please visit the [issues](https://github.com/tompazourek/NaturalSort.Extension/issues?state=open) page or send me an [e-mail](mailto:tom.pazourek@gmail.com).**

Download
--------

Binaries of the last build can be downloaded on the [AppVeyor CI page of the project](https://ci.appveyor.com/project/tompazourek/naturalsort-extension/build/artifacts).

The library is also [published on NuGet.org](https://www.nuget.org/packages/NaturalSort.Extension/) (prerelease), install using:

```
PM> Install-Package NaturalSort.Extension
```

<sup>NaturalSort.Extension is built for .NET v4.0 and .NET Standard 1.3.</sup>

Usage
-----

The recommended method for best results is to create the comparer by using the `StringComparison` enum.

```csharp
var sequence = new[] { "img12.png", "img10.png", "img2.png", "img1.png" };
var ordered = sequence.OrderBy(x => x, StringComparison.OrdinalIgnoreCase.WithNaturalSort());
// ordered will be "img1.png", "img2.png", "img10.png", "img12.png"
```

For more information about natural sort order, see: [Sorting for Humans: Natural Sort Order (Coding Horror)](https://blog.codinghorror.com/sorting-for-humans-natural-sort-order/).

The `NaturalSortComparer` created using the extension method is a `IComparer<string>`, which you can use in all the places that accept `IComparer<string>` (e.g. `OrderBy`, `Array.Sort`, ...)

If you wish, you can be more explicit by not using the `.WithNaturalSort()` extension method, and using the constructor directly:

```csharp
new NaturalSortComparer(StringComparison.OrdinalIgnoreCase)
```

Note that if you are using a custom `IComparer<string>` (or `StringComparer`), you can also use that instead of the `StringComparison` enum. **However, if you use `StringComparison`, it should perform better as it avoids constructing substrings.**
