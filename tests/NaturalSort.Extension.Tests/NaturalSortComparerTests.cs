using System;
using System.Linq;
using Xunit;

namespace NaturalSort.Extension.Tests
{
    public class NaturalSortComparerTests
    {
        private static void RunTests(string[] input, string[] expected)
        {
            RunTests_StringComparerOrdinalIgnoreCase(input, expected);
            RunTests_StringComparisonOrdinalIgnoreCase(input, expected);
        }

        private static void RunTests_StringComparerOrdinalIgnoreCase(string[] input, string[] expected)
        {
            var actual = input.OrderBy(x => x, StringComparer.OrdinalIgnoreCase.WithNaturalSort()).ToArray();
            Assert.Equal(expected, actual, StringComparer.Ordinal);
        }

        private static void RunTests_StringComparisonOrdinalIgnoreCase(string[] input, string[] expected)
        {
            var actual = input.OrderBy(x => x, StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToArray();
            Assert.Equal(expected, actual, StringComparer.Ordinal);
        }

        [Theory]
        [InlineData(new[] { "a", "b" }, new[] { "a", "b" })]
        [InlineData(new[] { "b", "a" }, new[] { "a", "b" })]
        [InlineData(new[] { "0", "1" }, new[] { "0", "1" })]
        [InlineData(new[] { "1", "0" }, new[] { "0", "1" })]
        [InlineData(new[] { "10.0401", "10.022" }, new[] { "10.022", "10.0401" })]
        public void TwoItems(string[] input, string[] expected) => RunTests(input, expected);

        [Theory]
        [InlineData(
            new[] { "10.0401", "10.022", "10.042", "10.021999" },
            new[] { "10.022", "10.042", "10.0401", "10.021999" }
        )]
        [InlineData(
            new[] { "2.2 sec", "1.9 sec", "1.53 sec" },
            new[] { "1.9 sec", "1.53 sec", "2.2 sec" }
        )]
        [InlineData(
            new[] { "2.2sec", "1.9sec", "1.53sec" },
            new[] { "1.9sec", "1.53sec", "2.2sec" }
        )]
        [InlineData(
            new[] { "1.txt", "1a.txt", "2.txt", "11.txt", "a.txt", "a1.txt", "a2.txt", "a11.txt", "aa.txt", "b.txt", "ba.txt", "bb.txt" },
            new[] { "1.txt", "1a.txt", "2.txt", "11.txt", "a.txt", "a1.txt", "a2.txt", "a11.txt", "aa.txt", "b.txt", "ba.txt", "bb.txt" }
        )]
        [InlineData(
            new[] { "!1", "!a", "!abc", "_abc", "{abc}", "1.9 sec", "1.53 sec", "1_2", "1_3", "1_txt", "2.2 sec", "8_txt", "12", "13_3", "99!txt", "99.txt", "99_txt", "999_txt", "a_txt", "a1_txt" },
            new[] { "!1", "!a", "!abc", "_abc", "{abc}", "1.9 sec", "1.53 sec", "1_2", "1_3", "1_txt", "2.2 sec", "8_txt", "12", "13_3", "99!txt", "99.txt", "99_txt", "999_txt", "a_txt", "a1_txt" }
        )]
        [InlineData(
            new[] { "x", "x x", "x!x", "x#x", "x%x", "x&x", "x(x", "x)x", "x,x", "x.x", "x;x", "x@x", "x[x", "x]x", "x^x", "x_x", "x{x", "x}x", "x~x", "x0x", "x1x", "x2x", "x3x", "x4x", "x5x", "x6x", "x7x", "x8x", "x9x", "xAx", "xBx", "xCx", "xDx", "xEx", "xFx", "xGx", "xHx", "xIx", "xJx", "xKx", "xLx", "xMx", "xNx", "xOx", "xPx", "xQx", "xRx", "xSx", "xTx", "xUx", "xVx", "xWx", "xx", "xXx", "xYx", "xZx" },
            new[] { "x", "x x", "x!x", "x#x", "x%x", "x&x", "x(x", "x)x", "x,x", "x.x", "x;x", "x@x", "x[x", "x]x", "x^x", "x_x", "x{x", "x}x", "x~x", "x0x", "x1x", "x2x", "x3x", "x4x", "x5x", "x6x", "x7x", "x8x", "x9x", "xAx", "xBx", "xCx", "xDx", "xEx", "xFx", "xGx", "xHx", "xIx", "xJx", "xKx", "xLx", "xMx", "xNx", "xOx", "xPx", "xQx", "xRx", "xSx", "xTx", "xUx", "xVx", "xWx", "xx", "xXx", "xYx", "xZx" }
        )]
        public void WindowsExplorer(string[] input, string[] expected) => RunTests(input, expected);

        /// <remarks>
        /// Data from: http://www.davekoelle.com/alphanum.html
        /// </remarks>
        [Theory]
        [InlineData(
            new[]
            {
                "z1.doc",
                "z10.doc",
                "z100.doc",
                "z101.doc",
                "z102.doc",
                "z11.doc",
                "z12.doc",
                "z13.doc",
                "z14.doc",
                "z15.doc",
                "z16.doc",
                "z17.doc",
                "z18.doc",
                "z19.doc",
                "z2.doc",
                "z20.doc",
                "z3.doc",
                "z4.doc",
                "z5.doc",
                "z6.doc",
                "z7.doc",
                "z8.doc",
                "z9.doc",
            },
            new[]
            {
                "z1.doc",
                "z2.doc",
                "z3.doc",
                "z4.doc",
                "z5.doc",
                "z6.doc",
                "z7.doc",
                "z8.doc",
                "z9.doc",
                "z10.doc",
                "z11.doc",
                "z12.doc",
                "z13.doc",
                "z14.doc",
                "z15.doc",
                "z16.doc",
                "z17.doc",
                "z18.doc",
                "z19.doc",
                "z20.doc",
                "z100.doc",
                "z101.doc",
                "z102.doc",
            }
        )]
        [InlineData(
            new[]
            {
                "1000X Radonius Maximus",
                "10X Radonius",
                "200X Radonius",
                "20X Radonius",
                "20X Radonius Prime",
                "30X Radonius",
                "40X Radonius",
                "Allegia 50 Clasteron",
                "Allegia 500 Clasteron",
                "Allegia 50B Clasteron",
                "Allegia 51 Clasteron",
                "Allegia 6R Clasteron",
                "Alpha 100",
                "Alpha 2",
                "Alpha 200",
                "Alpha 2A",
                "Alpha 2A-8000",
                "Alpha 2A-900",
                "Callisto Morphamax",
                "Callisto Morphamax 500",
                "Callisto Morphamax 5000",
                "Callisto Morphamax 600",
                "Callisto Morphamax 6000 SE",
                "Callisto Morphamax 6000 SE2",
                "Callisto Morphamax 700",
                "Callisto Morphamax 7000",
                "Xiph Xlater 10000",
                "Xiph Xlater 2000",
                "Xiph Xlater 300",
                "Xiph Xlater 40",
                "Xiph Xlater 5",
                "Xiph Xlater 50",
                "Xiph Xlater 500",
                "Xiph Xlater 5000",
                "Xiph Xlater 58",
            },
            new[]
            {
                "10X Radonius",
                "20X Radonius",
                "20X Radonius Prime",
                "30X Radonius",
                "40X Radonius",
                "200X Radonius",
                "1000X Radonius Maximus",
                "Allegia 6R Clasteron",
                "Allegia 50 Clasteron",
                "Allegia 50B Clasteron",
                "Allegia 51 Clasteron",
                "Allegia 500 Clasteron",
                "Alpha 2",
                "Alpha 2A",
                "Alpha 2A-900",
                "Alpha 2A-8000",
                "Alpha 100",
                "Alpha 200",
                "Callisto Morphamax",
                "Callisto Morphamax 500",
                "Callisto Morphamax 600",
                "Callisto Morphamax 700",
                "Callisto Morphamax 5000",
                "Callisto Morphamax 6000 SE",
                "Callisto Morphamax 6000 SE2",
                "Callisto Morphamax 7000",
                "Xiph Xlater 5",
                "Xiph Xlater 40",
                "Xiph Xlater 50",
                "Xiph Xlater 58",
                "Xiph Xlater 300",
                "Xiph Xlater 500",
                "Xiph Xlater 2000",
                "Xiph Xlater 5000",
                "Xiph Xlater 10000",
            }
        )]
        public void DaveKoelleAlphanum(string[] input, string[] expected) => RunTests(input, expected);

        /// <summary>
        /// Data from: https://github.com/yobacca/node-natural-sort
        /// (https://github.com/yobacca/node-natural-sort/blob/master/LICENSE.md)
        /// </summary>
        [Theory]
        [InlineData(
            new[] { "1.0.2", "1.0.1", "1.0.0", "1.0.9" },
            new[] { "1.0.0", "1.0.1", "1.0.2", "1.0.9" }
        )]
        [InlineData(
            new[] { "1.1.100", "1.1.1", "1.1.10", "1.1.54" },
            new[] { "1.1.1", "1.1.10", "1.1.54", "1.1.100" }
        )]
        [InlineData(
            new[] { "1.0.03", "1.0.003", "1.0.002", "1.0.0001" },
            new[] { "1.0.0001", "1.0.002", "1.0.003", "1.0.03" }
        )]
        [InlineData(
            new[] { "1.1beta", "1.1.2alpha3", "1.0.2alpha3", "1.0.2alpha1", "1.0.1alpha4", "2.1.2", "2.1.1" },
            new[] { "1.0.1alpha4", "1.0.2alpha1", "1.0.2alpha3", "1.1.2alpha3", "1.1beta", "2.1.1", "2.1.2" }
        )]
        [InlineData(
            new[] { "myrelease-1.1.3", "myrelease-1.2.3", "myrelease-1.1.4", "myrelease-1.1.1", "myrelease-1.0.5" },
            new[] { "myrelease-1.0.5", "myrelease-1.1.1", "myrelease-1.1.3", "myrelease-1.1.4", "myrelease-1.2.3" }
        )]
        [InlineData(
            new[] { "0001", "002", "001" },
            new[] { "0001", "001", "002" }
        )]
        [InlineData(
            new[]
            {
                "192.168.0.100",
                "192.168.0.1",
                "192.168.1.1",
                "192.168.0.250",
                "192.168.1.123",
                "10.0.0.2",
                "10.0.0.1"
            },
            new[]
            {
                "10.0.0.1",
                "10.0.0.2",
                "192.168.0.1",
                "192.168.0.100",
                "192.168.0.250",
                "192.168.1.1",
                "192.168.1.123"
            }
        )]
        [InlineData(
            new[] { "img12.png", "img10.png", "img2.png", "img1.png" },
            new[] { "img1.png", "img2.png", "img10.png", "img12.png" }
        )]
        [InlineData(
            new[] { "car.mov", "01alpha.sgi", "001alpha.sgi", "my.string_41299.tif", "organic2.0001.sgi" },
            new[] { "001alpha.sgi", "01alpha.sgi", "car.mov", "my.string_41299.tif", "organic2.0001.sgi" }
        )]
        [InlineData(
            new[]
            {
                "./system/kernel/js/01_ui.core.js",
                "./system/kernel/js/00_jquery-1.3.2.js",
                "./system/kernel/js/02_my.desktop.js"
            },
            new[]
            {
                "./system/kernel/js/00_jquery-1.3.2.js",
                "./system/kernel/js/01_ui.core.js",
                "./system/kernel/js/02_my.desktop.js"
            }
        )]
        [InlineData(
            new[] { "\u0044", "\u0055", "\u0054", "\u0043" },
            new[] { "\u0043", "\u0044", "\u0054", "\u0055" }
        )]
        [InlineData(
            new[]
            {
                "T78",
                "U17",
                "U10",
                "U12",
                "U14",
                "745",
                "U7",
                "485",
                "S16",
                "S2",
                "S22",
                "1081",
                "S25",
                "1055",
                "779",
                "776",
                "771",
                "44",
                "4",
                "87",
                "1091",
                "42",
                "480",
                "952",
                "951",
                "756",
                "1000",
                "824",
                "770",
                "666",
                "633",
                "619",
                "1",
                "991",
                "77H",
                "PIER-7",
                "47",
                "29",
                "9",
                "77L",
                "433"
            },
            new[]
            {
                "1",
                "4",
                "9",
                "29",
                "42",
                "44",
                "47",
                "77H",
                "77L",
                "87",
                "433",
                "480",
                "485",
                "619",
                "633",
                "666",
                "745",
                "756",
                "770",
                "771",
                "776",
                "779",
                "824",
                "951",
                "952",
                "991",
                "1000",
                "1055",
                "1081",
                "1091",
                "PIER-7",
                "S2",
                "S16",
                "S22",
                "S25",
                "T78",
                "U7",
                "U10",
                "U12",
                "U14",
                "U17"
            }
        )]
        [InlineData(
            new[]
            {
                "FSI stop, Position: 5",
                "Mail Group stop, Position: 5",
                "Mail Group stop, Position: 5",
                "FSI stop, Position: 6",
                "FSI stop, Position: 6",
                "Newsstand stop, Position: 4",
                "Newsstand stop, Position: 4",
                "FSI stop, Position: 5"
            },
            new[]
            {
                "FSI stop, Position: 5",
                "FSI stop, Position: 5",
                "FSI stop, Position: 6",
                "FSI stop, Position: 6",
                "Mail Group stop, Position: 5",
                "Mail Group stop, Position: 5",
                "Newsstand stop, Position: 4",
                "Newsstand stop, Position: 4"
            }
        )]
        [InlineData(
            new[] { "5D", "1A", "2D", "33A", "5E", "33K", "33D", "5S", "2C", "5C", "5F", "1D", "2M" },
            new[] { "1A", "1D", "2C", "2D", "2M", "5C", "5D", "5E", "5F", "5S", "33A", "33D", "33K" }
        )]
        [InlineData(
            new[] { "1", "02", "3" },
            new[] { "1", "02", "3" }
        )]
        [InlineData(
            new[] { "v1.100", "v1.1", "v1.10", "v1.54" },
            new[] { "v1.1", "v1.10", "v1.54", "v1.100" }
        )]
        [InlineData(
            new[] { "bar.1-2", "bar.1" },
            new[] { "bar.1", "bar.1-2" }
        )]
        public void NodeNaturalSort(string[] input, string[] expected) => RunTests(input, expected);

        /// <summary>
        /// In v2.2.0, this comparison appears to get stuck in an infinite loop whereas v2.1.0 handles it without any problems.
        /// See: https://github.com/tompazourek/NaturalSort.Extension/issues/4 (reported by El-Gor-do)
        /// </summary>
        [Fact]
        public void Issue4_OrdinalIgnoreCase_comparison_infinite_loop()
        {
            // action
            var result = StringComparer.OrdinalIgnoreCase.WithNaturalSort().Compare("abc", "Abc");

            // assert
            Assert.Equal(0, result);
        }
    }
}
