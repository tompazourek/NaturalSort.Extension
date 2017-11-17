using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NaturalSort.Extension
{
    /// <summary>
    /// Extension for <see cref="StringComparer"/> that adds support for natural sorting.
    /// </summary>
    public static class StringComparerNaturalSortExtension
    {
        /// <summary>
        /// Enhances string comparer with natural sorting functionality,
        /// which allows it to sort numbers inside the strings as numbers, not as letters.
        /// (e.g. "1", "2", "10" instead of "1", "10", "2")
        /// </summary>
        /// <param name="stringComparer">Used string comparer</param>
        /// <returns>Returns comparer of strings that considers natural sorting.</returns>
        public static IComparer<string> WithNaturalSort(this StringComparer stringComparer) => new NaturalSortComparer(stringComparer);

        private class NaturalSortComparer : IComparer<string>
        {
            public NaturalSortComparer(StringComparer stringComparer)
            {
                _stringComparer = stringComparer;
            }

            /// <summary>
            /// String comparer used for comparing strings.
            /// </summary>
            private readonly StringComparer _stringComparer;

            /// <summary>
            /// Regex to find sequences of numbers to split.
            /// </summary>
            private static readonly Regex NumberSequenceRegex = new Regex(@"(\d+)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            public int Compare(string s1, string s2)
            {
                var tokens1 = Tokenize(s1);
                var tokens2 = Tokenize(s2);

                var zipCompare = tokens1.Zip(tokens2, TokenCompare).FirstOrDefault(x => x != 0);
                if (zipCompare != 0)
                    return zipCompare;

                var lengthCompare = tokens1.Length.CompareTo(tokens2.Length);
                return lengthCompare;
            }
            
            /// <summary>
            /// Splits inputs into tokens, where each token is either a number or a piece of string.
            /// </summary>
            private static string[] Tokenize(string s) => s == null ? new string[] { } : NumberSequenceRegex.Split(s);

            /// <summary>
            /// Parses string as a number, or returns 0 otherwise.
            /// </summary>
            private static ulong ParseNumberOrZero(string s) => ulong.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var result) ? result : 0;

            /// <summary>
            /// Compares two tokens.
            /// </summary>
            private int TokenCompare(string token1, string token2)
            {
                var number1 = ParseNumberOrZero(token1);
                var number2 = ParseNumberOrZero(token2);

                var numberCompare = number1.CompareTo(number2);
                if (numberCompare != 0)
                    return numberCompare;

                var stringCompare = _stringComparer.Compare(token1, token2);
                return stringCompare;
            }
        }
    }
}