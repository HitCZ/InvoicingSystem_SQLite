using System;

namespace InvoicingSystem_SQLite.Logic.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsNullOrEmpty(this Array array) => array is null || array.Length == 0;
    }
}
