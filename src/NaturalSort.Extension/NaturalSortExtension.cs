﻿using System;
using System.Collections.Generic;

namespace NaturalSort.Extension;

/// <summary>
/// Extension for <see cref="StringComparer" /> that adds support for natural sorting.
/// </summary>
public static class NaturalSortExtension
{
    /// <summary>
    /// Enhances string comparer with natural sorting functionality
    /// which allows it to sort numbers inside the strings as numbers, not as letters.
    /// (e.g. "1", "2", "10" instead of "1", "10", "2").
    /// Note that using string comparison directly should perform better, consider <seealso cref="WithNaturalSort(System.StringComparison)" />.
    /// </summary>
    /// <param name="stringComparer">Used string comparer</param>
    /// <returns>Returns comparer of strings that considers natural sorting.</returns>
    public static NaturalSortComparer WithNaturalSort(this IComparer<string> stringComparer)
        => new(stringComparer);

    /// <summary>
    /// Uses given string comparison to create a comparer with natural sorting functionality
    /// which allows it to sort numbers inside the strings as numbers, not as letters.
    /// (e.g. "1", "2", "10" instead of "1", "10", "2").
    /// Using string comparison directly should perform better than <seealso cref="WithNaturalSort(System.Collections.Generic.IComparer{string})" />.
    /// </summary>
    /// <param name="stringComparison">Used string comparison</param>
    /// <returns>Returns comparer of strings that considers natural sorting.</returns>
    public static NaturalSortComparer WithNaturalSort(this StringComparison stringComparison)
        => new(stringComparison);

    /// <summary>
    /// Enhances string comparer with natural sorting functionality
    /// which allows it to sort numbers inside the strings as numbers, not as letters.
    /// (e.g. "1", "2", "10" instead of "1", "10", "2").
    /// Note that using string comparison directly should perform better, consider <seealso cref="WithNaturalSort(System.StringComparison, bool, bool)" />.
    /// </summary>
    /// <param name="stringComparer">Used string comparer</param>
    /// <param name="numberDescending">Whether or not to process numbers in descending order</param>
    /// <param name="stringDescending">Whether or not to process strings in descending order</param>
    /// <returns>Returns comparer of strings that considers natural sorting.</returns>
    public static NaturalSortComparer WithNaturalSort(this IComparer<string> stringComparer, bool numberDescending, bool stringDescending)
        => new(stringComparer, numberDescending, stringDescending);

    /// <summary>
    /// Uses given string comparison to create a comparer with natural sorting functionality
    /// which allows it to sort numbers inside the strings as numbers, not as letters.
    /// (e.g. "1", "2", "10" instead of "1", "10", "2").
    /// Using string comparison directly should perform better than <seealso cref="WithNaturalSort(System.Collections.Generic.IComparer{string}, bool, bool)" />.
    /// </summary>
    /// <param name="stringComparison">Used string comparison</param>
    /// <param name="numberDescending">Whether or not to process numbers in descending order</param>
    /// <param name="stringDescending">Whether or not to process strings in descending order</param>
    /// <returns>Returns comparer of strings that considers natural sorting.</returns>
    public static NaturalSortComparer WithNaturalSort(this StringComparison stringComparison, bool numberDescending, bool stringDescending)
        => new(stringComparison, numberDescending, stringDescending);
}
