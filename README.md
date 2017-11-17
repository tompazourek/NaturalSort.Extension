NaturalSort.Extension
=====================

*Extension method for `StringComparer` that adds support for natural sorting  (e.g. "abc1", "abc2", "abc10" instead of "abc1", "abc10", "abc2").*

[![Build status](https://img.shields.io/appveyor/ci/tompazourek/naturalsort-extension.svg)](https://ci.appveyor.com/project/tompazourek/naturalsort-extension)
[![Tests](https://img.shields.io/appveyor/tests/tompazourek/naturalsort-extension.svg)](https://ci.appveyor.com/project/tompazourek/naturalsort-extension/build/tests)
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

<sup>NaturalSort.Extension is built for .NET v4.0, .NET v4.7, .NET Standard 1.3 and .NET Standard 2.0.</sup>

Usage
-----

Adds `.WithNaturalSort()` extension method to any `StringComparer` you wish to use.

You can use the enhanced comparer in all the places where you can use `IComparer<string>`, e.g. `.OrderBy()` or `.Sort()`.

Sample:

```csharp
var sequence = new[] { "img12.png", "img10.png", "img2.png", "img1.png" };
var ordered = sequence.OrderBy(x => x, StringComparer.OrdinalIgnoreCase.WithNaturalSort());
// ordered will be "img1.png", "img2.png", "img10.png", "img12.png"
```

For more information about natural sort order, see: [Sorting for Humans: Natural Sort Order (Coding Horror)](https://blog.codinghorror.com/sorting-for-humans-natural-sort-order/).