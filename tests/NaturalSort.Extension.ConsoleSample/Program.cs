using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NaturalSort.Extension;

// Code and data is based on issue: https://github.com/tompazourek/NaturalSort.Extension/issues/2
var items = File.ReadAllLines("sample-data.txt").ToArray();

Console.WriteLine("Item count: " + items.Length);

var stopwatch = new Stopwatch();

Console.WriteLine("Sort with StringComparer.OrdinalIgnoreCase");

stopwatch.Restart();
var ordered1 = items.OrderBy(i => i, StringComparer.OrdinalIgnoreCase).ToList();
stopwatch.Stop();

Console.WriteLine("Duration " + stopwatch.Elapsed);

Console.WriteLine("Sort with StringComparison.OrdinalIgnoreCase.WithNaturalSort()");

stopwatch.Restart();
var ordered2 = items.OrderBy(i => i, StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToArray();
stopwatch.Stop();
Console.WriteLine("Duration " + stopwatch.Elapsed);

Console.ReadKey();
