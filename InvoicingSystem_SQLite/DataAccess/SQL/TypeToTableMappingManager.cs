using Invoicing.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    [Export(typeof(ITypeToTableMappingManager)), PartCreationPolicy(CreationPolicy.Shared)]
    public class TypeToTableMappingManager : ITypeToTableMappingManager
    {
        private readonly IReadOnlyDictionary<Type, string> typesTables;

        public TypeToTableMappingManager()
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

        public string GetTableNameByType(Type type)
        {
            var nameFound = typesTables.TryGetValue(type, out var tableName);

            return nameFound ? tableName : string.Empty;
        }
    }
}
