using System;
using System.Collections.Generic;

namespace NaturalSort.Extension
{
    /// <summary>
    /// Extension for <see cref="StringComparer" /> that adds support for natural sorting.
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
        public static IComparer<string> WithNaturalSort(this StringComparer stringComparer)
            => new NaturalSortComparer(stringComparer);

        private class NaturalSortComparer : IComparer<string>
        {
            /// <summary>
            /// String comparer used for comparing strings.
            /// </summary>
            private readonly StringComparer _stringComparer;

            // Token values (not an enum as a performance micro-optimization)
            private const byte TokenNone = 0;
            private const byte TokenOther = 1;
            private const byte TokenDigits = 2;
            private const byte TokenLetters = 3;

            public NaturalSortComparer(StringComparer stringComparer)
                => _stringComparer = stringComparer;

            public int Compare(string str1, string str2)
            {
                if (str1 == str2) return 0;
                if (str1 == null) return -1;
                if (str2 == null) return 1;

                var strLength1 = str1.Length;
                var strLength2 = str2.Length;

                var startIndex1 = 0;
                var startIndex2 = 0;

                while (true)
                {
                    // get next token from string 1
                    var endIndex1 = startIndex1;
                    var token1 = TokenNone;
                    while (endIndex1 < strLength1)
                    {
                        var charToken = GetTokenFromChar(str1[endIndex1]);
                        if (token1 == TokenNone)
                        {
                            token1 = charToken;
                        }
                        else if (token1 != charToken)
                        {
                            break;
                        }

                        endIndex1++;
                    }

                    // get next token from string 2
                    var endIndex2 = startIndex2;
                    var token2 = TokenNone;
                    while (endIndex2 < strLength2)
                    {
                        var charToken = GetTokenFromChar(str2[endIndex2]);
                        if (token2 == TokenNone)
                        {
                            token2 = charToken;
                        }
                        else if (token2 != charToken)
                        {
                            break;
                        }

                        endIndex2++;
                    }

                    // if the token kinds are different, compare just the token kind
                    var tokenCompare = token1.CompareTo(token2);
                    if (tokenCompare != 0)
                        return tokenCompare;

                    // now we know that both tokens are the same kind
                    var rangeLength1 = endIndex1 - startIndex1;
                    var rangeLength2 = endIndex2 - startIndex2;

                    if (token1 == TokenDigits)
                    {
                        // compare both tokens as numbers
                        var maxLength = Math.Max(rangeLength1, rangeLength2);

                        // both spans will get padded by zeroes on the left to be the same length
                        const char paddingChar = '0';
                        var paddingLength1 = maxLength - rangeLength1;
                        var paddingLength2 = maxLength - rangeLength2;

                        for (var i = 0; i < maxLength; i++)
                        {
                            var digit1 = i < paddingLength1 ? paddingChar : str1[startIndex1 + i - paddingLength1];
                            var digit2 = i < paddingLength2 ? paddingChar : str2[startIndex2 + i - paddingLength2];

                            var digitCompare = digit1.CompareTo(digit2);
                            if (digitCompare != 0)
                                return digitCompare;
                        }

                        // if the numbers are equal, we compare how much we padded the strings
                        var paddingCompare = paddingLength1.CompareTo(paddingLength2);
                        if (paddingCompare != 0)
                            return paddingCompare;
                    }
                    else
                    {
                        // compare both tokens as strings
                        var tokenString1 = str1.Substring(startIndex1, rangeLength1);
                        var tokenString2 = str2.Substring(startIndex2, rangeLength2);
                        var stringCompare = _stringComparer.Compare(tokenString1, tokenString2);
                        if (stringCompare != 0)
                            return stringCompare;
                    }

                    startIndex1 = endIndex1;
                    startIndex2 = endIndex2;
                }
            }

            private static byte GetTokenFromChar(char c)
                => c >= 'a'
                    ? c <= 'z'
                        ? TokenLetters
                        : c < 128
                            ? TokenOther
                            : char.IsLetter(c)
                                ? TokenLetters
                                : TokenOther
                    : c >= 'A'
                        ? c <= 'Z'
                            ? TokenLetters
                            : TokenOther
                        : c >= '0' && c <= '9'
                            ? TokenDigits
                            : TokenOther;
        }
    }
}