using System;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    public interface ITypeToTableMappingManager
    {
        string GetTableNameByType(Type type);
    }
}