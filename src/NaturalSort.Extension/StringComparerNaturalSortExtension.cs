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

                    if (currentTokenValue1 == Token.Digits)
                    {
                        // compare both tokens as numbers

                        // detect if one number has greater magnitude than the other
                        var orderOfMagnitude1 = GetOrderOfMagnitude(str1, startIndex1, endIndex1);
                        var orderOfMagnitude2 = GetOrderOfMagnitude(str2, startIndex2, endIndex2);

                        var orderOfMagnitudeCompare = orderOfMagnitude1.CompareTo(orderOfMagnitude2);
                        if (orderOfMagnitudeCompare != 0)
                            return orderOfMagnitudeCompare;

                        // both numbers have the same magnitude, compare digit per digit
                        var digitIndex1 = endIndex1 - orderOfMagnitude1 - 1;
                        var digitIndex2 = endIndex2 - orderOfMagnitude2 - 1;

                        while (true)
                        {
                            var digit1 = str1[digitIndex1];
                            var digit2 = str2[digitIndex2];

                            var digitCompare = digit1.CompareTo(digit2);
                            if (digitCompare != 0)
                                return digitCompare;

                            digitIndex1++;
                            
                            if (digitIndex1 >= endIndex1)
                                break;

                            digitIndex2++;

                            if (digitIndex2 >= endIndex2)
                                break;
                        }

                        // the numbers are the same, compare number lengths
                        var numberLength1 = endIndex1 - startIndex1;
                        var numberLength2 = endIndex2 - startIndex2;
                        var numberLengthCompare = numberLength2.CompareTo(numberLength1); // intentionally in reverse order
                        if (numberLengthCompare != 0)
                            return numberLengthCompare;
                    }
                    else
                    {
                        // compare both tokens as strings
                        var tokenString1 = str1.Substring(startIndex1, endIndex1 - startIndex1);
                        var tokenString2 = str2.Substring(startIndex2, endIndex2 - startIndex2);
                        var stringCompare = _stringComparer.Compare(tokenString1, tokenString2);
                        if (stringCompare != 0)
                            return stringCompare;
                    }

                    startIndex1 = endIndex1;
                    startIndex2 = endIndex2;
                }
            }

            private static int GetOrderOfMagnitude(string str, int startIndex, int endIndex)
            {
                for (var i = startIndex; i < endIndex; i++)
                {
                    var digit = str[i];
                    if (digit == '0')
                        continue;

                    var orderOfMagnitude = endIndex - i - 1;
                    return orderOfMagnitude;
                }

                return 0;
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