using Invoicing.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using InvoicingSystem_SQLite.Logic.Constants;

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
                { typeof(Address), Constants.ADDRESS_TABLE_NAME },
                { typeof(BankInformation), Constants.BANK_INFORMATION_TABLE_NAME },
                { typeof(Contractor), Constants.CONTRACTOR_TABLE_NAME },
                { typeof(Customer), Constants.CUSTOMER_TABLE_NAME },
                { typeof(Invoice), Constants.INVOICE_TABLE_NAME }
            });
        }

        public string GetTableNameByType(Type type)
        {
            var nameFound = typesTables.TryGetValue(type, out var tableName);

            return nameFound ? tableName : string.Empty;
        }
    }
}
