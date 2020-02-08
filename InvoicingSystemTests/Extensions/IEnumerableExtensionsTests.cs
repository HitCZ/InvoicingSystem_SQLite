using InvoicingSystem_SQLite.Logic.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace InvoicingSystemTests.Extensions
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class IEnumerableExtensionsTests
    {
        private const string VALUE1 = "value1";
        private const string VALUE2 = "value2";

        [TestMethod]
        public void JoinToStringsTest()
        {
            var values = new List<string> { VALUE1, VALUE2 };

            TestWithSeparatorWithSurround(values);
            TestWithSeparatorWithoutSurround(values);
            TestWithoutSeparatorWithSurround(values);
            TestWithoutSeparatorWithoutSurround(values);
        }

        private void TestWithSeparatorWithoutSurround(List<string> values)
        {
            var expected = $"{VALUE1}, {VALUE2}";
            var actual = values.JoinToStrings();

            Assert.AreEqual(expected, actual);
        }

        private void TestWithoutSeparatorWithoutSurround(List<string> values)
        {
            var expected = $"{VALUE1}{VALUE2}";
            var actual = values.JoinToStrings(string.Empty);

            Assert.AreEqual(expected, actual);
        }

        private void TestWithSeparatorWithSurround(List<string> values)
        {
            var surroundWith = ((char)34).ToString();
            var expected = $"\"{VALUE1}\", \"{VALUE2}\"";
            var actual = values.JoinToStrings(surroundWith: surroundWith);

            Assert.AreEqual(expected, actual);
        }

        private void TestWithoutSeparatorWithSurround(List<string> values)
        {
            var surroundWith = ((char)34).ToString();
            var expected = $"\"{VALUE1}\"\"{VALUE2}\"";
            var actual = values.JoinToStrings(string.Empty, surroundWith);

            Assert.AreEqual(expected, actual);
        }
    }
}
