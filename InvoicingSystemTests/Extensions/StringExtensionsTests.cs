using InvoicingSystem_SQLite.Logic.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace InvoicingSystemTests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void FormatIntoNumbersTest()
        {
            var testConfigs = new List<(string input, char separator, int margin, string expectedResult)>
            {
                (null, ' ', 0, null),
                ("ab", ' ', 0, null),
                ("10", ' ', 0, "10"),
                ("1000", '.', 2, "10.00"),
                ("1000000", ' ', 3, "1 000 000")
            };

            foreach (var config in testConfigs)
            {
                var input = config.input;
                var separator = config.separator;
                var margin = config.margin;
                var expected = config.expectedResult;

                var actual = string.Empty;
                try
                {
                    actual = input.FormatIntoNumbers(separator, margin);
                }
                catch (Exception ex)
                {
                    var exType = ex.GetType();

                    if (input is null && exType != typeof(ArgumentNullException))
                        Assert.Fail();
                    if (!(input is null) && expected is null && exType != typeof(ArgumentException))
                        Assert.Fail();
                }

                if (actual == string.Empty)
                    continue;
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
