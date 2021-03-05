using InvoicingSystem_SQLite.DataAccess.SQL;
using System.Collections.Generic;
using InvoicingSystem_SQLite.Logic.Extensions;

namespace InvoicingSystem_SQLite.Logic.Comparers
{
    public class PropertyInformationEqualityComparer : IEqualityComparer<PropertyInformation>
    {
        public bool Equals(PropertyInformation x, PropertyInformation y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null ^ y is null)
                return false;

            var columnNameEquals = x.ColumnName == y.ColumnName;
            bool valueEquals;

            if (x.Value.IsNumeric())
                valueEquals = x.Value.ToString() == y.Value.ToString();
            else
                valueEquals = x.Value == y.Value;

            if (columnNameEquals && valueEquals)
                return true;
            return false;
        }

        public int GetHashCode(PropertyInformation obj)
        {
            return obj.GetHashCode();
        }
    }
}
