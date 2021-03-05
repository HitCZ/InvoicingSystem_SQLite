using System.Collections.Generic;
using System.ComponentModel.Composition;
using Invoicing.Models;

namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    [InheritedExport]
    public interface ISqlDataProvider<T> where T : IDatabaseStorableObject
    {
        (string query, bool success) CreateOrUpdate(T item);
        List<(string query, bool success)> CreateOrUpdate(IEnumerable<T> items);

        /// <summary>
        /// It doesn't make sense to return the query.
        /// </summary>
        bool Delete(T item);

        List<(int index, bool success)> Delete(IEnumerable<T> items);
        T GetById(int id);
        IEnumerable<T> GetAll();

        /// <summary>
        /// Column name in database should be the same as name of the Property of given type.
        /// </summary>
        IEnumerable<T> GetBy(string columnName, string constraint);

        /// <summary>
        /// This is used to get a newly created db object and get its new ID.
        /// </summary>
        IEnumerable<T> GetByEverythingExceptId(T item);
    }
}