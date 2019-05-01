using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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
        public static IComparer<string> WithNaturalSort(this StringComparer stringComparer) => new NaturalSortComparer(stringComparer);

        private class NaturalSortComparer : IComparer<string>
        {
            /// <summary>
            /// String comparer used for comparing strings.
            /// </summary>
            private readonly StringComparer _stringComparer;

            public NaturalSortComparer(StringComparer stringComparer) => _stringComparer = stringComparer;

            public int Compare(string s1, string s2)
            {
                var tokens1 = Tokenize(s1).ToArray();
                var tokens2 = Tokenize(s2).ToArray();

                var zipCompare = tokens1.Zip(tokens2, TokenCompare).FirstOrDefault(x => x != 0);
                if (zipCompare != 0)
                    return zipCompare;

                var lengthCompare = tokens1.Length.CompareTo(tokens2.Length);
                return lengthCompare;
            }

            /// <summary>
            /// Splits inputs into tokens. Each token is either a number, piece of string, or a dot.
            /// </summary>
            private static IEnumerable<Token> Tokenize(string s)
            {
                if (s == null)
                    yield break;

                var currentTokenBuilder = new StringBuilder(s.Length);
                TokenKind? currentTokenKind = null;

                foreach (var c in s)
                {
                    var characterTokenKind = GetCharacterTokenKind(c);
                    if (currentTokenKind == characterTokenKind)
                    {
                        currentTokenBuilder.Append(c);
                        continue;
                    }

                    if (currentTokenBuilder.Length > 0 && currentTokenKind != null)
                        yield return new Token(currentTokenBuilder.ToString(), currentTokenKind.Value);

                    currentTokenBuilder.Clear().Append(c);
                    currentTokenKind = characterTokenKind;
                }

                if (currentTokenBuilder.Length > 0 && currentTokenKind != null)
                    yield return new Token(currentTokenBuilder.ToString(), currentTokenKind.Value);
            }

            private static TokenKind GetCharacterTokenKind(char c)
            {
                if (char.IsLetter(c))
                    return TokenKind.Letters;

                if (char.IsDigit(c))
                    return TokenKind.Digits;

                return TokenKind.Other;
            }

            /// <summary>
            /// Parses string as a number, or returns 0 otherwise.
            /// </summary>
            private static ulong ParseNumber(string s) => ulong.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var result) ? result : 0;

            /// <summary>
            /// Compares two tokens.
            /// </summary>
            private int TokenCompare(Token token1, Token token2)
            {
                // compare if the token kinds are different
                var tokenKindCompare = token1.Kind.CompareTo(token2.Kind);
                if (tokenKindCompare != 0)
                    return tokenKindCompare;

                if (token1.Kind == TokenKind.Digits)
                {
                    // compare if both tokens are digits
                    var number1 = ParseNumber(token1.Value);
                    var number2 = ParseNumber(token2.Value);

                    var numberCompare = number1.CompareTo(number2);
                    if (numberCompare != 0)
                        return numberCompare;
                }

                // compare as strings if both tokens are letters or other, or are the same number
                var stringCompare = _stringComparer.Compare(token1.Value, token2.Value);
                return stringCompare;
            }

            private enum TokenKind
            {
                Other = 0,
                Digits = 1,
                Letters = 2,
            }

            private struct Token
            {
                public Token(string value, TokenKind kind)
                {
                    Value = value;
                    Kind = kind;
                }

                public string Value { get; }
                public TokenKind Kind { get; }
            }
        }
    }
}