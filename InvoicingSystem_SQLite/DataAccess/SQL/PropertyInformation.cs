namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    public class PropertyInformation
    {
        public string ColumnName { get; }
        public object Value { get; }

        public PropertyInformation(string columnName, object value)
        {
            ColumnName = columnName;
            Value = value;
        }
    }
}
