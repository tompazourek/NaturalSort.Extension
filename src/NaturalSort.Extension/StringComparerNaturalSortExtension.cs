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
                    GetNextToken(str1, strLength1, startIndex1, out var currentToken1, out var endIndex1);
                    GetNextToken(str2, strLength2, startIndex2, out var currentToken2, out var endIndex2);

                    // check if any string doesn't have any tokens left
                    if (currentToken1 == null && currentToken2 == null) return 0;
                    if (currentToken1 == null) return -1;
                    if (currentToken2 == null) return 1;

                    // if the token kinds are different, compare just the token kind
                    var currentTokenValue1 = currentToken1.Value;
                    var currentTokenValue2 = currentToken2.Value;

                    var tokenCompare = currentTokenValue1.CompareTo(currentTokenValue2);
                    if (tokenCompare != 0)
                        return tokenCompare;

                    // now we know that both tokens are the same kind

                    var currentTokenLength1 = endIndex1 - startIndex1;
                    var currentTokenLength2 = endIndex2 - startIndex2;

                    if (currentTokenValue1 == Token.Digits)
                    {
                        // compare both tokens as numbers
                        var maxLength = Math.Max(currentTokenLength1, currentTokenLength2);

                        // both spans will get padded by zeroes on the left to be the same length
                        const char paddingChar = '0';
                        var paddingLength1 = maxLength - currentTokenLength1;
                        var paddingLength2 = maxLength - currentTokenLength2;

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
                        var tokenString1 = str1.Substring(startIndex1, currentTokenLength1);
                        var tokenString2 = str2.Substring(startIndex2, currentTokenLength2);
                        var stringCompare = _stringComparer.Compare(tokenString1, tokenString2);
                        if (stringCompare != 0)
                            return stringCompare;
                    }

                    startIndex1 = endIndex1;
                    startIndex2 = endIndex2;
                }
            }

            private static void GetNextToken(string str, int strLength, int startIndex, out Token? parsedToken, out int endIndex)
            {
                Token? currentToken = null;

                for (endIndex = startIndex; endIndex < strLength; endIndex++)
                {
                    var c = str[endIndex];

                    Token charToken;
                    if (c < '0') // ASCII before '0'
                    {
                        charToken = Token.Other;
                    }
                    else if (c <= '9')  // between '0' and '9'
                    {
                        charToken = Token.Digits;
                    }
                    else if (c < 'A') // after '9' and before 'A'
                    {
                        charToken = Token.Other;
                    }
                    else if (c <= 'Z') // between 'A' and 'Z'
                    {
                        charToken = Token.Letters;
                    }
                    else if (c < 'a') // after 'Z' and before 'a'
                    {
                        charToken = Token.Other;
                    }
                    else if (c <= 'z') // between 'a' and 'z'
                    {
                        charToken = Token.Letters;
                    }
                    else if (char.IsLetter(c)) // checks unicode categories
                    {
                        charToken = Token.Letters;
                    }
                    else
                    {
                        charToken = Token.Other;
                    }

                    if (currentToken == null)
                    {
                        currentToken = charToken;
                    }
                    else if (currentToken != charToken)
                    {
                        parsedToken = currentToken;
                        return;
                    }
                }

                parsedToken = currentToken;
            }

            private enum Token
            {
                Other = 0,
                Digits = 1,
                Letters = 2,
            }
        }
    }
}