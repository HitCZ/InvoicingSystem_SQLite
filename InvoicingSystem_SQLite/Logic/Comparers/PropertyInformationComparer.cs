using InvoicingSystem_SQLite.DataAccess.SQL;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace InvoicingSystem_SQLite.Logic.Comparers
{
    public class PropertyInformationComparer : IComparer<PropertyInformation>
    {
        public int Compare(PropertyInformation x, PropertyInformation y)
        {
            if (x is null && y is null)
                return 0;
            if (x is null ^ y is null)
                return -1;

            var columnNameEquals = x.ColumnName == y.ColumnName;
            bool valueEquals;

            if (x.Value.IsNumeric())
                valueEquals = x.Value.ToString() == y.Value.ToString();
            else
                valueEquals = x.Value == y.Value;

            if (columnNameEquals && valueEquals)
                return 0;
            return -1;
        }
    }
}
