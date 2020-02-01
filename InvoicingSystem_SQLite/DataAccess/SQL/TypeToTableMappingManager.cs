using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Invoicing.Models;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    public static class TypeToTableMappingManager
    {
        private static readonly IReadOnlyDictionary<Type, string> typesTables;

        static TypeToTableMappingManager()
        {
            typesTables = new ReadOnlyDictionary<Type, string>(new Dictionary<Type, string>
            {
                { typeof(Address), "Addresses" },
                { typeof(BankInformation), "BankInformation" },
                { typeof(Contractor), "Contractors" },
                { typeof(Customer), "Customers" },
                { typeof(Invoice), "Invoices" }
            });
        }

        public static string GetTableNameByType(Type type)
        {
            var nameFound = typesTables.TryGetValue(type, out var tableName);

            return nameFound ? tableName : string.Empty;
        }
    }
}
