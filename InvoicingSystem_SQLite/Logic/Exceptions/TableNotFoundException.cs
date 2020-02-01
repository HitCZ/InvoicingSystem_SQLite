using System;

namespace InvoicingSystem_SQLite.Logic.Exceptions
{
    public class TableNotFoundException : Exception
    {
        public TableNotFoundException(Type type) 
            : base($"No corresponding table for type {type.Name} has been found.")
        {
        }
    }
}
